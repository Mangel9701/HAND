using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class din_texture : MonoBehaviour
{
    public float speedX;
    public float speedY;

    public Material mtl;

    //private float CurrX;
    //private float CurrY;
    float CurrX;
    float CurrY;

    

    // Start is called before the first frame update
    void Start()
    {
        // Curr = mtl.GetTexture("_MainTex");
         CurrX = GetComponent<Renderer>().material.mainTextureOffset.x;
         CurrY = GetComponent<Renderer>().material.mainTextureOffset.y;
        /*CurrX = GetComponent<Renderer>().material.GetTextureOffset("_EmissionMap").x;
        CurrY = GetComponent<Renderer>().material.GetTextureOffset("_EmissionMap").y;*/
         
    }

    // Update is called once per frame
    void Update()
    {
        /*float a = Curr.width;
        float b = Curr.height;
        a += Time.deltaTime * speedX;
        b += Time.deltaTime * speedY;*/
        //GetComponent<Renderer>().material.SetTextureOffset("_BumpMap", new Vector2(a,b));

        CurrX += Time.deltaTime * speedX;
        CurrY += Time.deltaTime * speedY;

        //mtl.SetTextureOffset("_MainTex", new Vector2(a,b));

        GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(CurrX, CurrY));




    }
}
