using System;
using UnityEngine;
using UnityEngine.UI;

public class ActivationAreaSizeCheck : MonoBehaviour
{
    [SerializeField] private int areaSize;
    [SerializeField] private SendMessageCanvas sendMessageCanvas;
    [SerializeField] private Image check;
    Button btn;

    private void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(SetActiveAreaSize);
        sendMessageCanvas.OnSizeChanged += CheckActive;
    }

    private void CheckActive(object sender, EventArgs e)
    {
        check.gameObject.SetActive(sendMessageCanvas.GetSizeCode(areaSize) == Tokens.Instance.MessageDetails.activationArea);
    }

    private void SetActiveAreaSize()
    {
        sendMessageCanvas.ChangeActivationArea(areaSize);
    }

}
