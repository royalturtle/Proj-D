using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

[DataContract]
public class Stage6 : StageBase {
    public Stage6() : base(
        name: "Devil",
        hpMax: 8,
        gimics : new List<GimicBase> {
            new GimicMarble(opportunity:4, goal:15)
        },
        attackTime:new List<float> {0.9f, 0.8f, 0.7f},
        isShuffle:true
    ) {

    }
}
