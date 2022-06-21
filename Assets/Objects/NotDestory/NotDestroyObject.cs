using UnityEngine;
using UnityEngine.SceneManagement;

public class NotDestroyObject : MonoBehaviour {
    bool IsStart;
    public StorySceneArguments StoryArgs {get; private set;}

    void Start() {
        StartAction();
    }

    public void StartAction() {
        if(!IsStart) {
            DontDestroyOnLoad(this.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            BetterStreamingAssets.Initialize();
            LanguageSingleton.Instance.GetLang();
            LanguageSingleton.Instance.InitLang();
            
            CreateInfoText(GameConstants.FileTermsConditions);
            CreateInfoText(GameConstants.FilePrivacyPolicy);
            CreateInfoText(GameConstants.FileAttribution);
            IsStart = true;
        }
    }

    void CreateInfoText(string fileName) {
        FileUtils.DeletePersistentFile(fileName);
        FileUtils.StreammingAssetToPersistent(fileName);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if(scene.buildIndex == GameConstants.SCENE_START) {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Destroy(this.gameObject);
        }
    }

    public void SetSceneArguments(StorySceneArguments storyArgs = null) {
        StoryArgs = storyArgs;
    }

    public void ClearSceneArguments() {
        StoryArgs = null;
    }
    
    void CheckPlayerPrefs() {
        PlayerPrefs.SetFloat(GameConstants.PREF_BGM_SOUND, PlayerPrefs.GetFloat(GameConstants.PREF_BGM_SOUND, 0.5f));
        PlayerPrefs.SetInt(GameConstants.PREF_BGM_IS_ON, PlayerPrefs.GetInt(GameConstants.PREF_BGM_IS_ON, 1));
        PlayerPrefs.SetFloat(GameConstants.PREF_SFX_SOUND, PlayerPrefs.GetFloat(GameConstants.PREF_SFX_SOUND, 0.5f));
        PlayerPrefs.SetInt(GameConstants.PREF_SFX_IS_ON, PlayerPrefs.GetInt(GameConstants.PREF_SFX_IS_ON, 1));
    }

    #if UNITY_ANDROID
	public void UsedOnlyForAOTCodeGeneration() {
		//Bug reported on github https://github.com/aws/aws-sdk-net/issues/477
		//IL2CPP restrictions: https://docs.unity3d.com/Manual/ScriptingRestrictions.html
		//Inspired workaround: https://docs.unity3d.com/ScriptReference/AndroidJavaObject.Get.html

		AndroidJavaObject jo = new AndroidJavaObject("android.os.Message");
		int valueString = jo.Get<int>("what");
	}
	#endif
}
