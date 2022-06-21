using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[DataContract]
public class MpData {
    [DataMember] public int Max {get; private set;}
    [DataMember] public int Current {get; private set;}

    public MpData(int max, int current = 0) {
        Max = max;
        SetCurrent(current);
    }

    public void SetCurrent(int value) {
        Current = value < 0 ? 0 : (value > Max ? Max : value);
    }

    public void Add(int value = 1) {
        SetCurrent(Current + value);
    }

    public bool Use(int value = 1) {
        bool result = false;
        if(Current >= value) {
            Current -= value;
            result = true;
        }
        return result;
    }
}
