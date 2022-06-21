using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData {    
    public CharacterTypes Type {get; private set;}
    public List<SkillData> Skills {get; private set;}
    public int Hp {get; private set;}

    public CharacterData(CharacterTypes type, int hp, List<SkillTypes> skills) {
        Type = type;
        Hp = hp;
        Skills = new List<SkillData>();
        if(skills != null) {
            for(int i = 0; i < skills.Count; i++) {
                Skills.Add(SkillGenerator.Create(type:skills[i]));
            }
        }
    }
}
