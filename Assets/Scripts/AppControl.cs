using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppControl : MonoBehaviour
{

    [SerializeField]
    private Transform _effects;
    // Start is called before the first frame update

    [SerializeField]
    private Camera _arCamera;

#if PLATFORM_IOS
    [SerializeField] private Replay _replay;
#endif
#if PLATFORM_ANDROID
    [SerializeField] private AndroidScreenRecorder _androidRecorder;
#endif

    [SerializeField] private UIElement btn_rec;
    [SerializeField] private UIElement btn_stopRec;

    private bool _isRecording;

    [SerializeField] private CanvasGroup flashCG;
    private bool flash = false;

    private void Start(){
        #if PLATFORM_ANDROID
        btn_rec.gameObject.SetActive(false);
        #endif
    }

    private void Update()
    {
        if (flash)
        {
            flashCG.alpha = flashCG.alpha - (Time.deltaTime * (1 / .2f));
            if (flashCG.alpha <= 0)
            {
                flashCG.alpha = 0;
                flash = false;
            }
        }
    }

    public void ShowEffect(int _fx)
    {
        HideEffect();
        _effects.GetChild(_fx).gameObject.SetActive(true);
    }

    public void HideEffect()
    {
        foreach (Transform t in _effects.GetComponentInChildren<Transform>())
        {
            t.gameObject.SetActive(false);
        }
    }

    public void Capture()
    {
        Texture2D image = RTImage(_arCamera);

        NativeGallery.SaveImageToGallery(image, "Dancing Elf AR", "elf_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".jpg", (success, path) => Debug.Log("Media save result: " + success + " " + path));

        TriggerFlash();
    }


    private void TriggerFlash()
    {
        flash = true;
        flashCG.alpha = 1;
    }


    Texture2D RTImage(Camera camera)
    {
        int resWidth = Screen.width;
        int resHeight = Screen.height;
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        _arCamera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        _arCamera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        _arCamera.targetTexture = null;
        RenderTexture.active = null; // JC: a3dded to avoid errors
        Destroy(rt);

        return screenShot;
    }

    public void VideoCapture()
    {
#if PLATFORM_IOS
        _replay.RecordScreen();
#endif
#if PLATFORM_ANDROID
_androidRecorder.RecordScreen();
#endif

        _isRecording = !_isRecording;

        btn_rec.gameObject.SetActive(!_isRecording);
        btn_stopRec.gameObject.SetActive(_isRecording);
    }

}
