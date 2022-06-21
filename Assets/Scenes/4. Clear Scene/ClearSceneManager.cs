using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearSceneManager : SceneBase {
    [SerializeField] TransitionObject TransitionObj;
    [SerializeField] GameObject HideObj;
    [SerializeField] CharacterObject CharacterObj;
    [SerializeField] Animator GoalDiceObj;
    [SerializeField] Button SkipBtn;
    [SerializeField] ResultTextObject _resultText;
    [SerializeField] BGMMediator _bgm;

    protected override void StartAfter() {
        if(TransitionObj != null) {
            TransitionObj.SetOn();
            if(HideObj != null) {
                HideObj.SetActive(false);
            }
            TransitionObj.TurnOff();
        }
        StartCoroutine(Ending());
    }

    void Update() {
        CheckQuitGame();
    }

    IEnumerator Ending() {
        if(CharacterObj != null) {
            CharacterObj.WalkY(4);
            yield return StartCoroutine(WaitForRealSeconds(4.5f));
        }
        if(GoalDiceObj != null) {
            GoalDiceObj.SetTrigger("Small");
            yield return StartCoroutine(WaitForRealSeconds(3.0f));
            if(_bgm) {
                _bgm.SlowDown();
            }
            yield return StartCoroutine(WaitForRealSeconds(1.0f));
        }

        if(SkipBtn != null) {
            SkipBtn.gameObject.SetActive(false);
        }

        if(_resultText != null) {
            if(_bgm) {
                _bgm.GameClear();
            }
            _resultText.Show();
            yield return StartCoroutine(WaitForRealSeconds(3.0f));
            _resultText.Hide();
            yield return StartCoroutine(WaitForRealSeconds(1.0f));
        }

        if(TransitionObj != null) {
            TransitionObj.TurnOn(
                onAction : () => {
                    LoadTitleScene();
                },
                isTurnOnAuto:false
            );
        }

        yield return null;
    }

    public void Skip() {
        if(TransitionObj != null) {
            TransitionObj.TurnOn(onAction:() =>{
                LoadTitleScene();
            });
        }
    }

    IEnumerator WaitForRealSeconds (float seconds) {
        float startTime = Time.realtimeSinceStartup;
        while (Time .realtimeSinceStartup-startTime < seconds) {
            yield return null;
        }
    }
}
