using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceMediator : MonoBehaviour {
    InGameSceneManager GameManager;
    DiceManager diceManager;

    [SerializeField] DiceMultipleObject DiceObj;
    [SerializeField] DiceRollObject RollObj;
    [SerializeField] DiceConfirmObject _confirmObj;

    [SerializeField] DiceEyesCollection _eyesCollection;

    public bool IsChanceRemain {
        get {
            return diceManager == null ? false : diceManager.IsChanceRemain;
        }
    }

    public void ResetChance() {
        if(diceManager != null) {
            diceManager.ResetChance();
        }
    }

    public int RecentResult {
        get {
            return Utils.NotNull(diceManager) ? diceManager.RecentResult : GameConstants.NULL_INT;
        }
    }

    public void Ready(InGameSceneManager gameManager, DiceManager dice, Action releaseAction) {
        GameManager = gameManager;
        diceManager = dice;

        if(Utils.NotNull(GameManager, diceManager)) {
            if(DiceObj != null) {
                DiceObj.Ready(manager:diceManager, rollObj:RollObj);
            }

            if(RollObj != null) {
                RollObj.PressAction = () => {
                    GameManager.SetInteractable(value:false, isRolling:true);
                };

                RollObj.ReleaseAction = releaseAction;
            }

            
            if(_confirmObj) {
                _confirmObj.Ready(
                    buttonAction: delegate {
                        _confirmObj.Close();
                        GameManager.SkipChance();
                    }
                );
            }

            UpdateView();
        }
    }

    public void LoadGame() {
        if(Utils.NotNull(diceManager)) {
            diceManager.LoadGame();
            int recent = RecentResult;
            if(Utils.NotNull(DiceObj)) {

                if(!diceManager.IsChange) {
                    DiceObj.OpenEnforce(SkillTypes.KEEP);
                }
                else if(diceManager.Additional > 0) {
                    DiceObj.OpenEnforce(SkillTypes.PLUS);
                }
                else if(diceManager.Additional < 0) {
                    DiceObj.OpenEnforce(SkillTypes.MINUS);
                }
                else if(diceManager.Chance >= 2 || (diceManager.Chance >= 1 && recent != GameConstants.NULL_INT)) {
                    DiceObj.OpenEnforce(SkillTypes.CHANCE);
                    if((diceManager.Chance >= 1 && recent != GameConstants.NULL_INT) && _confirmObj != null) {
                        _confirmObj.Open(value:recent, collection:_eyesCollection);
                    }
                }
            }
        }
    }

    public void LoadAttackAfter() {
        if(Utils.NotNull(diceManager)) {
            ChangeSelected();
        }
    }

    public void Reset() {
    // void Reset() {
        bool isUpdate = false;
        // Reset Data
        if(diceManager != null) {
            isUpdate = ChangeSelected();
            diceManager.ResetStatus();
        }

        // Reset UI (Select)
        if(DiceObj != null) {
            DiceObj.Reset();
        }

        // Reset UI (Roll)
        if(RollObj != null) {
            RollObj.Reset();
        }

        if(!isUpdate) {
            UpdateView();
        }
    }

    #region Skills
    public bool ChangeSelected() {
        bool result = false;
        if(Utils.NotNull(diceManager) && diceManager.IsChange) {
            int tmp = diceManager.Selected;
            result = diceManager.ChangeSelected();
            if(result) {
                StartCoroutine(ChangeDiceAnimation(index:tmp));
            }
        }
        return result;
    }

    public void ChangeAll() {
        if(Utils.NotNull(diceManager)) {
            diceManager.ChangeAll();
            StartCoroutine(ChangeAllDiceAnimation());
        }
    }

    IEnumerator ChangeDiceAnimation(int index) {
        DiceObj.ChangeAnimation(index, true);
        yield return StartCoroutine(WaitForRealSeconds(0.3f));
        UpdateView();
        yield return StartCoroutine(WaitForRealSeconds(0.3f));
        DiceObj.ChangeAnimation(index, false);
        yield return null;
    }

    
    IEnumerator ChangeAllDiceAnimation() {
        DiceObj.ChangeAnimationAll(true);
        yield return StartCoroutine(WaitForRealSeconds(0.3f));
        UpdateView();
        yield return StartCoroutine(WaitForRealSeconds(0.3f));
        DiceObj.ChangeAnimationAll(false);
        yield return null;
    }

    public void ChangeAdditional(int value) {
        if(Utils.NotNull(diceManager)) {
            diceManager.AddAdditional(value);
            if(Utils.NotNull(DiceObj)) {
                DiceObj.OpenEnforce(diceManager.Additional >= 0 ? SkillTypes.PLUS : SkillTypes.MINUS);
            }
        }
        StartCoroutine(ChangeAllDiceAnimation());
    }

    public void AddChance(int value) {
        if(Utils.NotNull(diceManager)) {
            diceManager.AddChance(value);
            UpdateView();
        }
        if(Utils.NotNull(DiceObj)) {
            DiceObj.OpenEnforce(SkillTypes.CHANCE);
        }
    }

    public void SetNotChange() {
        if(Utils.NotNull(diceManager)) {
            diceManager.SetNotChange();
            UpdateView();
        }
        if(Utils.NotNull(DiceObj)) {
            DiceObj.OpenEnforce(SkillTypes.KEEP);
        }
    }
    #endregion

    #region Roll
    public bool RollDice() {
        return diceManager != null ? diceManager.RollDice() : false;
    }

    public void RollDiceUI() {
        if(Utils.NotNull(diceManager)) {
            int count = 2;
            if(RollObj != null) {
                float perc = RollObj.Guage;
                perc = perc < 0.0f ? 0.0f : (perc > 1.0f ? 1.0f : perc);
                count += (int)(perc / 0.35f);
            }

            RollObj.ShowResult(
                diceManager.GetSelectedRandomList(count:count), 
                diceManager.RecentResult,
                confirmAction:delegate {
                    GameManager.SetMode(InGameSceneModes.ROLLING_AFTER);
                }
            );
        }
    }
    #endregion

    #region Additional
    public void SetActiveAdditional(bool value) {
        if(Utils.NotNull(_confirmObj, diceManager)) {
            if(value) { 
                _confirmObj.Open(value:diceManager.RecentResult, collection:_eyesCollection);
            } 
            else { 
                _confirmObj.Close(); 
            } 
        }
    }

    public void EmptyRollContent() {
        if(RollObj) {
            RollObj.EmptyContent();
        }
    }
    #endregion

    #region UI
    public void UpdateView() {
        if(Utils.NotNull(diceManager, DiceObj)) {
            DiceObj.SetData(diceManager);
        }
    }

    public void SetInteractable(bool value, bool isRolling = false) {
        if(DiceObj != null) {
            DiceObj.SetInteractable(value);
        }

        if(RollObj != null) {
            RollObj.SetInteractable(value || isRolling);
        }

        if(_confirmObj) {
            _confirmObj.SetInteractable(value);
        }
    }
    #endregion
    
    IEnumerator WaitForRealSeconds (float seconds) {
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup-startTime < seconds) {
            yield return null;
        }
    }

    public void PrintData() {
        if(diceManager != null) {
            diceManager.PrintData();
        }
    }
}
