using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[DataContract]
public class SkillManager {
    public static readonly int SkillTypesCount = 6;

    [DataMember] public List<SkillData> SkillList { get; private set; }

    public SkillManager(List<SkillTypes> skills = null) {
        SkillList = new List<SkillData>();

        if(skills != null) {
            for(int i = 0; i < skills.Count && i < SkillTypesCount; i++) {
                SkillList.Add(SkillGenerator.Create((SkillTypes)i));
            }
        }
    }

    public void Reset() {
        if(Utils.NotNull(SkillList)) {
            for(int i = 0; i < SkillList.Count ; i++) {
                if(SkillList[i] != null) {
                    SkillList[i].Reset();
                }
            }
        }
    }

    public SkillTypes UseItem(int index, MpData mp) {
        SkillTypes result = SkillTypes.NONE;
        if(Utils.IsValidIndex(SkillList, index) && Utils.NotNull(mp) && SkillList[index].Mp <= mp.Current) {
            mp.Add(-SkillList[index].Mp);
            result = SkillList[index].Type;
        }
        return result;
    }

}