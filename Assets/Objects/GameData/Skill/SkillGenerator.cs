using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillGenerator {
    public static SkillData Create(SkillTypes type) {
        switch(type) {
            case SkillTypes.CHANGE_1: return new SkillData(type:type, mp:1, reuseAble:true);
            case SkillTypes.CHANGE_ALL: return new SkillData(type:type, mp:2, reuseAble:true);
            case SkillTypes.PLUS: return new SkillData(type:type, mp:2, reuseAble:true);
            case SkillTypes.MINUS: return new SkillData(type:type, mp:2, reuseAble:true);
            case SkillTypes.CHANCE: return new SkillData(type:type, mp:3, reuseAble:false);
            case SkillTypes.KEEP: return new SkillData(type:type, mp:2, reuseAble:false);
            default: return null;
        }
    }
}
