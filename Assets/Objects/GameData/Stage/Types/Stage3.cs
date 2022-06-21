using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

[DataContract]
public class Stage3 : StageBase {
    public Stage3() : base(
        name: "Magician",
        hpMax: 8,
        gimics : new List<GimicBase> {
            new GimicBetween(opportunity:3, minValue:9, maxValue:12)
        },
        attackTime:new List<float> {1.0f, 0.9f, 0.8f},
        isShuffle:true
    ) {

    }
}
