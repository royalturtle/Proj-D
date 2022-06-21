using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectObject : MonoBehaviour {
    public int Index {get; private set;}
    [SerializeField] protected Button Btn;
    [SerializeField] protected Animator Anim;

    void Start() {
        CheckAnimator();
    }

    public void Ready(int index, Action<int> selectAction) {
        Index = index;
        if(Btn != null && selectAction != null) {
            Btn.onClick.RemoveAllListeners();
            Btn.onClick.AddListener(() => {
                selectAction(Index);
            });
        }
    }

    void CheckAnimator() {
        if(Anim != null) {
            Anim = GetComponent<Animator>();
        }
    }

    public void Diselected() {
        CheckAnimator();
        if(Anim != null) {
            Anim.SetTrigger("Normal");
        }
    }

    public void Selected() {
        if(Anim != null) {
            Anim.SetTrigger("Selected");
        }
    }

    public void Pressed() {
        if(Anim != null) {
            Anim.SetTrigger("Pressed");
        }
        
    }

    public void SetInteractable(bool value) {
        if(Btn != null) {
            Btn.interactable = value;
        }
    }
}
