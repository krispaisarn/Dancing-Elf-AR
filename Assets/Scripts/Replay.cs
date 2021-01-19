using System;
using UnityEngine;
#if PLATFORM_IOS
using UnityEngine.iOS;
using UnityEngine.Apple.ReplayKit;

public class Replay : MonoBehaviour
{
    public bool enableMicrophone = false;

    private bool _isShowPreview;
    public void RecordScreen()
    {
        if (!ReplayKit.APIAvailable)
        {
            return;
        }
        var recording = ReplayKit.isRecording;

            try
            {
                recording = !recording;
                if (recording)
                {
                    ReplayKit.StartRecording(enableMicrophone, false);
                    _isShowPreview=false;
                }
                else
                {
                    ReplayKit.StopRecording();
                }
            }
            catch (Exception e)
            {
            }

    }

    void Update()
    {
        // If the camera is enabled, show the recorded video overlaying the game.
        if (ReplayKit.isRecording )
    {
            ReplayKit.HideCameraPreview();
    }

        if (ReplayKit.recordingAvailable && !_isShowPreview)
        {               
            _isShowPreview=true;
                            ReplayKit.Preview();


        }

    }
}
#endif