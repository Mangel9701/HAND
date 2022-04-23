using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;

namespace realapp.Tools.JsonEditor
{
    public class JsonEditorWindow : EditorWindow
    {
        private enum TypeOfContainer
        {
            tokenRecieved,
            messageDetails,
            arScene,
        }
        public const string path = "Assets/Editor/JsonEditorWindow/";
        private VisualElement container;
        private VisualElement jsonContainer;

        private JsonEditorObject jsonEditorSO;
        private TypeOfContainer typeOfContainer = TypeOfContainer.tokenRecieved;


        [MenuItem("Window/RealAppTools/Json Builder")]
        public static void ShowWindow()
        {
            JsonEditorWindow window = GetWindow<JsonEditorWindow>();
            window.titleContent = new GUIContent("Json Builder");
            window.minSize = new Vector2(500, 250);
        }
        public void CreateGUI()
        {
            container = rootVisualElement;
            VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>($"{path}JsonEditorWindow.uxml");
            container.Add(visualTree.Instantiate());

            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>($"{path}JsonEditorWindow.uss");
            container.styleSheets.Add(styleSheet);

            jsonContainer = container.Q<VisualElement>("JsonContainer");
            jsonEditorSO = AssetDatabase.LoadAssetAtPath<JsonEditorObject>($"{path}EditorJSONS/JsonEditorSO.asset");
            if (jsonEditorSO is null)
            {
                jsonEditorSO = ScriptableObject.CreateInstance<JsonEditorObject>();
                AssetDatabase.CreateAsset(jsonEditorSO, $"{path}EditorJSONS/JsonEditorSO.asset");
                AssetDatabase.SaveAssets();
            }

            //Tokens 
            Button tokenRecievedBtn = container.Q<Button>("TokenRecieved");
            tokenRecievedBtn.clicked += CreateTokenReciveCointainer;

            //Message Details 
            Button messageDetailsBtn = container.Q<Button>("MessageDetails");
            messageDetailsBtn.clicked += CreateMessageDetailsBtnCointainer;

            //ARScene 
            Button arSceneBtn = container.Q<Button>("ARScene");
            arSceneBtn.clicked += ARSceneContainer;

            //Build JSON
            Button buildJson = container.Q<Button>("BuildJson");
            buildJson.clicked += BuildJson;

            CreateTokenReciveCointainer();
        }
        private void CreateTokenReciveCointainer()
        {
            typeOfContainer = TypeOfContainer.tokenRecieved;
            BuildUIContainer();
        }
        private void CreateMessageDetailsBtnCointainer()
        {
            typeOfContainer = TypeOfContainer.messageDetails;
            BuildUIContainer();
        }
        private void ARSceneContainer()
        {
            typeOfContainer = TypeOfContainer.arScene;
            BuildUIContainer();
        }

        private void BuildUIContainer()
        {
            jsonContainer.Clear();

            SerializedObject jsonEditorSObj = new SerializedObject(jsonEditorSO);
            SerializedProperty jsonEditorPrp = jsonEditorSObj.GetIterator();
            jsonEditorPrp.Next(true);

            while (jsonEditorPrp.NextVisible(false))
            {
                PropertyField prop = new PropertyField(jsonEditorPrp, jsonEditorPrp.name);
                prop.SetEnabled(jsonEditorPrp.name != "m_script");
                prop.Bind(jsonEditorSObj);

                if (jsonEditorPrp.name.Contains(typeOfContainer.ToString()))
                {
                    jsonContainer.Add(prop);
                }

            }
        }
        private void BuildJson()
        {
            switch (typeOfContainer)
            {
                case TypeOfContainer.tokenRecieved:
                    jsonEditorSO.saveTokenRecieved();
                    break;
                case TypeOfContainer.messageDetails:
                    jsonEditorSO.saveMessageDetails();
                    break;
                case TypeOfContainer.arScene:
                    jsonEditorSO.saveArScene();
                    break;
            }
        }

    }
}
