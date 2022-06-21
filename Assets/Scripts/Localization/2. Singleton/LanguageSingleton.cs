using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Threading;

public class LanguageSingleton {
    string FileName { get { return "lang.tsv";}}
    string FilePath { get { return string.Format("{0}/{1}", Application.persistentDataPath , FileName);}}

    public List<string> SupportLanguageList {get; private set;}
    public int curLangIndex = 0;    // 현재 언어의 인덱스

    public Dictionary<LocalizationTypes, Dictionary<int, string>> Data {get; private set;}

    public event System.Action LocalizeChanged = () => { };
    public event System.Action LocalizeNameChanged = () => { };
    public event System.Action LocalizeSettingChanged = () => { };

    #region Singleton
    static LanguageSingleton _instance = null;
    public static LanguageSingleton Instance {
        get {
            if(_instance == null) {
                _instance = new LanguageSingleton();
            }
            return _instance;
        }
    }
    #endregion

    public string CurrentLanguage {
        get {
            string result = "English";
            if(curLangIndex == 1) {
                result = "한국어";
            }
            return result;
        }
    }

    LanguageSingleton() {
        Data = new Dictionary<LocalizationTypes, Dictionary<int, string>>();
        Data.Add(LocalizationTypes.STORY, new Dictionary<int, string>());
        Data.Add(LocalizationTypes.SKILL_NAME, new Dictionary<int, string>());
        Data.Add(LocalizationTypes.SKILL_DESCRIPTION, new Dictionary<int, string>());
        Data.Add(LocalizationTypes.GUI, new Dictionary<int, string>());
        Data.Add(LocalizationTypes.GIMIC_DESCRIPTION, new Dictionary<int, string>());

        SupportLanguageList = new List<string>{"english", "korean"};

        FileUtils.DeletePersistentFile(FileName);
        FileUtils.StreammingAssetToPersistent(FileName);
    }

    // InitLang 함수에서는 저장해놓은 언어 인덱스값이 있다면 가져오고 , 없다면 기본언어(영어)의 인덱스 값을 가져온다.
    public void InitLang() {
        int langIndex = PlayerPrefs.GetInt(GameConstants.PrefLanguage, -1);
        int systemIndex = SupportLanguageList.FindIndex(s => s.ToLower() == Application.systemLanguage.ToString().ToLower());
        if (systemIndex == -1) systemIndex = 0;
        int index = langIndex == -1 ? systemIndex : langIndex;

        SetLangIndex(index); // 값을 가져온 뒤 SetLangIndex의 매개변수로 넣어준다 
    }

    public void ChangeLangIndex(int index) {
        curLangIndex = index;   //initlang에서 구한 언어의 인덱스 값을 curLangIndex에 넣어줌 
        PlayerPrefs.SetInt(GameConstants.PrefLanguage, curLangIndex);  //저장
        SetLangList(curLangIndex);
        LocalizeChanged();  //텍스트들 현재 언어로 변경
        LocalizeNameChanged();
        LocalizeSettingChanged();   //드랍다운의 value변경
    }

    public void SetLangIndex(int index) {
        curLangIndex = index;   //initlang에서 구한 언어의 인덱스 값을 curLangIndex에 넣어줌 
        PlayerPrefs.SetInt(GameConstants.PrefLanguage, curLangIndex);  //저장
        LocalizeChanged();  //텍스트들 현재 언어로 변경
        LocalizeNameChanged();
        LocalizeSettingChanged();   //드랍다운의 value변경
    }
    
    [ContextMenu("언어 가져오기")]    //ContextMenu로 게임중이 아닐 때에도 실행 가능 
    public void GetLang() {
        SetLangList(curLangIndex);
        // foreach(LocalizationData item in Data.Values) { item.GetLangCo(curLangIndex); }
    }
    
    void SetLangList(int index) {
        FileInfo fileInfo = new FileInfo(FilePath);
        string tsv = "";

        if(fileInfo.Exists) {
            StreamReader reader = new StreamReader(FilePath);
            tsv = reader.ReadToEnd();
            reader.Close();
        }

        // 이차원 배열 생성
        string[] row = tsv.Split('\n'); //스페이스를 기준을 행 분류 
        int rowSize = row.Length;

        for(int i = 0; i < rowSize; i++) {
            string[] column = row[i].Split('\t');
            LocalizationTypes majorType = (LocalizationTypes)int.Parse(column[0]);
            int minorType = int.Parse(column[1]);
            string value = column[index+2];

            Dictionary<int, string> dict = Data[majorType];
            if(!dict.ContainsKey(minorType)) Data[majorType].Add(minorType, value);
            else Data[majorType][minorType] = value;
        }
    }

    public string GetText(int index) {
        return Instance.Data[LocalizationTypes.STORY][index].Replace("\\n", "\n");
    }

    public string GetSkillName(SkillTypes type) {
        return Instance.Data[LocalizationTypes.SKILL_NAME][(int)type].Replace("\\n", "\n");
    }

    public string GetSkillDescription(SkillTypes type) {
        return Instance.Data[LocalizationTypes.SKILL_DESCRIPTION][(int)type].Replace("\\n", "\n");
    }

    public string GetGUI(int index) {
        return Instance.Data[LocalizationTypes.GUI][index].Replace("\\n", "\n");
    }
}
