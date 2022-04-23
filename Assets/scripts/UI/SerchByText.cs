using UnityEngine;
using UnityEngine.UI;
using realapp.UIBuilder;


public class SerchByText : MonoBehaviour
{

    private UIModelBuilder[] arrayOfButtons;
    private string query;

    private void Start()
    {
        arrayOfButtons = FindObjectsOfType<UIModelBuilder>();
    }

    public void SerchWithText()
    {
        query = GetComponent<InputField>().text;
        foreach (UIModelBuilder b in arrayOfButtons)
        {
            b.ChangeModels(query);
        }
    }
}
