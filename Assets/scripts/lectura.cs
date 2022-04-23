using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lectura : MonoBehaviour
{
    public Button BotonPausa;
    public GameObject[] ObjAGuardar;
    public GameObject TextAGuardar;
    public GameObject g;
    public GameObject h;
    public Text mensaje;
    public InputField mensajeria;
    public Material mtl;
    GameObject textos;
    string textura = "empty";
    string fuente;
    public GameObject letra;
    public GameObject texto_Comentario;
    public GameObject texto_Archivo;
   public GameObject texto3d;

    private MText.Modular3DText mtex;


    

    void Update()
    {
        ObjAGuardar = GameObject.FindGameObjectsWithTag("con_colliders");
        
    }
    public void FindThem()
    {
        
        textos= transform.GetChild(0).gameObject;
        

        int N= ObjAGuardar.Length;

        for (int i = 0; i < N; i++)
        {
            g = Instantiate(textos, transform);
            
            g.GetComponent<Text>().text = ObjAGuardar[i].name  + "," + ObjAGuardar[i].GetComponent<Transform>().position.ToString() + "," + ObjAGuardar[i].GetComponent<Transform>().rotation.ToString() + "," + ObjAGuardar[i].GetComponent<Transform>().localScale.ToString() + "," + texto_Comentario.GetComponent<Text>().text ;//"Game "+i 
            
           
        }
        //Destroy(textos);
        textos.SetActive(false);
    }
    public void FindIt()
    {

        mtex = FindObjectOfType<MText.Modular3DText>();

        bool letrasactivas = letra.activeSelf;

        if (letrasactivas==true)
        {
            fuente = mtex.Font.ToString();
        }


        if (mtl.mainTexture!=null) {

             textura = mtl.mainTexture.name;
        }

        mensaje.GetComponent<Text>().text = mensajeria.text + "," + textura + ","+  mtl.color.r + "," + mtl.color.g + "," + mtl.color.b + "," + mtl.color.a + ","+mtl.GetFloat("_Glossiness").ToString() + "," + mtl.GetFloat("_Metallic").ToString() + TextAGuardar.GetComponent<Transform>().position.ToString() +","+ TextAGuardar.GetComponent<Transform>().localScale.ToString() + "," + fuente; 
    }


    public void  leer_comentario()
    {
        string Comentario = texto_Comentario.GetComponent<Text>().text;
        string Archivo = texto_Archivo.GetComponent<Text>().text;

        print(Archivo);
        print(Comentario);

        Archivo = Comentario;



    }
    //{ get component text, asignar a la variable, reemplazar con el texto de acá

    public void reactivar()
    {
        if (textos!=null)
        {
            textos.SetActive(true);
        }

        if (g != null)
        {
            Destroy(g);
        }
        
        mensaje.GetComponent<Text>().text = ("");
    }


}
