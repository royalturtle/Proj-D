using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[DataContract]
public class HpData {
    [DataMember] public int HpMax {get; private set;}
    [DataMember] public int HpCurrent {get; private set;}
    public bool IsDeath {get {return HpCurrent <= 0;}}

    public HpData(int hpMax, int hpCurrent = -1) {
        HpMax = hpMax;
        HpCurrent = (hpCurrent < 0 || hpCurrent > HpMax) ? HpMax : hpCurrent;
    }

    public void SetHpCurrent(int value) {
        HpCurrent = value < 0 ? 0 : (value > HpMax ? HpMax : value);
    }

    public void AddHpCurrent(int value) {
        SetHpCurrent(HpCurrent + value);
    }    
}
