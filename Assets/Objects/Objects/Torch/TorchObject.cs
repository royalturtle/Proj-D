using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchObject : MonoBehaviour {
    Animator _animator;
    [SerializeField] Animator _effectAnimator;

    void Awake() {
        _animator = GetComponent<Animator>();
    }

    public void Off() {
        if(_animator != null) {
            _animator.SetTrigger("Off");
        }
        if(_effectAnimator != null) {
            _effectAnimator.SetTrigger("TurnOn");
        }
    }

    public void SetTrigger(string trigger) {
        if(_animator != null) {
            _animator.SetTrigger(trigger);
        }
    }
}
