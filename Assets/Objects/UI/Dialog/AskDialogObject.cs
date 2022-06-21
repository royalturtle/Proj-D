using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AskDialogObject : MonoBehaviour {
    Animator _animator;
    [SerializeField] TextMeshProUGUI Txt;
    [SerializeField] Button OkBtn;

    void Awake() {
        _animator = GetComponent<Animator>();
    }

    public void Open(string text, Action okAction = null) {
        if(Txt != null) {
            Txt.text = text;
        }
        
        if(OkBtn != null) {
            OkBtn.onClick.RemoveAllListeners();
            OkBtn.onClick.AddListener(() => { if(okAction != null) { okAction(); } });
        }

        if(_animator != null) {
            ResetTrigger();
            _animator.SetTrigger("TurnOn");
        }
    }

    public void Close() {
        if(_animator) {
            ResetTrigger();
            _animator.SetTrigger("TurnOff");
        }
    }

    void ResetTrigger() {
        if(_animator) {
            _animator.ResetTrigger("TurnOn");
            _animator.ResetTrigger("TurnOff");
            _animator.ResetTrigger("On");
            _animator.ResetTrigger("Off");
        }
    }
}
