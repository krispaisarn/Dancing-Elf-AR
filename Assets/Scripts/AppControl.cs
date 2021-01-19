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

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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

}
