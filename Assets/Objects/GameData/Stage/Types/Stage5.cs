using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

[DataContract]
public class Stage5 : StageBase {
    public Stage5() : base(
        name: "Sinner",
        hpMax: 8,
        gimics : new List<GimicBase> {
            new GimicNo3(opportunity:4, successGoal:3, banCount:3)
        },
        attackTime:new List<float> {0.9f, 0.8f, 0.7f},
        isShuffle:true
    ) {

    }
}
