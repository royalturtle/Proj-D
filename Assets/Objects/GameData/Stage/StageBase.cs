using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[DataContract]
[KnownType(typeof(Stage1))]
[KnownType(typeof(Stage2))]
[KnownType(typeof(Stage3))]
[KnownType(typeof(Stage4))]
[KnownType(typeof(Stage5))]
[KnownType(typeof(Stage6))]
public class StageBase {
    [DataMember] public string Name {get; protected set;}
    [DataMember] public HpData Hp {get; protected set;}

    [DataMember] List<GimicBase> GimicList;
    public GimicBase CurrentGimic {
        get {
            return GimicList == null || GimicList.Count <= 0 || Sequence == null || Sequence.Count <= 0 ? null : GimicList[Sequence[0]];
        }
    }

    [DataMember] public List<int> Sequence {get; protected set;}
    [DataMember] public bool IsShuffle {get; protected set;}
    public bool IsClear {
        get {
            return Sequence == null || Sequence.Count <= 0;
        }
    }

    [DataMember] public List<float> AttackTime {get; protected set;}

    public StageBase(string name, int hpMax, List<GimicBase> gimics, List<float> attackTime, bool isShuffle=false) {
        Name = name;
        Hp = new HpData(hpMax:hpMax);
        IsShuffle = isShuffle;
        GimicList = (gimics == null) ? new List<GimicBase>() : gimics;
        AttackTime = attackTime;
        if(Utils.NotNull(AttackTime)) {
            AttackTime = new List<float>{0.1f, 0.1f, 0.1f};
        }
        SetSequence();
    }

    void SetSequence() {
        Sequence = new List<int>();
        for(int i = 0; i < GimicList.Count; i++) {
            Sequence.Add(i);
        }

        if(IsShuffle) {
            Sequence = Utils.ShuffleList(Sequence);
        }
    }

    void ResetCurrentGimic() {
        if(Utils.NotNull(CurrentGimic)) {
            CurrentGimic.Reset();
        }
    }

    public void UpdateResult(int result) {
        if(CurrentGimic != null) {
            CurrentGimic.UpdateResult(result);
        }
    }

    public void Failed() {
        ResetCurrentGimic();
        if(IsShuffle && Sequence != null) {
            Sequence = Utils.ShuffleList(Sequence);
        }
    }

    public void Success() {
        ResetCurrentGimic();
        if(Sequence != null) {
            if(Sequence.Count > 0) {
                Sequence.RemoveAt(0);
            }
            if(Sequence.Count <= 0) {
                SetSequence();
            }

        }
    }


}
