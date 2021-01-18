using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    public bool IsDebugAnimation;
    [Header("Character Animation")]
    public GameObject CharPrefab;
    private Animator _animator;

    [Header("Audio")]
    public MicInput micInput;

    private bool _isSinging;

    // Start is called before the first frame update
    void Start()
    {
        if (IsDebugAnimation)
        {
            GameObject charObj = Instantiate(CharPrefab);
            _animator = charObj.transform.GetChild(0).GetComponent<Animator>();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (!IsDebugAnimation)
            return;

        DebugAnimation();


        if (micInput.SingingState && !_isSinging)
        {
            int danceOpt = Random.Range(1, 3);
            _animator.SetInteger("danceMove", danceOpt);
            Debug.Log("Dance with move no." + danceOpt);
            _isSinging = true;
        }

        else if (!micInput.SingingState && _isSinging)
        {
            StopDance();
            _isSinging = false;
        }
    }

    private void DebugAnimation()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _animator.SetInteger("danceMove", 1);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            _animator.SetInteger("danceMove", 2);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            _animator.SetInteger("danceMove", 3);
        }
    }

    private void StopDance()
    {
        _animator.SetInteger("danceMove", 0);
        Debug.Log("StopDance");

    }

}