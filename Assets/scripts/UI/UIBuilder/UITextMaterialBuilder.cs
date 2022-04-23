using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace realapp.UIBuilder
{
    public class UITextMaterialBuilder : UIBuilderBase
    {
        [System.Serializable]
        private struct ColorTextureInformation
        {
            public string idCatalogValue;
            public Sprite icon;
            public string iconUrl;
            public string catalogValue;
            public bool hasTexutre;
        }

        #region fields 
        [SerializeField] private ColorTextureInformation[] colorInformation;
        [SerializeField] private ColorTextureInformation[] textureInformation;
        [SerializeField] private Sprite defaultIcon;
        private GameObject[] colorTextureButtons;
        #endregion

        #region Build UI 
        protected override void GatherUIBUildInformation()
        {
            string url = "http://api.realappar.com:80/message/getCatalogValueByCatalogTypeId/";
            GetUIBuildInformation<ColorTextureInformation>(url + 5, "modelCategories",
                (ColorTextureInformation[] returnedInformation) => { colorInformation = returnedInformation; });

            GetUIBuildInformation<ColorTextureInformation>(url + 6, "modelCategories",
                (ColorTextureInformation[] returnedInformation) =>
                {
                    textureInformation = returnedInformation;
                    GatherUITextures();
                }
            );
        }
        protected override void GatherUITextures()
        {
            GetUIIcons<ColorTextureInformation>(textureInformation,
            (ColorTextureInformation information) => { return information.iconUrl; },
            (Texture2D texture, int index) =>
                {
                    textureInformation[index].icon = GenerateSprite(texture);
                    textureInformation[index].hasTexutre = true;
                },
            () => { DrawUI(); }
            );
        }
        protected override async void DrawUI()
        {
            ColorTextureInformation[] bothInformation = colorInformation.Concat(textureInformation).ToArray();
            colorTextureButtons = new GameObject[bothInformation.Length];

            for (int i = 0; i < bothInformation.Length; i++)
            {
                colorTextureButtons[i] = Instantiate(buttonTemplate, transform);
                if (bothInformation[i].icon != null) { colorTextureButtons[i].transform.GetChild(0).GetComponent<Image>().sprite = bothInformation[i].icon; }
                else { colorTextureButtons[i].transform.GetChild(0).GetComponent<Image>().sprite = defaultIcon; }
                colorTextureButtons[i].transform.GetChild(1).GetComponent<Text>().text = bothInformation[i].catalogValue;
                colorTextureButtons[i].transform.GetChild(2).GetComponent<Text>().text = bothInformation[i].idCatalogValue;
                colorTextureButtons[i].transform.GetChild(3).GetComponent<Text>().text = bothInformation[i].iconUrl;
                colorTextureButtons[i].transform.GetChild(4).GetComponent<Text>().text = bothInformation[i].hasTexutre.ToString();

                colorTextureButtons[i].name = i.ToString();
                await Task.Yield();
            }
            Destroy(buttonTemplate);
        }
        #endregion
    }
}