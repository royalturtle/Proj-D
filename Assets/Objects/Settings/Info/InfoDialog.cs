using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class InfoDialog : MonoBehaviour {
    Animator _animator;
    string _termsConditions, _privacyPolicy, _attribution;
    [SerializeField] List<Toggle> _toggleList;
    [SerializeField] TextMeshProUGUI _text;
    bool _isOpened;

    void Awake() {
        _animator = GetComponent<Animator>();
        for(int i = 0; i < _toggleList.Count; i++) {
            _toggleList[i].onValueChanged.AddListener(
                (bool bOn) => {
                    if(bOn) {
                        UpdateView();
                    }
                }
            );
        }
    }

    public void Open() {
        if(_animator) {
            _animator.SetTrigger("TurnOn");
            if(!_isOpened){
                _isOpened = true;
                StartCoroutine(ReadFiles());
            }
        }
    }

    IEnumerator ReadFiles() {
        _termsConditions = ReadFile(GameConstants.FileTermsConditions);
        _privacyPolicy = ReadFile(GameConstants.FilePrivacyPolicy);
        _attribution = ReadFile(GameConstants.FileAttribution);
        UpdateView();
        yield return null;
    }

    void UpdateView() {
        int onIndex = OnIndex;
        string result = GetString(onIndex);
        if(_text) {
            _text.text = result;
            if(_text.GetComponent<RectTransform>()) {
                _text.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            }
        }
    }

    string GetString(int index) {
        switch(index) {
            case  0: return _termsConditions;
            case  1: return _privacyPolicy;
            case  2: return _attribution;
            default: return "";
        }
    }

    string FilePath(string fileName) { 
        return string.Format("{0}/{1}", Application.persistentDataPath , fileName);
    }

    string ReadFile(string fileName) {
        string filePath = FilePath(fileName);

        FileInfo fileInfo = new FileInfo(filePath);
        string text = "";

        if(fileInfo.Exists) {
            StreamReader reader = new StreamReader(filePath);
            text = reader.ReadToEnd();
            reader.Close();
        }
        return text;
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
}
