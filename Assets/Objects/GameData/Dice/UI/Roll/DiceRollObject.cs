using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceRollObject : MonoBehaviour {
    [SerializeField] CustomButton RollBtn;
    [SerializeField] GuageObject GuageObj;
    public float Guage {get{return GuageObj != null ? GuageObj.Guage : 0.0f;}}

    float PressCurrent;
    [SerializeField] float PressMax = 1.0f;

    [SerializeField] Image ResultImg, _tmpImg;
    [SerializeField] DiceEyesCollection eyesCollection;
    [SerializeField] Animator _effectAnimator;

    Animator _animator;

    public Action PressAction, ReleaseAction;
    Action _confirmAction;

    List<int> _randomList;
    int _result, _changeIndex;

    AudioSource _sfxAudio;
    [SerializeField] AudioClip _rollSFX, _confirmSFX;

    void Awake() {
        _animator = GetComponent<Animator>();
        _sfxAudio = GetComponent<AudioSource>();
    }

    void Start() {
        if(RollBtn != null) {
            RollBtn.DownAction = PressButton;
            RollBtn.ReleaseAction = ReleaseButton;
        }
    }

    void Update() {
        if(RollBtn != null && RollBtn.IsPressing) {
            if(GuageObj != null) {
                PressCurrent += Time.deltaTime;
                float value = PressCurrent / PressMax;
                value = value > 1.0f ? 1.0f : (value < 0.0f ? 0.0f : value);
                GuageObj.SetGuage(value);
            }
        }
    }

    public void Reset() {
        PressCurrent = 0.0f;
        if(GuageObj != null) {
            GuageObj.Close();
        }
        if(RollBtn != null) {
            RollBtn.SetInteractable(false);
        }
        if(ResultImg != null && eyesCollection != null) {
            ResultImg.sprite = eyesCollection.Question;
        }

        if(_animator != null) {
            _animator.ResetTrigger("TurnOn");
            _animator.SetTrigger("ChangeOff");
            _animator.SetTrigger("TurnOff");
        }
    }
    
    void PressButton() {
        PressCurrent = 0.0f;
        if(GuageObj != null) {
            GuageObj.Open(0.0f);
        }
        if(PressAction != null) {
            PressAction();
        }
    }

    void ReleaseButton() {
        if(ReleaseAction != null) {
            ReleaseAction();
        }
    }

    public void Turn(bool value) {
        if(_animator != null) {
            _animator.ResetTrigger("TurnOn");
            _animator.ResetTrigger("TurnOff");
            _animator.SetTrigger(value ? "TurnOn" : "TurnOff");
        }
    }

    public void SetInteractable(bool value) {
        if(RollBtn != null) {
            RollBtn.SetInteractable(value);
        }
    }

    public void ShowResult(List<int> randomList, int result, Action confirmAction) {
        if(_animator != null) {
            _animator.SetTrigger("ChangeOff");
            _confirmAction = confirmAction;
            _changeIndex = 0;
            _randomList = randomList;
            _result = result;
            if(ResultImg != null && eyesCollection != null) {
                // ResultImg.sprite = null;
                // _tmpImg.sprite = null;
                ResultImg.enabled = false;
                _tmpImg.sprite = eyesCollection.GetWhite(randomList != null && randomList.Count > 0 ? _randomList[_changeIndex++] : _result);
            
                _animator.SetTrigger("Changing");
            }
        }
    }

    public void ChangeDice() {
        Rolling(isEnd:!Utils.IsValidIndex(_randomList, _changeIndex));
    }

    void Rolling(bool isEnd) {
        if(_sfxAudio != null) {
            _sfxAudio.clip = _rollSFX;
            _sfxAudio.Play();
        }
        if(_animator != null && _tmpImg != null) {
            if(ResultImg != null) {
                ResultImg.sprite = _tmpImg.sprite;
                if(_changeIndex <= 1) {
                    ResultImg.enabled = true;
                }
            }
            _tmpImg.sprite = eyesCollection.GetWhite(isEnd ? _result : _randomList[_changeIndex++]);
            _animator.SetTrigger(isEnd ? "ChangeEnd" : "Changing");
        }
    }

    public void Confirm() {
        if(_sfxAudio != null) {
            _sfxAudio.clip = _confirmSFX;
            _sfxAudio.Play();
        }
        if(_animator != null && ResultImg != null ) {
            _tmpImg.sprite = eyesCollection.GetBlue(_result);
            _animator.SetTrigger("Confirm");
        }
    }

    public void AfterConfirm() {
        if(_effectAnimator != null) {
            _effectAnimator.SetTrigger("TurnOn");
        }
        if(_confirmAction != null) {
            _confirmAction();
        }
    }

    public void EmptyContent() {
        if(GuageObj) {
            GuageObj.Close();
        }
        if(_animator) {
            _animator.SetTrigger("ChangeOff");
        }
    }

    IEnumerator WaitForRealSeconds (float seconds) {
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup-startTime < seconds) {
            yield return null;
        }
    }
}
