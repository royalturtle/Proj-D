using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

[DataContract]
public class GimicGetAll : GimicBase {
    [DataMember] public int EyeCount {get; private set;}
    [DataMember] public List<int> EyesList {get; private set;}
    [DataMember] public List<bool> SuccessList {get; private set;}
    [DataMember] public int SuccessGoal {get; private set;}
    public int SuccessCount {
        get {
            int result = 0;
            if(SuccessList != null) {
                for(int i = 0; i < SuccessList.Count; i++) {
                    if(SuccessList[i]) {
                        result++;
                    }
                }
            }
            return result;
        }
    }
    int RemainSuccess {
        get {
            return SuccessGoal - SuccessCount;
        }
    }

    [DataMember] List<int> CheckList;
    
    public GimicGetAll(int opportunity, int eyeCount, int successGoal) : base(type:GimicTypes.GET_ALL, opportunity:opportunity) {
        EyeCount = eyeCount;
        SuccessGoal = successGoal;
        InitList();
    }

    protected override void ResetAfter() {
        InitList();
    }

    void InitList() {
        if(CheckList == null) CheckList = new List<int>();
        else CheckList.Clear();

        if(EyesList == null) EyesList = new List<int>();
        else EyesList.Clear();

        List<int> tmpList = Utils.ShuffleList(new List<int>{1, 2, 3, 4, 5, 6});
        for(int i = 0; i < EyeCount && i < tmpList.Count; i++) {
            EyesList.Add(tmpList[i]);
        }

        if(SuccessList == null) {
            SuccessList = new List<bool>();
        }

        while(SuccessList.Count > EyesList.Count) {
            SuccessList.RemoveAt(0);
        }

        for(int i = 0; i < SuccessList.Count; i++) {
            SuccessList[i] = false;
        }

        while(SuccessList.Count < EyesList.Count) {
            SuccessList.Add(false);
        }
    }

	protected override void CheckResult() {
        int result = Recent;
        for(int i = 0; i < EyesList.Count; i++) {
            if(result == EyesList[i] && !SuccessList[i]) {
                SuccessList[i] = true;
                CheckList.Add(i);
                break;
            }
        }
        
        int remain = RemainSuccess;
        IsSuccess = remain <= 0;
        IsFailed = IsOpportunityFinished && !IsSuccess && (remain > RemainOpportunity);
    }

	protected override void UpdateResultAfter() {
    }

    public override void ReadyView(DiceResultListObject dice, List<GimicTextObject> txtList, DiceEyesCollection collection, System.Action<int> offTextFunc, GimicCheckObject check) {
        if(Utils.NotNull(dice, txtList, collection, offTextFunc)) {
            dice.Ready(actives:EyeCount, xPos1:0.0f, xPos2:1.0f);
                        
            for(int i = 0; i < EyesList.Count; i++) {
                dice.DiceList[i].Reset(collection.GetGray(EyesList[i]));
            }

            offTextFunc(0);

            if(check) {
                check.Ready();
            }
        }
    }

	public override void UpdateView(List<DiceResultEyeObject> objList, List<GimicTextObject> txtList,DiceEyesCollection collection, GimicCheckObject check) { 
        if(Utils.NotNull(objList, collection)) {
            for(int i = 0; i < CheckList.Count; i++) {
                objList[CheckList[i]].SetData(collection.GetGreen(EyesList[CheckList[i]]));
            }
		}
        CheckList.Clear();
    }
}
