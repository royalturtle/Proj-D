using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillMultipleObject : SelectManagerObject<SkillObject> {
    [SerializeField] List<SkillObject> SkillList;
    protected override List<SkillObject> GetList { get { return SkillList; } }

    [SerializeField] TextMeshProUGUI DescriptionTxt;

    [SerializeField] Button UseBtn;

    public SkillData SelectedSkill {
        get {
            SkillData result = null;
            SkillObject obj = GetSelected;
            if(Utils.NotNull(obj)) {
                result = obj.Skill;
            }
            return result;
        }
    }

    public void ReadyAction(System.Action useAction) {
        if(Utils.NotNull(UseBtn, useAction)) {
            UseBtn.onClick.AddListener(() => {
                useAction();
            });
        }
    }

    public void SetData(SkillManager skill) {
        if(skill != null && SkillList != null && skill.SkillList != null) {
            int i = 0;
            for(; i < SkillList.Count && i < skill.SkillList.Count; i++) {
                SkillList[i].SetData(skill.SkillList[i]);
            }
            for(; i < SkillList.Count; i++) {
                SkillList[i].SetData(null);
            }
        }
    }

    protected override void ClearAction() {
        if(DescriptionTxt != null) {
            DescriptionTxt.text = "";
        }
    }

    protected override void CheckAction() {
        if(DescriptionTxt != null) {
            if(SkillList[Selected].Skill != null) {
                DescriptionTxt.text = SkillList[Selected].Skill.Description;
            }
        }
    }

    public void CheckEnable(int mp) {
        SkillData skill = SelectedSkill;
        if(Utils.NotNull(skill)) {
            if(skill.Mp > mp) {
                ClearSelect();
            }
        }

        for(int i = 0; i < SkillList.Count; i++) {
            SkillList[i].Check(mp);
        }
    }

    protected override void SetInteractableAfter(bool value) {
        if(UseBtn != null) {
            UseBtn.interactable = value;
        }
    }
}
