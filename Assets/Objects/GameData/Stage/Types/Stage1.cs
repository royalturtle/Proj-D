using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

[DataContract]
public class Stage1 : StageBase {
    public Stage1() : base(
        name: "Monster",
        hpMax: 8,
        gimics : new List<GimicBase> {
            new GimicEven(opportunity:3, successCount:2),
            new GimicOdd(opportunity:3, successCount:2)
        },
        attackTime:new List<float> {1.2f, 1.0f, 0.8f},
        isShuffle:true
    ) {

    }
}
