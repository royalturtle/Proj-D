using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillMediator : MonoBehaviour {
    InGameSceneManager GameManager;

    [SerializeField] SkillMultipleObject SkillObj;
    [SerializeField] Button Btn;

    SkillManager skillManager;

    AudioSource _sfxAudio;

    void Awake() {
        _sfxAudio = GetComponent<AudioSource>();
    }

    public void Ready(InGameSceneManager gameManager, SkillManager skill) {
        GameManager = gameManager;
        skillManager = skill;
        if(Utils.NotNull(GameManager, skillManager)) {
            UpdateView();
        }

        if(Utils.NotNull(SkillObj)) {
            SkillObj.ReadyAction(useAction:Use);
        }
    }

    public void Reset(HpMediator hp) {
        if(Utils.NotNull(skillManager)) {
            skillManager.Reset();
        }

        CheckEnable(hp);
    }

    public void UpdateView() {
        if(Utils.NotNull(SkillObj)) {
            SkillObj.SetData(skillManager);
        }
    }

    public void SetInteractable(bool value) {
        if(Btn != null) {
            Btn.interactable = value;
        }

        if(SkillObj != null) {
            SkillObj.SetInteractable(value);
        }
    }

    void CheckEnable(HpMediator hp) {
        if(Utils.NotNull(hp, hp.Mp, SkillObj)) {
            SkillObj.CheckEnable(hp.Mp.Current);
        }
    }

    void Use() {
        bool result = false;

        SkillObject obj = SkillObj.GetSelected;
        SkillData skill = SkillObj.SelectedSkill;

        if(!Utils.NotNull(skill)) {
            if(Utils.NotNull(GameManager)) {
                GameManager.OpenWarning("사용할 스킬을 선택하고 누르세요.");
            }
        }
        else if(skill.IsUseAble) {
            if(Utils.NotNull(GameManager)) {
                GameManager.OpenWarning("두 번 사용할 수 없습니다.");
            }
        }
        else if(Utils.NotNull(GameManager, GameManager.Hp, GameManager.Dice)) {
            HpMediator hp = GameManager.Hp;
            DiceMediator dice = GameManager.Dice;

            bool isUseAble = hp.IsSkillUseable(skill);

            if(isUseAble) {
                switch(skill.Type) {
                    case SkillTypes.CHANGE_1:
                        bool changeResult = dice.ChangeSelected();
                        if(!changeResult) {
                            GameManager.OpenWarning(LanguageSingleton.Instance.GetGUI(26));
                        }
                        else {
                            result = hp.UseMp(skill.Mp);
                        }
                        break;
                    case SkillTypes.CHANGE_ALL:
                        dice.ChangeAll();
                        result = hp.UseMp(skill.Mp);
                        break;
                    case SkillTypes.PLUS:
                        dice.ChangeAdditional(1);
                        result = hp.UseMp(skill.Mp);
                        break;
                    case SkillTypes.MINUS:
                        dice.ChangeAdditional(-1);
                        result = hp.UseMp(skill.Mp);
                        break;
                    case SkillTypes.CHANCE:
                        dice.AddChance(1);
                        result = hp.UseMp(skill.Mp);
                        break;
                    case SkillTypes.KEEP:
                        dice.SetNotChange();
                        result = hp.UseMp(skill.Mp);
                        break;
                }
                if(result) {
                    if(obj != null) {
                        obj.Use();
                    }
                    if(_sfxAudio != null) {
                        _sfxAudio.Play();
                    }
                    skill.AddUseCount();
                    CheckEnable(hp);
                    if(GameManager != null) {
                        GameManager.Save();
                    }
                }
            }
            else {
                GameManager.OpenWarning(text:LanguageSingleton.Instance.GetGUI(19));
            }

            
        }
        // return result;
    }
}
