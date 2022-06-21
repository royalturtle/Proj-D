using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioMediator : MonoBehaviour {
    [SerializeField] CameraEffects cameraEffects;
    [field:SerializeField] public CharacterObject CharacterObj {get; private set;}
    [field:SerializeField] public BossObject BossObj {get; private set;}
    [SerializeField] Animator DoorObj;
    [SerializeField] Animator HpObj, PauseObj, RollObj, SkillObj, DiceObj, BossHpObj;
    [SerializeField] ResultTextObject _resultText;
    [SerializeField] CircleTransition _circleTransition;
    [SerializeField] Animator _gameOverUIAnimator;
    [SerializeField] List<Animator> _darkPanels;

    public void SetDarkPanels(bool value, bool isAnimation) {
        if(_darkPanels != null){
            for(int i = 0; i < _darkPanels.Count; i++) {
                SetDarkPanel(value:value, index:i, isAnimation:isAnimation);
            }
        }
    }

    public void SetDarkPanel(bool value, int index, bool isAnimation) {
        if(Utils.IsValidIndex(_darkPanels, index)) {
            if(isAnimation) {                
                _darkPanels[index].SetTrigger(value ? "TurnOn" : "TurnOff");
            }
            else {
                _darkPanels[index].SetTrigger(value ? "On" : "Off");
            }
        }
    }

    public void LoadGame() {
        if(CharacterObj != null) {
            CharacterObj.transform.localPosition = new Vector3(0.0f, -1.5f, 0.0f);
        }

        StartAnim();
    }

    public void LoadAttackAfter() {
        if(CharacterObj != null) {
            CharacterObj.transform.localPosition = new Vector3(0.0f, -1.5f, 0.0f);
        }
        SetTrigger(new Animator[] {HpObj, PauseObj}, true);
    }

    public IEnumerator GameStart() {
        SetDarkPanel(value:false, index:0, isAnimation:true);
        yield return new WaitForSeconds(2.0f);
        SetDarkPanel(value:false, index:1, isAnimation:true);
        yield return new WaitForSeconds(2.0f);
        if(CharacterObj != null) {
            CharacterObj.WalkY(5);
        }
        
        yield return new WaitForSeconds(4.0f);
    }

    public void CameraShake(float amount, float duration) {
        if(cameraEffects != null) {
            StartCoroutine(cameraEffects.Shake(amount:amount, duration:duration));
        }
    }

    public void Attack() {
        CameraShake(amount:0.2f, duration:0.2f);
        
        if(BossObj != null) {
            BossObj.Damage();
        }
    }

    public void Damage() {
        if(CharacterObj != null) {
            CharacterObj.Damage();
        }

        CameraShake(amount:0.2f, duration:0.2f);
    }

    public IEnumerator GameClear() {
        SetTrigger(new Animator[] {HpObj, PauseObj}, false);

        if(CharacterObj != null) {
            CharacterObj.WalkY(4);
            yield return StartCoroutine(WaitForRealSeconds(4.0f));
        }

        if(DoorObj != null) {
            DoorObj.SetTrigger("Open");
            yield return StartCoroutine(WaitForRealSeconds(2.0f));
        }

        if(CharacterObj != null) {
            CharacterObj.WalkY(3);
            yield return StartCoroutine(WaitForRealSeconds(2.5f));
        }

        yield return null;
    }

    IEnumerator WaitForRealSeconds (float seconds) {
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup-startTime < seconds) {
            yield return null;
        }
    }

    public void StartAnim() {
        SetTrigger(new Animator[] {HpObj, PauseObj, SkillObj, DiceObj, BossHpObj}, true);
    }

    public void ReadyEnemyAnim() {
        SetTrigger(BossHpObj, true);
    }

    public void ReadyGimicAnim() {
        SetTrigger(new Animator[] {SkillObj, DiceObj}, true);
    }

    public void CloseGimicAnim() {
        SetTrigger(new Animator[] {RollObj, SkillObj, DiceObj}, false);
    }

    public void ClearStageAnim() {
        SetTrigger(BossHpObj, false);
    }

    void SetTrigger(Animator anim, bool value)  {
        if(anim != null) {
            anim.ResetTrigger("TurnOn");
            anim.ResetTrigger("TurnOff");
            anim.SetTrigger(value ? "TurnOn" : "TurnOff");
        }
    }

    void SetTrigger(Animator[] anims, bool value) {
        if(anims != null) {
            for(int i = 0; i < anims.Length; i++) {
                SetTrigger(anims[i], value);
            }
        }
    }

    public IEnumerator NextStage() {
        CameraShake(amount:0.2f, duration:0.2f);

        if(BossObj != null) {
            BossObj.Death();
        }
        yield return StartCoroutine(WaitForRealSeconds(3.0f));
    }

    public void GameOver() {
        if(CharacterObj != null) {
            CharacterObj.Death();
        }
        if(_circleTransition != null) {
            _circleTransition.TurnOn(afterAction:delegate {
                if(_gameOverUIAnimator) {
                    _gameOverUIAnimator.SetTrigger("TurnOn");
                }
            });
        }
    }

    #region ResultText
    public void ShowResultText(string text, Color32 color) {
        if(_resultText != null) {
            _resultText.Show(text, color);
        }
    }

    public void ChangeResultText(string text, Color32 color) {
        if(_resultText != null) {
            _resultText.Change(text, color);
        }
    }

    public void HideResultText() {
        if(_resultText != null) {
            _resultText.Hide();
        }
    }    
    #endregion
}
