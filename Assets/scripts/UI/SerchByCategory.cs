using UnityEngine;
using UnityEngine.UI;
using realapp.UIBuilder;


public class SerchByCategory : MonoBehaviour
{
    [SerializeField] private UIModelBuilder[] arrayOfButtons;
    [SerializeField] private int id;

    private void Start()
    {
        arrayOfButtons = FindObjectsOfType<UIModelBuilder>();
    }

    public void SerchWithCategory()
    {
        foreach (UIModelBuilder b in arrayOfButtons)
        {
            int.TryParse(transform.GetChild(3).GetComponent<Text>().text, out id);
            b.ChangeModels(id);
        }
    }
}
