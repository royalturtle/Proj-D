using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneBase : MonoBehaviour {
    [SerializeField] protected NotDestroyObject NotDestroyObjOrig;
    protected NotDestroyObject NotDestroyObj;
    [SerializeField] protected AskDialogObject _askDialog;

    void Awake() {
        GameObject NotDestroyObjParent = GameObject.FindWithTag(GameConstants.TAG_NOT_DESTROY_OBJECT);
        if(NotDestroyObjParent == null && NotDestroyObjOrig != null) { 
            NotDestroyObjParent = Instantiate(NotDestroyObjOrig).gameObject; 
        }
        if(NotDestroyObjParent != null) {
            NotDestroyObj = NotDestroyObjParent.GetComponent<NotDestroyObject>();
            if(NotDestroyObj != null) NotDestroyObj.StartAction();
        }
        
        AwakeAfter();
    }
    protected virtual void AwakeAfter() {}

    void Start() {
        StartAfter();
    }
    protected virtual void StartAfter() { }

    protected void LoadTitleScene() {
        SceneManager.LoadScene(GameConstants.SCENE_TITLE);
    }

    protected void LoadInGameScene() {
        SceneManager.LoadScene(GameConstants.SCENE_INGAME);
    }

    protected void LoadClearScene() {
        SceneManager.LoadScene(GameConstants.SCENE_CLEAR);
    }

    protected void CheckQuitGame() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(_askDialog != null) {
                _askDialog.Open(text:LanguageSingleton.Instance.GetGUI(27), okAction:QuitGame);
            }
        }
    }

    public void QuitGame() {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }
}
