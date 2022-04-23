using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace realapp.UIBuilder
{
    public class UICategoryBuilder : UIBuilderBase
    {
        [System.Serializable]
        protected struct CategoryInformation
        {
            public string idCatalogValue;
            public string catalogValue;
            public string iconUrl;
            public Sprite icon;
        }
        [SerializeField] protected GameObject[] categoryButtons;
        [SerializeField] protected CategoryInformation[] allCategories;

        protected override void GatherUIBUildInformation()
        {
            string url = "http://api.realappar.com:80/message/getCatalogValueByCatalogTypeId/4";
            GetUIBuildInformation<CategoryInformation>(url, "modelCategories",
            (CategoryInformation[] returnedInformation) =>
                {
                    allCategories = returnedInformation;
                    categoryButtons = new GameObject[allCategories.Length];

                    for (int i = 0; i < allCategories.Length; i++)
                    {
                        categoryButtons[i] = (GameObject)Instantiate(buttonTemplate, transform);
                    }
                    Destroy(buttonTemplate);

                    GatherUITextures();
                }
            );
        }

        protected override void GatherUITextures()
        {
            GetUIIcons<CategoryInformation>(allCategories,
                (CategoryInformation information) => { return information.iconUrl; },
                (Texture2D texture, int index) =>
                    {
                        allCategories[index].icon = GenerateSprite(texture);
                    },
                () => { DrawUI(); }
            );
        }
        protected override async void DrawUI()
        {
            for (int i = 0; i < allCategories.Length; i++)
            {
                categoryButtons[i].transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = allCategories[i].icon;
                categoryButtons[i].transform.GetChild(2).GetChild(0).GetComponent<Image>().color = Color.white;
                categoryButtons[i].transform.GetChild(2).GetChild(0).GetComponent<Animator>().enabled = false;

                categoryButtons[i].transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = allCategories[i].catalogValue;
                categoryButtons[i].transform.GetChild(2).GetChild(0).GetComponent<Image>().sprite = allCategories[i].icon;
                categoryButtons[i].transform.GetChild(3).GetComponent<Text>().text = allCategories[i].idCatalogValue;
                categoryButtons[i].name = i.ToString();
                await Task.Yield();
            }
        }
    }
}