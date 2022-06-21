using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

[DataContract]
public class Stage2 : StageBase {
    public Stage2() : base(
        name: "Warrior",
        hpMax: 8,
        gimics : new List<GimicBase> {
            new GimicMore(opportunity:4, goal:16),
            new GimicLess(opportunity:4, goal:12)
        },
        attackTime:new List<float> {1.2f, 1.0f, 0.8f},
        isShuffle:true
    ) {

    }
}
