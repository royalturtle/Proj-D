using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[DataContract]
public class DiceData {    
    [DataMember] public int[] Eyes {get; private set;}

    public DiceData(int[] eyes) {
        Eyes = eyes;
    }

    public int RollDice() {
        return Eyes[UnityEngine.Random.Range(0, Eyes.Length)];
    }

    public void PrintData() {
        string result = "";
        for(int i = 0; i < Eyes.Length; i++) {
            result += Eyes[i].ToString() + " ";
        }
        Debug.Log(result);
    }
}
