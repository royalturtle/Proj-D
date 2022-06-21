using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultTextObject : MonoBehaviour {
    TextMeshProUGUI _text;
    Animator _animator;

    string _tmpText;
    Color32 _tmpColor;

    void Awake() {
        _text = GetComponent<TextMeshProUGUI>();
        _animator = GetComponent<Animator>();
    }

    public void Show() {
        if(_animator != null) {
            _animator.SetTrigger("TurnOn");
        }
    }

    public void Show(string text) {
        if(_text != null) {
            _text.text = text;
        }
        if(_animator != null) {
            _animator.SetTrigger("TurnOn");
        }
    }
    
    public void Show(string text, Color32 color) {
        if(_text != null) {
            _text.text = text;
            _text.color = color;
        }
        if(_animator != null) {
            _animator.SetTrigger("TurnOn");
        }
    }

    public void Change(string text, Color32 color) {
    // void Change(string text) {
        _tmpText = text;
        _tmpColor = color;
        if(_animator != null) {
            _animator.SetTrigger("RotateTurnOn");
        }
    }

    public void ChangeText() {
        if(_text != null) {
            _text.text = _tmpText;
            _text.color = _tmpColor;
        }
        if(_animator != null) {
            _animator.ResetTrigger("RotateTurnOn");
            _animator.SetTrigger("RotateTurnOff");
        }
    }

    public void Hide() {
        if(_animator != null) {
            _animator.SetTrigger("TurnOff");
        }
    }
}
