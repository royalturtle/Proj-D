using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGenerator {
    public static CharacterData Create(CharacterTypes type) {
        switch(type) {
            default:
                return new CharacterData(
                    type:type, 
                    hp:10, 
                    skills: new List<SkillTypes> { SkillTypes.CHANGE_1, SkillTypes.CHANGE_ALL, SkillTypes.PLUS, SkillTypes.MINUS, SkillTypes.KEEP, SkillTypes.CHANCE}
                );
        }
    }
}
