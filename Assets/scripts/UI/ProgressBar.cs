using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image bar;
    [SerializeField, Min(1)] private int maxProgress = 1;
    public int MaxProgress { get { return maxProgress; } set { maxProgress = value; } }
    [SerializeField, Min(1)] private int startProgress = 0;
    [SerializeField] private float speed = 1;
    private float targetAmount;
    private float _progress;
    public float Progress
    {
        get { return _progress; }
        set { if (value <= maxProgress && value >= 0) { _progress = value; } }
    }
    private void Awake()
    {
        bar.fillAmount = startProgress / maxProgress;
    }
    public void SetProgressValue(float value)
    {
        Progress = value;
        targetAmount = Progress / maxProgress;
    }

    private void Update()
    {
        if (bar.fillAmount != targetAmount)
        {
            bar.fillAmount = Mathf.Lerp(bar.fillAmount, targetAmount, Time.deltaTime * speed);
        }
    }
}