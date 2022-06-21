using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCollection : MonoBehaviour {
    [SerializeField] List<RuntimeAnimatorController> BossList;

    public RuntimeAnimatorController GetController(int index) {
        if(Utils.IsValidIndex(BossList, index)) {
            return BossList[index];
        }
        else {
            return null;
        }
    }
}
