using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceMultipleObject : SelectManagerObject<DiceSingleObject> {
    [SerializeField] List<DiceSingleObject> ObjList;
    [SerializeField] DiceEnforceObject enforceObj;
    protected override List<DiceSingleObject> GetList { get { return ObjList; } }

    DiceRollObject RollObj;
    DiceManager Manager;

    AudioSource _sfxAudio;
    [SerializeField] AudioClip _selectSFX, _deselectSFX;

    void Awake() {
        _sfxAudio = GetComponent<AudioSource>();
    }

    public void Ready(DiceManager manager, DiceRollObject rollObj) {
        Manager = manager;
        RollObj = rollObj;
    }

    public void SetData(DiceManager dice) {
        if(ObjList != null && dice != null && dice.DiceList != null) {
            int i = 0;
            for(; i < dice.DiceList.Count; i++) {
                ObjList[i].SetData(dice.DiceList[i], dice.Additional);
            }
            for(; i < ObjList.Count; i++) {
                ObjList[i].SetData(null);
            }
        }
    }

    public void Reset() {
        if(Manager != null) {
            Manager.ClearSelected();
        }
        for(int i = 0; i <ObjList.Count; i++) {
            ObjList[i].Diselected();
        }
        if(Utils.NotNull(enforceObj)) {
            enforceObj.Close();
        }
    }

    public void OpenEnforce(SkillTypes item) {
        if(Utils.NotNull(enforceObj, Manager)) {
            enforceObj.Open(Manager, item);
        }
    }

    public void ChangeAnimation(int index, bool value) {
        if(Utils.IsValidIndex(ObjList, index)) {
            ObjList[index].ChangeAnimation(value);
        }
    }

    public void ChangeAnimationAll(bool value) {
        for(int i = 0; i < ObjList.Count; i++) {
            ObjList[i].ChangeAnimation(value);
        }
    }

    protected override void ClearAction() {
        if(Manager != null) {
            Manager.ClearSelected();

            _sfxAudio.clip = _deselectSFX;
            _sfxAudio.Play();

            if(RollObj != null) {
                RollObj.Turn(false);
                RollObj.SetInteractable(false);
            }
        }
    }

    protected override void CheckAction() {
        if(Manager != null) {
            Manager.SetSelected(Selected);

            _sfxAudio.clip = _selectSFX;
            _sfxAudio.Play();

            if(RollObj != null) {
                RollObj.Turn(true);
                RollObj.SetInteractable(true);
            }   
        }
    }

}
