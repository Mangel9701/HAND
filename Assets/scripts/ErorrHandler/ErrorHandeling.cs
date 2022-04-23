using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace realapp.Core
{
    public class ErrorHandeling : Singleton<ErrorHandeling>
    {
        [SerializeField] private GameObject conexionError;
        [SerializeField] private TextMeshProUGUI errorText;
        [SerializeField] private GameObject unexpectedError;
        private bool isHandelingError = true;

        public void HandleError(string error)
        {
            if (isHandelingError)
            {
                if (error.Contains("Unauthorized")) { unexpectedError.SetActive(true); }
                else
                {
                    conexionError.SetActive(true);
                    errorText.text = error;
                }
                isHandelingError = true;
            }
        }

        public void Retray()
        {
            conexionError.SetActive(false);
            Tokens.Instance.Reload();
            isHandelingError = true;
        }

        public void ExitEditor()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

    }
}
