using UnityEngine;

public class MpGuageObject : MonoBehaviour {
    Animator _animator;
    void Awake() {
        _animator = GetComponent<Animator>();
    }
    public void Turn(bool value) {
        if(_animator != null) {
            _animator.ResetTrigger(value ? "TurnOff" : "TurnOn");
            _animator.SetTrigger(value ? "TurnOn" : "TurnOff");
        }
    }

    public void SetData(bool value) {
        if(_animator != null) { 
            _animator.SetTrigger(value ? "On" : "Off");
        }
    }
}
