using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.Networking;

public class DescargarModelo : MonoBehaviour
{
    string Nombre;
    public float progresoDescarga;
    public GameObject modelo = null;
    public string url;
    public AssetBundle bundle1;

    public void Descargar()
    {
        url = transform.GetChild(4).GetComponent<Text>().text;
        Nombre = transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>().text;
        StartCoroutine(downloadObject());
    }

    public IEnumerator downloadObject()
    {
        Dictionary<string, AssetBundle> urlBundle = Variables.Scene(this.gameObject).Get("UrlBundle") as Dictionary<string, AssetBundle>;
        if (urlBundle is null || !urlBundle.ContainsKey(url))
        {
            UnityWebRequest www1 = UnityWebRequestAssetBundle.GetAssetBundle(url);
            var operation = www1.SendWebRequest();

            while (!operation.isDone)
            {
                yield return null;
                progresoDescarga = www1.downloadProgress * 100;
            }

            if (operation.isDone)
            {


                bundle1 = DownloadHandlerAssetBundle.GetContent(www1);
                if (urlBundle is null)
                {
                    urlBundle = new Dictionary<string, AssetBundle>();
                    Variables.Scene(this.gameObject).Set("UrlBundle", urlBundle);
                }
                urlBundle.Add(url, bundle1);

            }
        }
        else
        {
            bundle1 = urlBundle[url];
        }
        //obtener nombre del asset 
        string rootAssetPath = bundle1.GetAllAssetNames()[0];
        GameObject arObject = bundle1.LoadAsset(rootAssetPath) as GameObject;
        modelo = arObject;

    }

}



