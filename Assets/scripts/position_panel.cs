using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class position_panel : MonoBehaviour
{
    //public GameObject panel;
    public RectTransform rect;

    public RectTransform rect2;
    public RectTransform rect3;

    public float posicionY;

    public float posicionX;

    public float posicionY_1;

    void Update()
    {
        posicionY = rect.anchoredPosition.y;
        posicionY_1 = rect.anchoredPosition.y;
        posicionX = rect2.anchoredPosition.x;
    }
}
