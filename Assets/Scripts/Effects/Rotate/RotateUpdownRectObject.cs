using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateUpdownRectObject : MonoBehaviour {
    List<EffectBase> EffectList;

    [SerializeField] float Speed;
    
    void Awake(){
        EffectList = new List<EffectBase>{
            new RotateUpdownRectEffect(obj:gameObject, speed:Speed)
        };
    }
    
    void Update() {
        foreach(EffectBase effect in EffectList) {effect.Work();}
    }
}
