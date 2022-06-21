using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageDialog : MonoBehaviour {
    Animator _animator;
    [SerializeField] List<Toggle> _toggleList;

    void Awake() {
        _animator = GetComponent<Animator>();
    }

    int GetLangIndex {
        get {
            return PlayerPrefs.GetInt(GameConstants.PrefLanguage, -1);
        }
    }

    int OnIndex {
        get {
            int result = GameConstants.NULL_INT;
            if(_toggleList != null) {
                for(int i = 0; i < _toggleList.Count; i++) {
                    if(_toggleList[i].isOn) {
                        result = i;
                        break;
                    }
                }
            }
            return result;
        }
    }

    public void Confirm() {
        int langIndex = GetLangIndex;
        int onIndex = OnIndex;

        Debug.Log("Confirm " + langIndex.ToString() + " " + onIndex.ToString() );

        if(langIndex != onIndex) {
            LanguageSingleton.Instance.ChangeLangIndex(onIndex);
        }

        if(_animator != null) {
            _animator.SetTrigger("TurnOff");
        }
    }

    public void Open() {
        Debug.Log("Open");
        int langIndex = GetLangIndex;

        if(Utils.IsValidIndex(_toggleList, langIndex)) {
            _toggleList[langIndex].isOn = true;
        }

        if(_animator != null) {
            _animator.SetTrigger("TurnOn");
        }
    }
}
