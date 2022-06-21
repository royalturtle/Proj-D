using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

[DataContract]
public class GimicEven : GimicBase {
    #region Variables
    [DataMember] public int SuccessCount {get; private set;}
    #endregion

    public GimicEven(
        int opportunity,
        int successCount
    ) : base(type:GimicTypes.EVEN, opportunity:opportunity) {
        SuccessCount = successCount;
    }
    
    protected override void CheckResult() {
        int count = 0;
        for(int i = 0; i < ResultList.Count; i++) {
            if(ResultList[i] % 2 == 0) {
                count++;
            }
        }
        IsSuccess = count >= SuccessCount;
        IsFailed = IsRemainSuccessLessFailed(SuccessCount, count);
    }

    public override void ReadyView(DiceResultListObject dice, List<GimicTextObject> txtList, DiceEyesCollection collection, System.Action<int> offTextFunc, GimicCheckObject check) {
        if(Utils.NotNull(dice, txtList, collection, offTextFunc)) {            
            dice.Ready(actives:Opportunity, xPos1:0.0f, xPos2:1.0f);
                        
            for(int i = 0; i < Opportunity; i++) {
                dice.DiceList[i].Reset(collection.Question);
            }

            for(int i = 0; i < ResultList.Count; i++) {
                if(Utils.IsValidIndex(dice.DiceList, i)) {
                    dice.DiceList[i].Reset(GetSprite(collection, ResultList[i]));
                }
            }

            offTextFunc(0);

            if(check) {
                check.Ready();
            }
        }
    }

	public override void UpdateView(List<DiceResultEyeObject> objList, List<GimicTextObject> txtList,DiceEyesCollection collection, GimicCheckObject check) {
		if(Utils.NotNull(objList, collection)) {
            int index = ResultList.Count - 1;
            if(Utils.IsValidIndex(objList, index)) {
                objList[index].SetData(GetSprite(collection, ResultList[index]));
            }
		}
	}

    Sprite GetSprite(DiceEyesCollection collection, int value) {
        Sprite result = null;
        if(collection != null) {
            result = value % 2 == 0 ? collection.GetGreen(value) : collection.GetRed(value);
        }
        return result;
    }

    public override string[] DescriptionValues {
		get {
			return new string[] {SuccessCount.ToString(), Opportunity.ToString()};
		}
	}
}
