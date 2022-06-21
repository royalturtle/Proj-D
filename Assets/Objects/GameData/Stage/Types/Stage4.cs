using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

[DataContract]
public class Stage4 : StageBase {
    public Stage4() : base(
        name: "Guardian",
        hpMax: 8,
        gimics : new List<GimicBase> {
            new GimicNonDuplicate(opportunity:5, successGoal:4),
        },
        attackTime:new List<float> {1.0f, 0.9f, 0.8f},
        isShuffle:true
    ) {

    }
}
