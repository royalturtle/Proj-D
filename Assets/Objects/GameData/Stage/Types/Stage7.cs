using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

[DataContract]
public class Stage7 : StageBase {
    public Stage7() : base(
        name: "Stage7",
        hpMax: 2,
        gimics : new List<GimicBase> {
            new GimicBetween(opportunity:3, minValue:8, maxValue:12),
            new GimicEven(opportunity:3, successCount:2),
            new GimicOdd(opportunity:3, successCount:2)
        },
        attackTime:new List<float> {1.2f, 1.0f, 0.8f},
        isShuffle:true
    ) {

    }
}
