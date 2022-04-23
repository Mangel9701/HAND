using System.Collections;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace realapp.UIBuilder
{
    public class UIModelBuilder : UIBuilderBase
    {
        [System.Serializable]
        public struct ModelInformation
        {
            public string idArModel;
            public string description;
            public string urlModel;
            public Sprite Icon;
            public string urlPreview;
            public string name;
        }
        private struct PagesInJson
        {
            public int totalPages;
        }

        #region  fields
        //querry information
        private int totalPages;
        private int pageNum = 0;
        private int category = 0;
        private string query;

        //generalInformation
        [SerializeField] private GameObject[] ModelButtons;
        [SerializeField] private ModelInformation[] allModelInfromation;

        //reference 
        [SerializeField] private GameObject othrePanel;

        public UnityEngine.Events.UnityEvent onTimeToResizeScrollbar;

        //flags
        public bool HasFullyLoaded { get; private set; } = false;
        #endregion

        #region Change Models
        public void ChangeModels(int _category) => ChangeModels(_category, "");
        public void ChangeModels(string _query) => ChangeModels(0, _query);
        public void ChangeModels(int _category, string _query)
        {

            if (!HasFullyLoaded)
            {
                StopAllCoroutines();
            }
            HasFullyLoaded = false;
            pageNum = 0;

            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }

            category = _category;
            query = _query;

            GatherUIBUildInformation();
        }
        public void LoadExtraModels(Vector2 scrollPositon)
        {
            if (scrollPositon.y < 0f && HasFullyLoaded && pageNum < totalPages)
            {
                HasFullyLoaded = false;
                pageNum++;
                GatherUIBUildInformation();
            }
        }

        #endregion

        #region Build UI 
        protected override void GatherUIBUildInformation()
        {
            string url = $"http://api.realappar.com:80/message/getPagedModels/{category}/{pageNum}/5/?query={query}";
            GetUIBuildInformation<ModelInformation>(url, "models", (ModelInformation[] returnedInformation) =>
            {
                allModelInfromation = returnedInformation;
                ModelButtons = new GameObject[allModelInfromation.Length];

                //generate buttons
                for (int i = 0; i < allModelInfromation.Length; i++)
                {
                    ModelButtons[i] = (GameObject)Instantiate(buttonTemplate, transform);
                    Variables.Object(ModelButtons[i]).Set("MyPanelArray", gameObject);
                    Variables.Object(ModelButtons[i]).Set("OtherPanelAray", othrePanel);

                    ModelButtons[i].name = i.ToString();
                }
                //get images 
                GatherUITextures();
                onTimeToResizeScrollbar.Invoke();
            });
        }
        protected override T[] GetInformationObject<T>(string jsonString, string category)
        {
            JSONObject json = new JSONObject(jsonString);
            totalPages = JsonUtility.FromJson<PagesInJson>(json.ToString()).totalPages;
            return base.GetInformationObject<T>(jsonString, category);
        }
        protected override void GatherUITextures()
        {
            GetUIIcons<ModelInformation>(allModelInfromation,
                (ModelInformation information) => { return information.urlPreview; },
                (Texture2D texture, int index) =>
                    {
                        allModelInfromation[index].Icon = GenerateSprite(texture);
                    },
                () => { DrawUI(); }
            );
        }
        protected override async void DrawUI()
        {
            for (int i = 0; i < allModelInfromation.Length; i++)
            {
                ModelButtons[i].transform.GetChild(1).GetComponent<Image>().sprite = allModelInfromation[i].Icon;
                ModelButtons[i].transform.GetChild(1).GetComponent<Image>().color = Color.white;
                ModelButtons[i].transform.GetChild(1).GetComponent<Animator>().enabled = false;

                ModelButtons[i].transform.GetChild(1).GetComponent<Image>().sprite = allModelInfromation[i].Icon;
                ModelButtons[i].transform.GetChild(0).GetComponent<Text>().text = allModelInfromation[i].idArModel;
                ModelButtons[i].transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text = allModelInfromation[i].name;
                ModelButtons[i].transform.GetChild(3).GetComponent<TMPro.TextMeshProUGUI>().text = allModelInfromation[i].description;
                ModelButtons[i].transform.GetChild(4).GetComponent<Text>().text = allModelInfromation[i].urlModel;
                await Task.Yield();
            }
            HasFullyLoaded = true;
        }
        #endregion
    }
}