using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

[DataContract]
public class GimicNonDuplicate : GimicBase {
    #region Variables
    [DataMember] public int SuccessGoal {get; private set;}
    [DataMember] bool _recentResult;
    public int SuccessCount {get{return ResultList == null ? 0 : ResultList.Count;}}
    [DataMember] List<bool> _successList;
    #endregion

    public GimicNonDuplicate(
        int opportunity,
        int successGoal
    ) : base(type:GimicTypes.NON_DUPLICATE, opportunity:opportunity) {
        SuccessGoal = successGoal;
        _recentResult = false;
        _successList = new List<bool>();
    }

	protected override void ResetAfter() {
        if(_successList != null) {
            _successList.Clear();
        }
        else {
            _successList = new List<bool>();
        }
    }
    
    protected override void CheckResult() {
        IsSuccess = SuccessCount >= SuccessGoal;
        IsFailed = !IsSuccess && IsOpportunityFinished;
    }

    protected override void UpdateResultAfter() {
        _recentResult = IsRecentSuccess();
        _successList.Add(_recentResult);
        // Success
        if(!_recentResult) {
            if(ResultList != null && ResultList.Count > 0) {
                ResultList.RemoveAt(ResultList.Count - 1);
            }
        }
    }

    bool IsRecentSuccess() {
        bool result = true;
        int recent = Recent;
        for(int i = 0; i < ResultList.Count - 1; i++) {
            if(ResultList[i] == recent) {
                result = false;
                break;
            }
        }
        return result;
    }

    bool IsValueSuccess(int value) {
        bool result = true;
        for(int i = 0; i < ResultList.Count - 1; i++) {
            if(ResultList[i] == value) {
                result = false;
                break;
            }
        }
        return result;
    }

    public override void ReadyView(DiceResultListObject dice, List<GimicTextObject> txtList, DiceEyesCollection collection, System.Action<int> offTextFunc, GimicCheckObject check) {
        if(Utils.NotNull(dice, txtList, collection, offTextFunc)) {            
            dice.Ready(actives:SuccessGoal, xPos1:0.0f, xPos2:1.0f);
                        
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
                check.Ready(count:Opportunity);
                for(int i = 0; i < _successList.Count; i++) {
                    check.SetResult(result:_successList[i], index:i, isAnimation:false);
                }
            }
        }
    }

	public override void UpdateView(List<DiceResultEyeObject> objList, List<GimicTextObject> txtList,DiceEyesCollection collection, GimicCheckObject check) {
		if(Utils.NotNull(objList, collection)) {
            if(_recentResult) {
                int index = ResultList.Count - 1;
                if(Utils.IsValidIndex(objList, index)) {
                    objList[index].SetData(GetSprite(collection, ResultList[index]));
                }
                _recentResult = false;
            }
            
            if(check) {
                check.SetResult(result:_successList[_successList.Count - 1], index:_successList.Count - 1, isAnimation:true);
            }
		}
	}

	public override string[] DescriptionValues {
		get {
			return new string[] {SuccessGoal.ToString()};
		}
	}

    Sprite GetSprite(DiceEyesCollection collection, int value) {
        Sprite result = null;
        if(collection != null) {
            result = collection.GetWhite(value);
        }
        return result;
    }

}
