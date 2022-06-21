using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossObject : MonoBehaviour {
    Animator animator;
    [SerializeField] Animator PortalObj, EffectObj;
    SpriteRenderer _spriteRenderer;

    void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void CheckAnimator() {
        if(animator == null) {
            animator = GetComponent<Animator>();
        }
    }
    
    public void PortalTurn(bool value) {
        if(PortalObj != null) {
            PortalObj.SetTrigger(value ? "TurnOn" : "TurnOff");
        }
    }

    public void Ready(RuntimeAnimatorController controller) {
        CheckAnimator();
        if(animator != null) {
            animator.runtimeAnimatorController = controller;
        }
    }

    public void Damage() {
        if(EffectObj != null) {
            float random = UnityEngine.Random.Range(0.8f, 1.0f);
            EffectObj.transform.localScale = new Vector3(random, random, random);
            // EffectObj.transform.rotation = Quaternion.Euler(new Vector3(0,0,UnityEngine.Random.Range(0, 360)));
            EffectObj.transform.rotation = Quaternion.Euler(0,0,UnityEngine.Random.Range(0, 360));
            EffectObj.SetTrigger("Attack");
        }        
    }

    public void Death() {
        if(EffectObj != null) {
            EffectObj.SetTrigger("Death");
            if(animator != null) {
                animator.runtimeAnimatorController = null;
            }
            if(_spriteRenderer != null) {
                _spriteRenderer.sprite = null;
            }
        }
    }
}
