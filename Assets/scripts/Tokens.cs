using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Tokens : Singleton<Tokens>
{
    [Serializable]
    struct InfoRecieved
    {
        public bool requireNavigation;
        public string scene;
        public string token;
        public string SceneRecreationURL;
    }
    [Serializable]
    public struct MessageInfo
    {
        public string message;
        public long idSender;
        public long[] idReceiversList;
        public long messageType;
        public long chatType;
        public double? latitude;
        public double? longitude;
        public int? activationArea;
        public string uriImage;
        public ARObject[] objects;
    }

    public string TokenReceived { get; private set; }
    public string JsonMessageDetails { get; private set; }
    [SerializeField] public MessageInfo MessageDetails;
    public int RecreationMsgId { get; private set; }
    public bool requireNavigation { get; private set; }
    public Texture2D PreviewImage { get; set; }
    public ARObject[] ARObjects { set { MessageDetails.objects = value; } }
    public event EventHandler<EventArgs> OnLoadAssets;



#if UNITY_EDITOR

    [SerializeField]
    private TextAsset jsonFile;
    [SerializeField]
    private TextAsset jsonFileMessage;
    [SerializeField]
    private string jsonMessagePath;
    [SerializeField]
    private int recMsgId = 0;
    private void Start()
    {
        RecreationMsgId = recMsgId;
        string json = jsonFile.ToString();
        ExtractToken(json);
        ExtractMessageDetails(jsonFileMessage.ToString());
    }

#endif

    #region  Tokens
    public void user_token(string token)
    {
        ExtractToken(token);
    }
    private void ExtractToken(string token)
    {
        try
        {
            InfoRecieved info = JsonUtility.FromJson<InfoRecieved>(token.ToString());
            TokenReceived = "Bearer " + info.token;
            requireNavigation = info.requireNavigation;
            StartCoroutine(LoadScene(info.scene));
        }
        catch
        {
            Debug.LogError("Something went Wrong while loading data");
        }
    }
    public void Reload()
    {
        OnLoadAssets?.Invoke(this, EventArgs.Empty);
    }
    #endregion
    #region  messageDetails
    public void messageDetails(string details)
    {
        ExtractMessageDetails(details);
    }
    private void ExtractMessageDetails(string details)
    {

        MessageDetails = JsonConvert.DeserializeObject<MessageInfo>(details);
        JsonUtility.FromJson<MessageInfo>(details);
    }

    #endregion
    public void recreationDetails(string details)
    {
        JsonMessageDetails = details;
        int _recreationMsgId;
        int.TryParse(details, out _recreationMsgId);
        RecreationMsgId = _recreationMsgId;
    }
    public IEnumerator LoadScene(string scene)
    {
#if UNITY_EDITOR
        if (SceneManager.GetActiveScene().name != "Load_Loader")
        {
            OnLoadAssets?.Invoke(this, EventArgs.Empty);
            yield break;
        }
#endif

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene);
        yield return new WaitUntil(() => asyncOperation.isDone == true);
        OnLoadAssets?.Invoke(this, EventArgs.Empty);
    }
    #region Quit
    public void FinishEditor()
    {
        string _message = JsonConvert.SerializeObject(MessageDetails);

        debugOnAndroid(_message);

        StartCoroutine(sendMessage("http://api.realappar.com:80/message/sendMessage", _message, () =>
            {
                Application.Quit();
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            }));

    }
    public IEnumerator sendMessage(string url, string postInfo, Action onDone)
    {
#if UNITY_EDITOR
        System.IO.File.WriteAllText(jsonMessagePath, postInfo);
#endif
        using (UnityWebRequest request = UnityWebRequest.Post(url, postInfo))
        {
            request.SetRequestHeader("Authorization", TokenReceived);
            request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(postInfo));
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success) { Debug.LogError("something went wronmg while posting: ->" + request.error); }
            onDone();
        }

    }
    #endregion
    private void debugOnAndroid(string logMsg)
    {
        if (logMsg.Length > 1000)
        {
            string subLogMsg = logMsg.Substring(0, 1000);
            string subLogMsgRest = logMsg.Substring(1000);

            Debug.Log(subLogMsg);
            debugOnAndroid(subLogMsgRest);
        }
        else { Debug.Log(logMsg); }
    }
}