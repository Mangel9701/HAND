
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Getting Initial Data for handlers
/// </summary>
public class InitialData : MonoBehaviour
{
    /// <summary>
    /// Game Object To place 
    /// </summary>
    public static GameObject SpawningObject;

    public static Material[] getObjectMaterials;
    public static Material[] setObjectMaterials;

    /// <summary>
    /// Value return true if scene is a single object placement
    /// </summary>
    public static bool _singleObjectPlacement;

    void Start()
    {
        getObjectMaterials = null;
    }

    public void ShowPrefabInARView(GameObject spwaningObject)
    {
        SpawningObject = spwaningObject;
        SceneHandler.GoToNextView();
        getObjectMaterials = EventSystem.current.currentSelectedGameObject.GetComponent<PrefabMaterialHandler>().ObjectMaterials;
        setObjectMaterials = getObjectMaterials;
        for (int i = 0; i < getObjectMaterials.Length; i++)
        {
            getObjectMaterials[i].shader = (Shader)Resources.Load("TransparentShader", typeof(Shader));
            getObjectMaterials[i].color = new Color32(255, 255, 255, 100);
        }
    }
}
