using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    /// <summary>
    /// Method use for go to ar view scene 
    /// </summary>
    public static void GoToNextView()
    {
        LoaderUtility.Initialize();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /// <summary>
    /// 
    /// </summary>
    public void BackFromCurrentScene()
    {
        PlaceOnPlane.isObjectPlaced = false;
        MultipleObjectPlacement.isObjectPlaced = false;
        Destroy(PlaceOnPlane.spawnedObject);
        PrefabMaterialHandler.SpawningObjectMaterials = null;
        LoaderUtility.Deinitialize();
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Method use for go to ar measurement scene 
    /// </summary>
    public static void ARMeasurement()
    {
        LoaderUtility.Initialize();
        SceneManager.LoadScene("ARMeasurement");
    }

    /// <summary>
    /// Method use for go to AR halloween MultiPlacement
    /// </summary>
    public static void ARHalloweenMulti()
    {
        LoaderUtility.Initialize();
        SceneManager.LoadScene("HalloweenMultipleObjectsPlacement");
    }

    

    /// <summary>
    /// Method use for go to ar measurement scene 
    /// </summary>
    public static void ARTile()
    {
        LoaderUtility.Initialize();
        SceneManager.LoadScene("ARTilling");
    }

    /// <summary>
    /// Method use for go to ar measurement scene 
    /// </summary>
    public static void ARMultipleObjects()
    {
        LoaderUtility.Initialize();
        SceneManager.LoadScene("ARMultipleObjects");
    }
}
