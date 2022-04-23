using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
public class SendMessageCanvas : MonoBehaviour
{
    [SerializeField] private TMP_InputField message;
    [SerializeField] private TextMeshProUGUI activationArea;
    private readonly Dictionary<int, int> activationAreaSize = new Dictionary<int, int> { { 36, 10 }, { 37, 50 }, { 38, 100 } };
    public event System.EventHandler<System.EventArgs> OnSizeChanged;

    private void Start()
    {
        message.text = Tokens.Instance.MessageDetails.message;
        int value;
        int tokenActivationArea = Tokens.Instance.MessageDetails.activationArea ?? 0;
        activationAreaSize.TryGetValue(tokenActivationArea, out value);
        activationArea.text = value + "m";
        OnSizeChanged?.Invoke(this, System.EventArgs.Empty);
    }

    public void SetMessage(string msg)
    {
        Tokens.Instance.MessageDetails.message = msg;
    }

    public void ChangeActivationArea(int actArea)
    {
        if (actArea == 10 || actArea == 50 || actArea == 100)
        {
            Tokens.Instance.MessageDetails.activationArea = activationAreaSize.FirstOrDefault(x => x.Value == actArea).Key;
            activationArea.text = actArea + "m";
            OnSizeChanged?.Invoke(this, System.EventArgs.Empty);
            return;
        }
        Debug.LogError("invalid value traying to set activation area");
    }

    public int GetSizeCode(int size)
    {
        int sizeCode = activationAreaSize.FirstOrDefault(x => x.Value == size).Key;
        return sizeCode;
    }
}


