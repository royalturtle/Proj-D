using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneManager : SceneBase {
    protected override void StartAfter() {
        LoadTitleScene();
    }
}
