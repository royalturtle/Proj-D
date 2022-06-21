using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[DataContract]
public class StageManager {
    [DataMember] List<StageBase> StageList;
    [DataMember] public int StageIndex {get; private set;}
    public StageBase CurrentStage {
        get {
            return Utils.IsValidIndex(StageList, StageIndex) ? StageList[StageIndex] : null;
        }
    }

    public bool IsClear {get{return !Utils.IsValidIndex(StageList, StageIndex);}}

    public StageManager(int index = 0, List<StageBase> stageList = null) {
        StageIndex = index;

        StageList = stageList == null || stageList.Count < 1 ? new List<StageBase> {
            // new Stage4(),
            new Stage1(),
            new Stage2(),
            new Stage3(),
            new Stage4(),            
            new Stage5(),
            new Stage6()
        } : stageList;
    }

    public void ReadyAction(StageMediator mediator) {
        if(mediator != null) {
            
        }
    }

    public void NextStage() {
        StageIndex++;
    }

    public void UpdateResult(int result) {
        if(CurrentStage != null) {
            CurrentStage.UpdateResult(result);
        }
    }

    public void EnemyAttacked() {
        
    }

}
