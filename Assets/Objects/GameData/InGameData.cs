using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[DataContract]
public class InGameData {
    [DataMember] public InGameSceneModes Mode {get; private set;}
    [DataMember] public InGameSceneModes FormerMode {get; private set;}
    [DataMember] public DiceManager Dice {get; private set;}
    [DataMember] public SkillManager Skill {get; private set;}
    [DataMember] public HpData Hp {get; private set;}
    [DataMember] public MpData Mp {get; private set;}
    [DataMember] public StageManager Stage {get; private set;}

    public InGameData(int hpMax = 5) {
        // Mode = InGameSceneModes.BATTLE_START;
        Mode = InGameSceneModes.SCENARIO;
        FormerMode = InGameSceneModes.NONE;
        Dice = new DiceManager(startCount:3);
        Skill = new SkillManager(
            skills : new List<SkillTypes> {
                SkillTypes.CHANGE_1,
                SkillTypes.CHANGE_ALL,
                SkillTypes.PLUS,
                SkillTypes.MINUS,
                SkillTypes.KEEP,
                SkillTypes.CHANCE,
            }
        );
        Hp = new HpData(hpMax:hpMax);
        Mp = new MpData(max:3);
        Stage = new StageManager();
    }

    public void SetMode(InGameSceneModes mode) {
        Mode = mode;
    }

    public InGameSceneModes NextMode() {
        FormerMode = Mode;
        Mode = InGameSceneModes.NONE;
        return FormerMode;
    }

    public bool CheckBoth(InGameSceneModes mode) {
        return mode == FormerMode || mode == Mode;
    }
}
