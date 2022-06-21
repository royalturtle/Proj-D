using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AttackObject : MonoBehaviour {    
    [SerializeField] float RotateTime = 2.0f;
    [SerializeField] List<AttackGoalObject> goalObjList;
    [SerializeField] RectTransform _pinObj;
    [SerializeField] CustomButton _attackButton;
    [SerializeField] Sprite _validPinSprite, _invalidPinSprite;
    [SerializeField] ScenarioMediator _scenario;

    AudioSource _sfxAudio;

    float minBlank = 0.05f, start = 0.15f, end = 0.05f;
    float SumMax {get{return 1.0f - end;}}

    float currentTime;
    bool IsWorking;
    public int AttackCount {get; private set;}
    public int RemainAttack {get; private set;}

    Action FinishAct;

    Animator _animator;

    void Awake() {
        _animator = GetComponent<Animator>();
    }

    void Update() {
        if(IsWorking) {
            bool isEnd = UpdateFill(Time.deltaTime);
            if(isEnd) {
                FinishAttack();
            }
        }
    }

    public void Ready(Func<int, bool> attackAct, Action finishAct) {
        FinishAct = finishAct;

        if(Utils.NotNull(_attackButton, attackAct)) {
            _attackButton.DownAction = () => {
                if(IsWorking) {
                    bool isAttacked = CheckAttack();
                    if(isAttacked) {
                        bool isDeath = attackAct(1);
                        if(isDeath) {
                            FinishAttack();
                        }
                    }
                    else {
                        if(_scenario != null) {
                            _scenario.CameraShake(amount:0.2f, duration:0.2f);
                        }
                        SetPinSprite(false);
                        FinishAttack();
                    }
                }
            };
        }
    }

    void SetPinSprite(bool value) {
        if(_pinObj) {
            _pinObj.GetComponent<Image>().sprite = value ? _validPinSprite : _invalidPinSprite;
        }
    }

    bool CheckAttack() {
        float value = currentTime / RotateTime;
        bool result = false;
        for(int i = 0; i < goalObjList.Count; i++) {
            if(goalObjList[i].Start <= value && value <= goalObjList[i].End) {
                result = true;
                --RemainAttack;
                break;
            }
        }

        return result;
    }

    public void StartAttack() {
        currentTime = 0.0f;
        RemainAttack = AttackCount;
        IsWorking = true;
        if(_sfxAudio) {
            _sfxAudio.Play();
        }
    }

    public void FinishAttack() {
        IsWorking = false;
        if(_sfxAudio) {
            _sfxAudio.Stop();
        }
        if(Utils.NotNull(FinishAct)) {
            FinishAct();
        }
    }

    bool UpdateFill(float value) {
        bool isEnd = false;
        currentTime += value;
        if(currentTime >= RotateTime) {
            currentTime = 2.0f;
            isEnd = true;
        }
        
        if(Utils.NotNull(_pinObj)) {
            _pinObj.rotation = Quaternion.Euler(0,0,-((currentTime / RotateTime)*360.0f));
        }

        return isEnd;
    }

    public void Close() {
        if(_animator != null) {
            _animator.SetTrigger("TurnOff");
        }
    }

    public void Open(List<float> goalList) {
        if(Utils.NotNull(_pinObj)) {
            _pinObj.rotation = Quaternion.Euler(0,0,0);
        }
        SetPinSprite(true);
        if(Utils.NotNull(goalList)) {
            float sum = 0.0f;
            
            List<float> dataList = new List<float>();
            sum = AddList(dataList, start, sum);
            for(int i = 0; i < goalList.Count && sum <= SumMax; i++) {
                sum = AddList(dataList, goalList[i], sum);
                sum = AddList(dataList, minBlank, sum);
            }

            if(dataList.Count % 2 == 0) {
                dataList.Add(end);
            }

            // º¸Á¤
            float remain = 1.0f - sum;
            int to = (dataList.Count + 1) / 2;
            for(int i = 0; i < to - 1 && remain > 0.0f; i++) {
                float value = UnityEngine.Random.Range(0.0f, remain);
                remain -= value;
                dataList[i * 2] += value;
            }

            if(remain > 0.0f) {
                dataList[(to - 1) * 2] += remain;
            }

            int j = 0;
            sum = 0.0f;
            for(; j < dataList.Count && j / 2 < goalObjList.Count; j++) {
                if(j % 2 == 1) {
                    goalObjList[j / 2].SetData(isEnable:true,start:sum, end:sum+dataList[j]);
                }
                sum += dataList[j];
            }

            for(; j / 2 < goalObjList.Count; j++) {
                goalObjList[j / 2].SetData(isEnable:false);
            }

            AttackCount = dataList.Count / 2; 

            
            if(_animator != null) {
                _animator.SetTrigger("TurnOn");
            }
        }
    }

    float AddList(List<float> list, float value, float sum) {
        if(Utils.NotNull(list) && sum < SumMax) {
            float tmp = value;
            if(tmp + sum > SumMax) {
                tmp = SumMax - sum;
            }

            list.Add(tmp);
            sum += tmp;
        }
        return sum;
    }
}
