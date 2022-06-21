using UnityEngine;
using TMPro;

public class GimicCountObject : MonoBehaviour {
    Animator _animator;

    [SerializeField] TextMeshProUGUI Txt;
    int _temp;

    void Awake() {
        _animator = GetComponent<Animator>();
    }
    
    public void SetActive(bool value) {
        gameObject.SetActive(value);
    }

    public void SetData(int value, bool isAnimation) {
        if(isAnimation) {
            if(Txt) {
                Txt.text = value.ToString();
            }
        }
        else {
            _temp = value;
            if(_animator) {
                _animator.SetTrigger("TurnOn");
            }
        }
        
    }

    public void SetText() {
        if(Txt) {
            Txt.text = _temp.ToString();
        }
        if(_animator) {
            _animator.SetTrigger("TurnOff");
        }
    }
}
