using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MicInput : MonoBehaviour
{
    [Tooltip("How loud to make it count as singing")]
    [SerializeField] public int _loudThreshold;
    [Tooltip("How long to switch state between singing and silent")]
    [SerializeField] public int _freqThreshold;

    private float _micLoudness;
    private string _device;
    private bool _isMicOn;
    private bool _isSinging;
    public bool SingingState
    {
        get
        {
            return _isSinging;
        }
        set
        {
            _isSinging = value;
            IsSinging= value;
        }
    }

    public static bool IsSinging;

    public int _singCounter;
    public int _silentCounter;
    //mic initialization
    void InitMic()
    {
        if (_device == null) _device = Microphone.devices[0];
        _clipRecord = Microphone.Start(_device, true, 999, 44100);
        _isMicOn = true;
    }

    void StopMicrophone()
    {
        _isMicOn = false;
        Microphone.End(_device);
    }

    AudioClip _clipRecord;
    int _sampleWindow = 128;

    //get data from microphone into audioclip
    float LevelMax()
    {
        float levelMax = 0;
        float[] waveData = new float[_sampleWindow];
        int micPosition = Microphone.GetPosition(null) - (_sampleWindow + 1); // null means the first microphone
        if (micPosition < 0) return 0;
        _clipRecord.GetData(waveData, micPosition);
        // Getting a peak on the last 128 samples
        for (int i = 0; i < _sampleWindow; i++)
        {
            float wavePeak = waveData[i] * waveData[i];
            if (levelMax < wavePeak)
            {
                levelMax = wavePeak;
            }
        }
        return levelMax;
    }



    void Update()
    {
        if (!_isMicOn)//only update if mic is on
            return;
        // levelMax equals to the highest normalized value power 2, a small number because < 1
        // pass the value to a static var so we can access it from anywhere
        _micLoudness = LevelMax() * 100;//rescale by multiply 100
        CheckLoudness(_micLoudness);
    }

    ///<summary>
    /// Check if volume is louder than threshold by period of _freqThreshold
    /// then switch state between sing and silent.
    ///</summary>
    private void CheckLoudness(float _loudness)
    {
        if (_loudness > _loudThreshold) //if current recording volume loader that threshold
        {
            _singCounter++;
        }
        else
        {
            _silentCounter++;
        }

        if (_singCounter > _freqThreshold) //if mic can record desire volume longer enough then change its singing state
        {
            Debug.Log("Is Sing");
            ResetCounter();
            SingingState = true;
        }
        else if (_silentCounter > _freqThreshold)// cut exit threshold by half to sync dance move when stop singing
        {
            Debug.Log("Is Silent");
            ResetCounter();
            SingingState = false;
        }
    }

    void ResetCounter()
    {
        _singCounter = 0;
        _silentCounter = 0;
    }



    bool _isInitialized;
    // start mic when scene starts
    void OnEnable()
    {
        InitMic();
        _isInitialized = true;
    }

    //stop mic when loading a new level or quit application
    void OnDisable()
    {
        StopMicrophone();
    }

    void OnDestroy()
    {
        StopMicrophone();
    }


    // make sure the mic gets started & stopped when application gets focused
    void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Debug.Log("Focus");

            if (!_isInitialized)
            {
                //Debug.Log("Init Mic");
                InitMic();
                _isInitialized = true;
            }
        }
        if (!focus)
        {
            //Debug.Log("Pause");
            StopMicrophone();
            Debug.Log("Stop Mic");
            _isInitialized = false;

        }
    }
}
