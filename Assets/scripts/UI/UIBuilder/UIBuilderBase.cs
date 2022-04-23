using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using realapp.Core;

namespace realapp.UIBuilder
{
    public abstract class UIBuilderBase : MonoBehaviour
    {
        private static Dictionary<string, Texture2D> downloadedTextures = new Dictionary<string, Texture2D>();
        [SerializeField] protected GameObject buttonTemplate;
        [SerializeField] protected Texture2D defaultImage;

        #region Build UI
        protected abstract void GatherUIBUildInformation(); // method to download and set the build information 
        protected abstract void GatherUITextures(); // method to download and get all images 
        protected abstract void DrawUI(); // method to draw the UI
        #endregion

        #region loadAssets
        protected void OnEnable() => Tokens.Instance.OnLoadAssets += LoadAssets;
        private void OnDisable() { if (!(Tokens.Instance is null)) { Tokens.Instance.OnLoadAssets -= LoadAssets; } }
        protected void LoadAssets(object sender, System.EventArgs e) => GatherUIBUildInformation();
        #endregion

        #region Gather Information
        //called to download json and retirve the information 
        protected void GetUIBuildInformation<T>(string url, string category, Action<T[]> onInformationDone) where T : struct
        {
            StartCoroutine(DownloadJSON(url, Tokens.Instance.TokenReceived, (string error) =>
            {
                Debug.LogError(error + " ,while dowloading JSON", gameObject);
            },
            (string jsonString) =>
            {
                onInformationDone(GetInformationObject<T>(jsonString, category));
            }
            ));
        }
        //extracts all iformation from json 
        protected virtual T[] GetInformationObject<T>(string jsonString, string category)
        {
            JSONObject json = new JSONObject(jsonString);
            JSONObject jsonCategory = json.GetField(category);
            return JsonHelper.GetArray<T>(jsonCategory.ToString());
        }
        // called to download images for icons 
        protected void GetUIIcons<T>(T[] allBuildInformation,
            Func<T, string> getElementTextureURL,
            Action<Texture2D, int> setElementTexture,
            Action onAllImagesDownloaded) where T : struct
        {
            int imagesDone = 0; // index to check if all images have been downloaded

            for (int i = 0; i < allBuildInformation.Length; i++)
            {
                string url = getElementTextureURL(allBuildInformation[i]);

                StartCoroutine(DownloadImages(url, i,
                    (string error) => { Debug.LogError(error + " ,while dowloading Image", gameObject); },
                    (Texture2D texture, int index) =>
                        {
                            setElementTexture(texture, index);
                        },
                    () =>
                        {
                            imagesDone++;
                            if (imagesDone == allBuildInformation.Length) { onAllImagesDownloaded(); }
                        }
                    ));
            }
        }
        protected Sprite GenerateSprite(Texture2D texture)
        {
            return Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), Vector2.zero, 1f);
        }
        #endregion

        #region Download Managment
        private IEnumerator DownloadJSON(string url, string _token, Action<string> onError, Action<String> onSuccess)
        {
            UnityWebRequest request = UnityWebRequest.Get(url);
            request.SetRequestHeader("Authorization", _token);

            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                onError("Error: " + request.error + " -> on Url " + request.url);
                ErrorHandeling.Instance.HandleError(request.error);
            }
            else
            {
                onSuccess(request.downloadHandler.text);
            }
        }
        private IEnumerator DownloadImages(string url, int index, Action<string> onError, Action<Texture2D, int> onSuccess, Action onDone)
        {
            UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
            //prevents redownloading the same image two or more times 
            if (downloadedTextures.ContainsKey(url))
            {
                yield return new WaitUntil(() => !(downloadedTextures[url] is null));
                onSuccess(downloadedTextures[url], index);
            }
            else
            {
                //creates a key to prevent many objects downloading the same image at the same time 
                downloadedTextures.Add(url, null);
                yield return request.SendWebRequest();
                if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                {
                    onError("Error: " + request.error + " -> on Url " + request.url);
                    downloadedTextures[url] = defaultImage;
                }
                else
                {
                    Texture2D texture = DownloadHandlerTexture.GetContent(request);
                    //adds the texture to the key that was creted 
                    Debug.Log(texture.format);
                    texture.Compress(false);
                    Debug.Log(texture.format);
                    downloadedTextures[url] = texture;
                    onSuccess(texture, index);
                }
            }
            onDone();
        }
        #endregion
    }

}