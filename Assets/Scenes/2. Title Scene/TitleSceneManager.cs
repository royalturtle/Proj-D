using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleSceneManager : SceneBase {
    [SerializeField] NewGameDialog NewDlg;
    [SerializeField] TransitionObject TransitionObj;
    [SerializeField] AskDialogObject _deleteSaveDialog;

    [SerializeField] Button _continueButton, _newGameButton;
    [SerializeField] Animator _titleAnimator, _actionAnimator, _appearAnimator;

    bool IsContinueExist;

    protected override void AwakeAfter() {
        IsContinueExist = SaveManager.Check;
        
        if(_continueButton != null) {
            _continueButton.gameObject.SetActive(IsContinueExist);
        }

        if(_appearAnimator) {
            _appearAnimator.SetTrigger("On");
        }
    }

    protected override void StartAfter() {
        if(Utils.NotNull(NewDlg)) {
            NewDlg.Ready();
        }

        if(Utils.NotNull(TransitionObj)) {
            TransitionObj.SetOff();
        }

        if(_appearAnimator) {
            _appearAnimator.SetTrigger("TurnOff");
        }

        if(_titleAnimator) {
            _titleAnimator.SetTrigger("TurnOn");
        }
        if(_actionAnimator) {
            _actionAnimator.SetTrigger("TurnOn");
        }
    }

    void Update() {
        CheckQuitGame();
    }

    public void ContinueGame() {
        if(Utils.NotNull(TransitionObj)) {
            TransitionObj.TurnOn(onAction:() => {
                LoadInGameScene();
            },
            isTurnOnAuto:false);
        }
    }

    public void CheckNewGame() {
        if(IsContinueExist) {
            if(_deleteSaveDialog != null) {
                _deleteSaveDialog.Open(text:LanguageSingleton.Instance.GetGUI(13), okAction:delegate{RemoveContinueAndStart();});
            }
        } else {
            NewGame();
        }
    }

    void NewGame() {
        if(Utils.NotNull(TransitionObj)) {
            TransitionObj.TurnOn(onAction:() => {
                SaveManager.CreateNewGame();
                
                LoadInGameScene();
            },
            isTurnOnAuto:false);
        }
    }

    public void OpenSettings() {
        
    }

    void RemoveContinueAndStart() {
        if(IsContinueExist) {
            SaveManager.Delete();
            NewGame();
        }
    }
}
