using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    public bool IsDebugAnimation;
    [Header("Character Animation")]
    public GameObject CharPrefab;
    private Animator _animator;

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
        if (IsDebugAnimation)
            DebugAnimation();
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
}