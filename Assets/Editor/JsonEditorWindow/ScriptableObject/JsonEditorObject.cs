using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEditor;
using UnityEngine;

namespace realapp.Tools.JsonEditor
{
    public class JsonEditorObject : ScriptableObject
    {
        [Serializable]
        private struct InfoRecieved
        {
            public bool requireNavigation;
            [JsonConverter(typeof(StringEnumConverter))]
            public Scenes scene;
            public string token;
        }
        public enum Scenes
        {
            Load_Editor,
            Load_Scene_Builder,
        }
        [Serializable]
        private struct MessageDetails
        {
            public string message;
            public long idSender;
            public long[] idReceiversList;
            public long messageType;
            public long chatType;

            //nullable latitude
            [JsonProperty("latitude")]
            public double? Latitude
            {
                get
                {
                    if (latitude.isNull) return null;
                    else return latitude.value;
                }
            }
            [JsonIgnore]
            public nullableField<double> latitude;

            //nullable longitude
            [JsonProperty("longitude")]
            public double? Longitude
            {
                get
                {
                    if (longitude.isNull) return null;
                    else return longitude.value;
                }
            }
            [JsonIgnore]
            public nullableField<double> longitude;

            //nullable activationArea
            [JsonProperty("activationArea")]
            public int? ActivationArea
            {
                get
                {
                    if (activationArea.isNull) return null;
                    else return activationArea.value;
                }
            }
            [JsonIgnore]
            public nullableField<int> activationArea;

            public string uriImage;

        }
        [Serializable]
        private struct nullableField<T>
        {
            public T value;
            public bool isNull;
        }

        [Serializable]
        private struct ARScene
        {
            public ARObject[] objects;
        }

        //Tokens
        [SerializeField] private string tokenRecievedPath;
        [SerializeField] private InfoRecieved tokenRecieved;

        //Message Details 
        [SerializeField] private string messageDetailsPath;
        [SerializeField] private MessageDetails messageDetails;

        //AR Scene 
        [SerializeField] private string arScenePath;
        [SerializeField] private ARScene arScene;

        public void saveTokenRecieved()
        {
            WriteNewJsonFile(JsonConvert.SerializeObject(tokenRecieved, Formatting.Indented), tokenRecievedPath);
        }
        public void saveMessageDetails()
        {
            WriteNewJsonFile(JsonConvert.SerializeObject(messageDetails, Formatting.Indented), messageDetailsPath);
        }
        public void saveArScene()
        {
            WriteNewJsonFile(JsonUtility.ToJson(arScene, true), arScenePath);
        }
        private void WriteNewJsonFile(string json, string path)
        {
            if (!(string.IsNullOrEmpty(path)))
            {
                System.IO.File.WriteAllText(path, json);
                AssetDatabase.Refresh();
            }
        }
    }
}