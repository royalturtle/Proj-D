using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiceEnforceObject : MonoBehaviour {
    [SerializeField] TextMeshProUGUI Txt;
    Animator _animator;

    [SerializeField] Image _iconImg;
    [SerializeField] Sprite _upSprite, _downSprite, _multiplySprite, _fixSprite;

    void Awake() {
        _animator = GetComponent<Animator>();
    }

    public void Close() {
        if(_animator != null) {
            _animator.SetTrigger("TurnOff");
        }
        Debug.Log("TURNOFF");
    }

    public void Open(DiceManager dice, SkillTypes item) {
        if(Utils.NotNull(dice)) {
            switch(item) {
                case SkillTypes.PLUS:
                    SetImage(_upSprite);
                    SetText(dice.Additional.ToString());
                    Open();
                    break;
                case SkillTypes.MINUS:
                    SetImage(_downSprite);
                    SetText(dice.Additional.ToString());
                    Open();
                    break;
                case SkillTypes.KEEP:
                    SetImage(_fixSprite);
                    SetText("고정");
                    Open();
                    break;
                case SkillTypes.CHANCE:
                    SetImage(_multiplySprite);
                    SetText("추가 기회");
                    Open();
                    break;
            }
        }
    }

    void SetImage(Sprite sprite) {
        if(_iconImg != null) {
            _iconImg.sprite = sprite;
        }
    }

    void Open() {
        if(_animator != null) {
            _animator.ResetTrigger("TurnOff");
            _animator.SetTrigger("TurnOn");
        }
    }

    void SetText(string text) {
        if(Utils.NotNull(Txt)) {
            Txt.text = text;
        }
    }

}
