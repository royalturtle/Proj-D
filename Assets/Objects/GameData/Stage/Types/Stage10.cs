using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

[DataContract]
public class Stage10 : StageBase {
    public Stage10() : base(
        name: "Stage10",
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
