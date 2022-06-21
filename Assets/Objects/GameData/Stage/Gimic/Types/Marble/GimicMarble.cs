using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

[DataContract]
public class GimicMarble : GimicBase {
    public int SumValue {
        get {
            int result = 0;
            for(int i = 0; i < ResultList.Count; i++) {
                result += ResultList[i];
            }
            return result;
        }
    }

    [DataMember] public List<int> CellList {get; private set;}
    [DataMember] public int Position {get; private set;}
    [DataMember] public int Goal {get; private set;}
    [DataMember] public int Sum {get; private set;}
    [DataMember] List<bool> _successList;

    public GimicMarble(
        int opportunity,
        int goal
    ) : base(type:GimicTypes.MARBLE, opportunity:opportunity) {
        Goal = goal;
        Sum = 0;
        Position = 0;
        // CellList = new List<int>{0, 1, 2, 5, 6, 3, 2, 4, 5, 3};
        // CellList = new List<int>{0, 1, 6, 2, 5, 3, 4, 1, 3, 5};
        CellList = new List<int>{0, 3, 6, 1, 5, 2, 4, 1, 3, 5};
        CheckSuccessList();
    }

    void CheckSuccessList() {
        if(_successList != null) {
            _successList.Clear();
        }
        else {
            _successList = new List<bool>();
        }
    }

    protected override void UpdateResultAfter() {
        int recent = Recent;
        _successList.Add(true);
        Position = (Position + recent) % CellList.Count;
        Sum += CellList[Position];
    }

    protected override void CheckResult() {
        bool isFulled = IsOpportunityFinished;
        IsSuccess = Sum >= Goal;
        IsFailed = (isFulled && !IsSuccess) || (Goal - Sum > 6 * RemainOpportunity);
    }

	protected override void ResetAfter() {
        Sum = 0;
        CheckSuccessList();
    }

    public void ReadyViewCustom(GimicMarbleUI ui) {
        if(ui) {
            ui.Ready(
                cellList:CellList,
                position:Position,
                sum:Sum
            );
        }
    }

    public override void ReadyView(DiceResultListObject dice, List<GimicTextObject> txtList, DiceEyesCollection collection, System.Action<int> offTextFunc, GimicCheckObject check) {
        if(Utils.NotNull(dice, txtList, collection, offTextFunc)) {            
            dice.Ready(actives:0, xPos1:0.0f, xPos2:0.0f);
            offTextFunc(0);
        }

        if(check) {
            check.Ready(count:Opportunity);
            for(int i = 0; i < _successList.Count; i++) {
                check.SetResult(result:_successList[i], index:i, isAnimation:false);
            }
        }
    }

	public override void UpdateView(List<DiceResultEyeObject> objList, List<GimicTextObject> txtList,DiceEyesCollection collection, GimicCheckObject check) {
        if(check) {
            check.SetResult(result:_successList[_successList.Count - 1], index:_successList.Count - 1, isAnimation:true);
        }
	}

    public override string[] DescriptionValues {
		get {
			return new string[] { (Goal).ToString()};
		}
	}

}
