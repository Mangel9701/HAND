using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;


public class cambio_material : MonoBehaviour
{
    public Material mtl;
    public Slider Material;
    public Slider Brillo;

    public  Button [] Boton ;

    public void cambio_brillo()
    {
        mtl.SetFloat("_Smoothness",Brillo.value);
    }

    public void cambio_metalic()
    {
        mtl.SetFloat("_Metallic",Material.value);
    }


}
