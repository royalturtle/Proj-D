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
    public int curLangIndex = 0;    // ���� ����� �ε���

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
                result = "�ѱ���";
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

    // InitLang �Լ������� �����س��� ��� �ε������� �ִٸ� �������� , ���ٸ� �⺻���(����)�� �ε��� ���� �����´�.
    public void InitLang() {
        int langIndex = PlayerPrefs.GetInt(GameConstants.PrefLanguage, -1);
        int systemIndex = SupportLanguageList.FindIndex(s => s.ToLower() == Application.systemLanguage.ToString().ToLower());
        if (systemIndex == -1) systemIndex = 0;
        int index = langIndex == -1 ? systemIndex : langIndex;

        SetLangIndex(index); // ���� ������ �� SetLangIndex�� �Ű������� �־��ش� 
    }

    public void ChangeLangIndex(int index) {
        curLangIndex = index;   //initlang���� ���� ����� �ε��� ���� curLangIndex�� �־��� 
        PlayerPrefs.SetInt(GameConstants.PrefLanguage, curLangIndex);  //����
        SetLangList(curLangIndex);
        LocalizeChanged();  //�ؽ�Ʈ�� ���� ���� ����
        LocalizeNameChanged();
        LocalizeSettingChanged();   //����ٿ��� value����
    }

    public void SetLangIndex(int index) {
        curLangIndex = index;   //initlang���� ���� ����� �ε��� ���� curLangIndex�� �־��� 
        PlayerPrefs.SetInt(GameConstants.PrefLanguage, curLangIndex);  //����
        LocalizeChanged();  //�ؽ�Ʈ�� ���� ���� ����
        LocalizeNameChanged();
        LocalizeSettingChanged();   //����ٿ��� value����
    }
    
    [ContextMenu("��� ��������")]    //ContextMenu�� �������� �ƴ� ������ ���� ���� 
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

        // ������ �迭 ����
        string[] row = tsv.Split('\n'); //�����̽��� ������ �� �з� 
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
