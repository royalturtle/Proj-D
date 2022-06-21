using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterObject : MonoBehaviour {
    float WalkSpeed = 2.0f;
    Animator animator;
    bool IsWalkingY = false;
    float move, moveResult;
    Vector3 StartPosition;

    [SerializeField] Animator _effectAnimator;

    void Start() {
        animator = GetComponent<Animator>();
    }

    void Update() {
        if(IsWalkingY) {
            if(move < moveResult) {
                move += Time.deltaTime * WalkSpeed;
                transform.position = new Vector3(StartPosition.x, StartPosition.y + move, StartPosition.z);
            }
            else {
                transform.position = new Vector3(StartPosition.x, StartPosition.y + moveResult, StartPosition.z);
                StopWalk();
            }
        }
    }

    public void Attack() {
        SetAnimatorTrigger("Attack");
    }

    public void Damage() {
        SetAnimatorTrigger("Damage");
        if(_effectAnimator != null) {
            _effectAnimator.SetTrigger("Damage");
        }
    }

    public void Death() {
        SetAnimatorTrigger("Dead");
    }

    void SetAnimatorTrigger(string trigger) {
        if(animator != null) {
            animator.SetTrigger(trigger);
        }
    }

    void StopWalk() {
        IsWalkingY = false;
        if(animator != null) {
            animator.SetBool("IsWalk", false);
        }
    }

    public void WalkY(int value) {
        StartPosition = transform.localPosition;
        IsWalkingY = true;
        if(animator != null) {
            animator.SetBool("IsWalk", true);
        }
        moveResult = value;
        move = 0;
    }
}
