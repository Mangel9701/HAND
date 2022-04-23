using UnityEngine;
using UnityEngine.UI;
using realapp.UIBuilder;

public class ResetSearch : MonoBehaviour
{
    [SerializeField] private UIModelBuilder[] arrayOfButtons;

    private void Start()
    {
        arrayOfButtons = FindObjectsOfType<UIModelBuilder>();
    }

    public void ResetTheSearch()
    {
        foreach (UIModelBuilder b in arrayOfButtons)
        {
            b.ChangeModels("");
        }
    }
}
