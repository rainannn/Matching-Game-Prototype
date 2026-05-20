using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using DG.Tweening;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static CancellationTokenSource _cts = new();
    private void Awake()
    {
        GenerateCancelToken();
        SetFPS();
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        DOTween.SetTweensCapacity(250,50);
    }
    
    private static void SetFPS()
    {
        QualitySettings.vSyncCount = 0;
        Resolution[] resolutions = Screen.resolutions;
        int maxRefreshRate = 0;
        
        foreach (var res in resolutions)
        {
            double tmp = res.refreshRateRatio.value;
            if (tmp > maxRefreshRate)
            {
                maxRefreshRate = Convert.ToInt32(tmp);
            }

            Debug.Log(res.width + "x" + res.height + " : " + tmp);
        }

        if (maxRefreshRate < 30)
        {
            maxRefreshRate = 30;
        }
        else if (maxRefreshRate > 60)
        {
            maxRefreshRate = 60;
        }

        Application.targetFrameRate = maxRefreshRate;
        Debug.Log("Application.targetFrameRate: " + Application.targetFrameRate);
    }
    
    public static CancellationToken GetCancellationToken()
    {
        return _cts.Token;
    }

    public static void GenerateCancelToken()
    {
        if (_cts != null)
        {
            _cts.Cancel();
            _cts.Dispose();
        }

        _cts = new CancellationTokenSource();
    }

    public static void CancelToken()
    {
        // Debug.LogError("CancelToken");
        // await UniTask.WaitUntil(IsAnyLockerRunning);
        _cts.Cancel();
    }
}
