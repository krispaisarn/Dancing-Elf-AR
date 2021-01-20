using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AndroidScreenRecorder : MonoBehaviour
{
    private AndroidUtils androidUtils;
    [SerializeField] private bool _isRecording;
    private void Start()
    {
        androidUtils = FindObjectOfType<AndroidUtils>();

    }
    public void RecordScreen()
    {
        _isRecording = !_isRecording;
        if (_isRecording)
            androidUtils.StartRecording();
        else
            androidUtils.StopRecording();
    }

}
