using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

[DataContract]
public class GimicMore : GimicBase {
    [DataMember] public int Goal {get; private set;}
    public int SumValue {
        get {
            int result = 0;
            for(int i = 0; i < ResultList.Count; i++) {
                result += ResultList[i];
            }
            return result;
        }
    }

    public GimicMore(
        int opportunity,
        int goal
    ) : base(type:GimicTypes.MORE, opportunity:opportunity) {
        Goal = goal;
    }

    protected override void CheckResult() {
        int sum = SumValue;
        bool isFulled = IsOpportunityFinished;
        IsSuccess = sum >= Goal;
        IsFailed = (isFulled && !IsSuccess) || (Goal - sum > 6 * RemainOpportunity);
    }

    public override void ReadyView(DiceResultListObject dice, List<GimicTextObject> txtList, DiceEyesCollection collection, System.Action<int> offTextFunc, GimicCheckObject check) {
        if(Utils.NotNull(dice, txtList, collection, offTextFunc)) {
            dice.Ready(actives:Opportunity, xPos1:0.0f, xPos2:0.75f);

            for(int i = 0; i < Opportunity; i++) {
                dice.DiceList[i].Reset(collection.Question);
            }

            
            for(int i = 0; i < ResultList.Count; i++) {
                if(Utils.IsValidIndex(dice.DiceList, i)) {
                    dice.DiceList[i].Reset(collection.GetWhite(ResultList[i]));
                }
            }

            if(Utils.IsValidIndex(txtList, 0) && txtList[0] != null) {
                txtList[0].Ready(SumValue.ToString(), xPos1:0.75f, xPos2:1.0f);
            }
            offTextFunc(1);

            if(check) {
                check.Ready();
            }
        }
    }

	public override void UpdateView(List<DiceResultEyeObject> objList, List<GimicTextObject> txtList,DiceEyesCollection collection, GimicCheckObject check) {
		if(Utils.NotNull(objList, txtList, collection)) {
            int index = ResultList.Count - 1;
            if(Utils.IsValidIndex(objList, index)) {
                objList[index].SetData(collection.GetWhite(ResultList[index]));
            }
            if(Utils.NotNull(txtList[0])) {
                txtList[0].SetText(SumValue.ToString());
            }
		}
	}

    
	public override string[] DescriptionValues {
		get {
			return new string[] {Goal.ToString()};
		}
	}

}
