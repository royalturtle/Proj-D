using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewGameDialog : MonoBehaviour {
    List<CharacterData> characterDataList;
    int selectIndex;

    [SerializeField] List<SkillSingleDescriptionObject> SkillList;
    [SerializeField] TextMeshProUGUI NameTxt, HpTxt;

    public void Ready() {
        characterDataList = new List<CharacterData>();
        characterDataList.Add(CharacterGenerator.Create(type:CharacterTypes.NONE));

        ChangeSelect(0);
    }

    public void Next() {
        ChangeSelect(selectIndex + 1);
    }

    public void Previous() {
        ChangeSelect(selectIndex - 1);
    }

    void ChangeSelect(int value) {
        if(characterDataList != null && characterDataList.Count > 0) {
            if(value < 0) {
                selectIndex = characterDataList.Count - 1;
            }
            else if(value >= characterDataList.Count) {
                selectIndex = 0;
            }
            else {
                selectIndex = value;
            }
            UpdateView();
        }
    }

    void UpdateView() {
        Debug.Log("UPDATE");
        if(Utils.IsValidIndex(characterDataList, selectIndex)) {
            CharacterData data = characterDataList[selectIndex];
            if(data != null) {
                if(NameTxt != null) {
                    NameTxt.text = data.Type.ToString();
                }

                if(HpTxt != null) {
                    HpTxt.text = data.Hp.ToString();
                }

                if(SkillList != null) {
                    int i = 0;
                    List<SkillData> skills = data.Skills;
                    for(i = 0; i < SkillList.Count && i < skills.Count; i++) {
                        SkillList[i].SetData(skills[i]);
                    }
                    for(; i < SkillList.Count; i++) {
                        SkillList[i].SetData(null);
                    }
                }
            }
        }
    }

    
}
