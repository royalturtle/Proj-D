using UnityEngine;

public class MediatorBase : MonoBehaviour {
    protected InGameSceneManager GameManager;

    public void Ready(InGameSceneManager gameManager) {
        GameManager = gameManager;
        ReadyAfter();
    }

    protected virtual void ReadyAfter() {

    }
}
