using UnityEngine;
public class ScreenShot : MonoBehaviour
{
    public Texture2D TakeScreenShot()
    {
        return ScreenCapture.CaptureScreenshotAsTexture();
    }

}
