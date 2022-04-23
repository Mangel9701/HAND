using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MText;
using System;
using UnityEngine.Networking;

public class RebuildScene : MonoBehaviour
{
    public ARObject[] ARObjects;
    private List<AssetBundle> bundles = new List<AssetBundle>();
    private List<Texture2D> textures = new List<Texture2D>();
    [SerializeField] private GameObject parametricText;
    [SerializeField] private GameObject blockingCanvas;
    [SerializeField] private ProgressBar bar;
    private Transform plane;
    public TextAsset JSONFile;

    private void OnEnable()
    {
        PlaneHasSpawn.onPlaneSpawn += StartRecreating;
        Tokens.Instance.OnLoadAssets += startDownloading;
    }
    private void OnDisable()
    {
        PlaneHasSpawn.onPlaneSpawn -= StartRecreating;
        Tokens.Instance.OnLoadAssets -= startDownloading;
    }

    private void startDownloading(object sender, EventArgs e)
    {
        GetSceneAR();
    }
    public void StartRecreating(object sender, PlaneHasSpawn.OnPlaneSpawnArgs e)
    {
        plane = e.transform;
        PlaneHasSpawn.onPlaneSpawn -= StartRecreating;
    }
    private void GetSceneAR()
    {
        string url = $"http://api.realappar.com:80/message/getMessageById/{Tokens.Instance.RecreationMsgId}";
        StartCoroutine(GetJsonFile(url,
        (string error) =>
        {
            Debug.LogError("could not retrive the json file to rebuild scene ==> " + error);
            Application.Quit();
        },
        (string jsonFile) =>
        {
            JSONObject json = new JSONObject(jsonFile);
            JSONObject objects = json.GetField("arMessages");
            ARObjects = JsonHelper.GetArray<ARObject>(objects.ToString());
            BuildSceneAR();
        }
        ));
    }
    private void BuildSceneAR()
    {
        int objectsDownloaded = 0;
        foreach (ARObject obj in ARObjects)
        {
            StartCoroutine(GetSceneContent(obj,
                    (string error, string url) => { Debug.LogError("Error: " + error + " while downloading " + url); },
                    (object _object) => { StartCoroutine(AddToScene(_object, obj)); },
                    () => { HandleProgressBar(objectsDownloaded++); }));
        }
    }

    private void HandleProgressBar(int objectsDownloaded)
    {
        if (bar.MaxProgress != ARObjects.Length) bar.MaxProgress = ARObjects.Length;
        bar.SetProgressValue(objectsDownloaded);
        if (objectsDownloaded == ARObjects.Length) { bar.gameObject.SetActive(false); }
    }

    private IEnumerator AddToScene(object _object, ARObject obj)
    {
        yield return new WaitUntil(() => !(plane is null));
        // blockingCanvas.SetActive(false);
        GameObject instance = null;
        switch (obj.type)
        {
            case ARObjectType.Object:
                instance = Create3DObject(_object);
                break;
            case ARObjectType.Text:
                instance = Create3DText(_object, obj);
                break;
        }
        if (instance != null)
        {
            instance.transform.localScale = obj.scale;
            instance.transform.localRotation = obj.rotation;
            instance.transform.localPosition = obj.position;
            instance.GetComponent<Collider>().enabled = false;
        }
    }
    private GameObject Create3DObject(object _object)
    {
        GameObject instance;
        AssetBundle bundle = _object as AssetBundle;
        String RootAssetPath = bundle.GetAllAssetNames()[0];
        GameObject arObject = bundle.LoadAsset(RootAssetPath) as GameObject;
        instance = Instantiate(arObject, plane);
        return instance;
    }
    private GameObject Create3DText(object _object, ARObject obj)
    {
        GameObject instance;
        Texture2D texture = _object as Texture2D;
        textures.Add(texture);
        instance = Instantiate(parametricText, plane);
        Modular3DText text = instance.GetComponentInChildren<Modular3DText>();
        text.Text = obj.text;
        Material mat = text.Material;
        mat.SetColor("_Color", obj.color);
        mat.SetFloat("_Smoothness", obj.roughness);
        mat.SetFloat("_Metallic", obj.material);
        if (!obj.textureUrl.Contains("Ellipse")) { mat.SetTexture("_MainTex", texture); }

        return instance;
    }

    private IEnumerator GetJsonFile(string url, Action<string> onError, Action<string> onSuccess)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("Authorization", Tokens.Instance.TokenReceived);

            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success) { onError(request.error); }
            else { onSuccess(request.downloadHandler.text); }
        }
    }
    private IEnumerator GetSceneContent(ARObject obj, Action<string, string> onError, Action<object> onSuccess, Action onDone)
    {
        using (UnityWebRequest request =
            (obj.type == ARObjectType.Text) ?
                UnityWebRequestTexture.GetTexture(obj.textureUrl) : UnityWebRequestAssetBundle.GetAssetBundle(obj.objectUrl))
        {
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success) { onError(request.error, obj.type == ARObjectType.Text ? obj.textureUrl : obj.objectUrl); }
            else
            {
                object _object = null;
                switch (obj.type)
                {
                    case ARObjectType.Object:
                        _object = DownloadHandlerAssetBundle.GetContent(request);
                        break;
                    case ARObjectType.Text:
                        _object = DownloadHandlerTexture.GetContent(request);
                        break;
                }
                onSuccess(_object);
            }
            onDone();
        }
    }
}