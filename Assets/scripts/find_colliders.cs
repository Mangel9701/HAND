using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class find_colliders : MonoBehaviour
{
    public GameObject [] Con_colliders;
    public GameObject[] Con_colliders_text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    //void Update()
    //{
    //    Encontrados();
    //}

    public void Encontrados()
    {
       Con_colliders = GameObject.FindGameObjectsWithTag("con_colliders");
       Con_colliders_text = GameObject.FindGameObjectsWithTag("mensaje");

    }

    public void Activados()
    {
        foreach (GameObject r in Con_colliders)
        {
            r.GetComponent<BoxCollider>().enabled = true;
        }
        foreach (GameObject r in Con_colliders_text)
        {
            r.GetComponent<BoxCollider>().enabled = true;
        }
    }

    public void Desactivados()
    {
        foreach (GameObject r in Con_colliders)
        {
            r.GetComponent<BoxCollider>().enabled = false;
        }
        foreach (GameObject r in Con_colliders_text)
        {
            r.GetComponent<BoxCollider>().enabled = false;
        }
    }


}
