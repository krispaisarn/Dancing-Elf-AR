using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{

    [SerializeField]
    private Animator _animator;

    private GameObject _charObj;
    private bool _isDancing;
    public bool IsDancing{
        get{
            return _isDancing;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        CheckState(MicInput.IsSinging);
    }
    private void CheckState(bool _state)
    {
        if (_state && !_isDancing)
        {
            StartDance();
        }

        else if (!_state && _isDancing)
        {
            StopDance();
        }

        void StartDance()
        {
            int danceOpt = Random.Range(1, 3);
            _animator.SetInteger("danceMove", danceOpt);
            Debug.Log("Dance with move no." + danceOpt);
            _isDancing = true;
        }

        void StopDance()
        {
            _animator.SetInteger("danceMove", 0);
            _isDancing = false;

            Debug.Log("StopDance");
        }
    }

}
