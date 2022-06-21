using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionObject : MonoBehaviour {
    Action OnAction;
    bool IsTurnOnAuto;
    [SerializeField] Animator animator;

    AudioSource _sfxAudio;

    void Awake() {
        _sfxAudio = GetComponent<AudioSource>();
    }

    void Update() {
        if(animator != null && animator.GetCurrentAnimatorStateInfo(0).IsName("On")){
            if(OnAction != null) {
                OnAction();
                if(IsTurnOnAuto) {
                    animator.SetTrigger("TurnOff");
                }
            }
        }
    }

    public void TurnOn(Action onAction = null, bool isTurnOnAuto=true) {
        IsTurnOnAuto = isTurnOnAuto;
        OnAction = onAction;
        if(Utils.NotNull(animator)) {
            animator.SetTrigger("TurnOn");
        }
        if(_sfxAudio != null) {
            _sfxAudio.Play();
        }
    }

    public void TurnOff() {
        if(Utils.NotNull(animator)) {
            animator.SetTrigger("TurnOff");
        }        
        if(_sfxAudio != null) {
            _sfxAudio.Play();
        }
    }

    public void SetOn() {
        if(Utils.NotNull(animator)) {
            animator.SetTrigger("On");
        }
    }

    public void SetOff() {
        if(Utils.NotNull(animator)) {
            animator.SetTrigger("Off");
        }        

    }
}
