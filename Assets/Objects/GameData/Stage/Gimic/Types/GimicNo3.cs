using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

[DataContract]
public class GimicNo3 : GimicBase {
    #region Variables
    [DataMember] public List<int> BanList {get; private set;}
    [DataMember] public int BanCount {get; private set;}
    [DataMember] public int SuccessGoal {get; private set;}
    public int SuccessCount {
        get {
            int result = 0;
            for(int i = 0; i < _successList.Count; i++) {
                if(_successList[i]) {
                    result += 1;
                }
            }
            return result;
        }
    }
    public int RemainSuccess {get {return SuccessGoal - SuccessCount;}}
    [DataMember] List<bool> _successList;
    #endregion

    public GimicNo3(int opportunity, int successGoal, int banCount)
    : base(type:GimicTypes.NO3, opportunity:opportunity) {
        BanCount = banCount;
        SuccessGoal = successGoal;
        ResetAfter();
    }

	protected override void ResetAfter() {
        // Create First Ban List
        List<int> tmpList = new List<int>{1, 2, 3, 4, 5, 6};
        tmpList = Utils.ShuffleList(tmpList);
        
        BanList = new List<int>();
        for(int i = 0; i < BanCount; i++) {
            BanList.Add(tmpList[i]);
        }
        
        if(_successList != null) {
            _successList.Clear();
        }
        else {
            _successList = new List<bool>();
        }
    }

    protected override void CheckResult() {
        if(Utils.NotNull(BanList, ResultList)) {
            IsSuccess = SuccessCount >= SuccessGoal;
            IsFailed = !IsSuccess && (RemainOpportunity < RemainSuccess);
        }
    }

	protected override void UpdateResultAfter() {
        bool result = IsRecentSuccess();
        _successList.Add(result);
        // Success
        if(result) {
            for(int i = 1; i < BanCount; i++) {
                BanList[i - 1] = BanList[i];
            }
            BanList[BanCount - 1] = Recent;
        }
        // Fail
        else {
            if(ResultList != null && ResultList.Count > 0) {
                ResultList.RemoveAt(ResultList.Count - 1);
            }
        }
    }

    bool IsRecentSuccess() {
        bool result = true;
        int recent = Recent;
        for(int i = 0; i < BanList.Count; i++) {
            if(recent == BanList[i]) {
                result = false;
                break;
            }
        }
        return result;
    }

    public override void ReadyView(DiceResultListObject dice, List<GimicTextObject> txtList, DiceEyesCollection collection, System.Action<int> offTextFunc, GimicCheckObject check) {
        if(Utils.NotNull(dice, txtList, collection, offTextFunc)) {
            dice.Ready(actives:BanCount, xPos1:0.0f, xPos2:1.0f);
            
            for(int i = 0; i < BanCount; i++) {
                dice.DiceList[i].Reset(collection.GetWhite(BanList[i]));
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
            for(int i = 0; i < BanCount && i < objList.Count; i++) {
                objList[i].SetData(collection.GetWhite(BanList[i]));
            }
            
		}
        
        if(check) {
            check.SetResult(result:_successList[_successList.Count - 1], index:_successList.Count - 1, isAnimation:true);
        }
	}

    public override string[] DescriptionValues {
		get {
			return new string[] { SuccessGoal.ToString()};
		}
	}

}
