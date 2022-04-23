using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

//using UnityEngine.Color;

public class Color_Cambio : MonoBehaviour
{

    public string mitexto;

    public Color32 nuevo_color;
    public byte r, g, b, a;
    public Color color_text;
    Material mtl_texto;

    void Update()
    {
        mitexto = this.transform.GetChild(1).GetComponent<Text>().text;

        separar();

    }

    void separar()
    {

        string[] datos = mitexto.Split(',');
        r = byte.Parse(datos[0]);
        g = byte.Parse(datos[1]);
        b = byte.Parse(datos[2]);
        a = byte.Parse(datos[3]);


        this.transform.GetChild(0).GetComponent<Image>().color = new Color32(r, g, b, a);

        color_text = this.transform.GetChild(0).GetComponent<Image>().color;

    }



}
