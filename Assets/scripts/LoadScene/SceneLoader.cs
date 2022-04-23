using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public void loadScene(string scene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        Destroy(this.gameObject);
    }
}
