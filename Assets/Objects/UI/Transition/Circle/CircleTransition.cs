using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleTransition : MonoBehaviour {
    Animator _animator;
    void Awake() {
        _animator = GetComponent<Animator>();
    }

    public void TurnOn(Action afterAction = null) {
        if(_animator != null) {
            _animator.SetTrigger("TurnOn");
        }
        StartCoroutine(TurnOnAfter(afterAction));
    }

    IEnumerator TurnOnAfter(Action afterAction) {
        yield return StartCoroutine(WaitForRealSeconds(1.0f));
        if(afterAction != null) {
            afterAction();
        }
    }

    IEnumerator WaitForRealSeconds (float seconds) {
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup-startTime < seconds) {
            yield return null;
        }
    }
}
