using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[DataContract]
public class DiceManager {
    const int EyeMax = 6;
    [DataMember] public List<DiceData> DiceList {get; private set;}
    [DataMember] public int Selected {get; private set;}
    [DataMember] public int RecentResult {get; private set;}
    [DataMember] public int Additional {get; private set;}
    [DataMember] public int Chance {get; private set;}
    [DataMember] public bool IsChange {get; private set;}

    public bool IsChanceRemain {get{return Chance > 0;}}
    public bool IsValidSelect {
        get {
            return IsValidIndex(Selected);
        }
    }
    public bool IsValidIndex(int index) {
        return DiceList != null && 0 <= index && index < DiceList.Count;
    }

    public DiceManager(int startCount = 3) {
        ResetStatus();

        DiceList = new List<DiceData>();
        for(int i = 0; i < startCount; i++) {
            DiceList.Add(CreateDice());
        }
    }

    public void LoadGame() {
        Selected = GameConstants.NULL_INT;
    }

    DiceData CreateDice() {
        int[] eyes = new int[EyeMax];
        for(int i = 0; i < EyeMax; i++) {
            eyes[i] = UnityEngine.Random.Range(1, EyeMax + 1);
        }
        System.Array.Sort(eyes);
        
        return new DiceData(eyes : eyes);
    }

    void ChangeDice(int index) {
        if(IsValidIndex(index)) {
            DiceList[index] = CreateDice();
        }
    }
    
    public static int GetValue(int origValue, int additional) {
		
        return (mod((origValue + additional - 1), EyeMax)) + 1;
    }
	
	static int mod(int x, int m) {
	    return (x%m + m)%m;
	}

    public void SetSelected(int value) {
        if(IsValidIndex(value)) {
            Selected = value;
        }
    }

    public void ClearSelected() {
        Selected = GameConstants.NULL_INT;
    }

    public void ResetStatus() {
    // void ResetStatus() {
        Debug.Log("Dice Reset");
        Selected = GameConstants.NULL_INT;
        RecentResult = GameConstants.NULL_INT;
        Additional = 0;
        Chance = 1;
        IsChange = true;
    }

    public bool RollDice() {
        bool result = false;
        if(IsValidSelect) {
            RecentResult = GetValue(DiceList[Selected].RollDice(), Additional);
            result = true;
            Chance--;
            // ChangeDice(Selected);
        }
        return result;
    }

    public bool ChangeSelected() {
        bool result = false;
        if(IsValidSelect) {
            ChangeDice(Selected);
            result = true;
        }
        return result;
    }

    public void ChangeAll() {
        if(DiceList != null) {
            for(int i = 0; i < DiceList.Count; i++) {
                ChangeDice(i);
            }
        }
    }

    public void AddAdditional(int value) {
        Additional += value;
    }

    public void AddChance(int value) {
        Chance += value;
    }

    public void SetNotChange() {
        IsChange = false;
    }

    public List<int> GetSelectedRandomList(int count) {
        List<int> result = new List<int>();
        if(IsValidSelect) {
            for(int i = 0; i < count; i++) {
                result.Add(GetValue(DiceList[Selected].RollDice(), Additional));
            }
        }
        return result;
    }

    public void ResetChance() {
        Chance = 0;
    }

    public void PrintData() {
        for(int i = 0; i < DiceList.Count; i++) {
            DiceList[i].PrintData();
        }
    }
}
