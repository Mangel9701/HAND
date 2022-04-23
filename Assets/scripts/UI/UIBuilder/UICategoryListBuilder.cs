using System.Threading.Tasks;
using UnityEngine.UI;

namespace realapp.UIBuilder
{
    public class UICategoryListBuilder : UICategoryBuilder
    {
        protected override async void DrawUI()
        {
            for (int i = 0; i < base.allCategories.Length; i++)
            {
                base.categoryButtons[i].transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = allCategories[i].catalogValue;
                base.categoryButtons[i].transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = allCategories[i].icon;
                base.categoryButtons[i].transform.GetChild(3).GetComponent<Text>().text = allCategories[i].idCatalogValue;

                base.categoryButtons[i].name = i.ToString();
                await Task.Yield();
            }
        }
    }
}