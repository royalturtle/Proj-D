using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[DataContract]
[KnownType(typeof(GimicOdd))]
[KnownType(typeof(GimicGetAll))]
[KnownType(typeof(GimicEven))]
[KnownType(typeof(GimicComing))]
[KnownType(typeof(GimicBetween))]
[KnownType(typeof(GimicNo3))]
[KnownType(typeof(GimicMore))]
[KnownType(typeof(GimicLess))]
[KnownType(typeof(GimicNonDuplicate))]
[KnownType(typeof(GimicMarble))]
public class GimicBase{
    [DataMember] public GimicTypes Type {get; private set;}
	[DataMember] public bool IsSuccess {get; protected set;} 
	[DataMember] public bool IsFailed {get; protected set;}
	[DataMember] public int Opportunity {get; protected set;}
	[DataMember] public int Tried {get; protected set;}
	public int RemainOpportunity { get { return Opportunity - Tried; } }
	protected bool IsOpportunityFinished { get { return RemainOpportunity < 1; }}

	[DataMember] public List<int> ResultList {get; protected set;}
	public int Recent {
		get {
			return ResultList == null || ResultList.Count < 1 ? GameConstants.NULL_INT : ResultList[ResultList.Count - 1];
		}
	}

	public GimicBase(GimicTypes type, int opportunity) {
		Type = type;
		Opportunity = opportunity;
		IsSuccess = false;
		IsFailed = false;
		Tried = 0;
		ResultList = new List<int>();
	}

	public void Reset() {
		IsSuccess = false;
		IsFailed = false;
		Tried = 0;
		ResultList.Clear();
		ResetAfter();
	}
	protected virtual void ResetAfter() {}

	public void UpdateResult(int value) {
		ResultList.Add(value);
		Tried++;
		UpdateResultAfter();
		CheckResult();
	}
	protected virtual void CheckResult() {}
	protected virtual void UpdateResultAfter() {}

	#region Result
	protected bool IsRemainSuccessLessFailed(int total, int count) { 
		return !IsSuccess && ((total - count) > RemainOpportunity);
	}
	#endregion
	
	#region UI
	public virtual void ReadyView(DiceResultListObject dice, List<GimicTextObject> txtList, DiceEyesCollection collection,  System.Action<int> offTextFunc, GimicCheckObject check) {}
	public virtual void UpdateView(List<DiceResultEyeObject> objList, List<GimicTextObject> txtList,DiceEyesCollection collection, GimicCheckObject check) { }
	#endregion

	#region Description
	public virtual string[] DescriptionValues {
		get {
			return new string[] {};
		}
	}
	public string Description {
		get {
			string result = LanguageSingleton.Instance.Data[LocalizationTypes.GIMIC_DESCRIPTION].ContainsKey((int)Type) ?
				LanguageSingleton.Instance.Data[LocalizationTypes.GIMIC_DESCRIPTION][(int)Type] : "";
			return System.String.Format(result, DescriptionValues);
		}
	}
	#endregion
}
