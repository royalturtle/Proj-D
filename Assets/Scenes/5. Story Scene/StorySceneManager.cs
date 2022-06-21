using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorySceneManager : SceneBase {
    [SerializeField] TransitionObject TransitionObj;
    [SerializeField] GameObject HideObj;

    protected override void StartAfter() {
        if(TransitionObj != null) {
            TransitionObj.SetOn();
            if(HideObj != null) {
                HideObj.SetActive(false);
            }
            TransitionObj.TurnOff();
        }
    }

    public void Skip() {
        if(TransitionObj != null) {
            TransitionObj.TurnOn(onAction:() =>{
                LoadInGameScene();
            });
        }
    }
}
