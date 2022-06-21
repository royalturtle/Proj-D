using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GimicCheckUnitObject : MonoBehaviour {
    Animator _animator;
    [SerializeField] Image _image;
    Sprite _tmpSprite;

    void Awake() {
        _animator = GetComponent<Animator>();
    }
    
    public void SetImage(Sprite sprite, bool isAnimation) {
        if(isAnimation) {
            _tmpSprite = sprite;
            if(_animator) {
                _animator.SetTrigger("TurnOn");
            }
        }
        else if(_image){
            _image.sprite = sprite;
        }
    }

    public void SetImageLate() {
        if(_image) {
            _image.sprite = _tmpSprite;
        }
        if(_animator) {
            _animator.SetTrigger("TurnOff");
        }
    }
}
