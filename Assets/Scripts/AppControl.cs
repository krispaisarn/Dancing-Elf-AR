using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppControl : MonoBehaviour
{

    [SerializeField]
    private Transform _effects;
    // Start is called before the first frame update
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

}
