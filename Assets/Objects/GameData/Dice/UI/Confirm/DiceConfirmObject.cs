using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceConfirmObject : MonoBehaviour {
    Animator _animator;
    [SerializeField] Image _diceImage;
    [SerializeField] CustomButton _button;

    void Awake() {
        _animator = GetComponent<Animator>();
    }

    public void Ready(Action buttonAction) {
        if(_button) {
            _button.DownAction = buttonAction;
        }
    }

    public void Open(int value, DiceEyesCollection collection) {
        if(Utils.NotNull(collection, _diceImage)) {
            _diceImage.sprite = collection.GetBlue(value);
        }
        SetActive(true);
    }

    public void Close() {
        SetActive(false);
    }

    void SetActive(bool value) {
        if(_animator) {
            if(value) {
                if(_animator.GetCurrentAnimatorStateInfo(0).IsName("Off")) {
                    _animator.SetTrigger("TurnOn");
                }
            }
            else {
                if(_animator.GetCurrentAnimatorStateInfo(0).IsName("On")) {
                    _animator.SetTrigger("TurnOff");
                }
            }
        }
    }

    public void SetInteractable(bool value) {
        if(_button) {
            _button.SetInteractable(value);
        }
    }
}
