using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[DataContract]
public class SkillData {
    [DataMember] public SkillTypes Type { get; private set; }
    [DataMember] public int Mp { get; private set; } 
    [DataMember] public bool ReuseAble {get; private set;}
    [DataMember] public int UseCount {get; private set;}
    public bool IsUseAble {get{return !ReuseAble && UseCount > 0;}}

    public SkillData(SkillTypes type, int mp, bool reuseAble, int useCount = 0) {
        Type = type;
        Mp = mp;
        ReuseAble = reuseAble;
        UseCount = useCount;
    }

    public void Reset() {
        UseCount = 0;
    }

    public string Name {
        get {
            return 
                (Type == SkillTypes.NONE) ? 
                "" : 
                LanguageSingleton.Instance.GetSkillName(Type);
        }
    }

    public string Description {
        get {
            return
                (Type == SkillTypes.NONE) ? 
                "" : 
                LanguageSingleton.Instance.GetSkillDescription(Type);
        }
    }

    public void AddUseCount(int value = 1) {
        UseCount += value;
    }
}