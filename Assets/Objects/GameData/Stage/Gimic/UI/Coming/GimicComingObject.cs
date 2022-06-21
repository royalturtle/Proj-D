using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimicComingObject : MonoBehaviour {
    [SerializeField] List<GimicComingSingleObject> DiceList;
    
    public int Height {get{return DiceList == null ? 0 : DiceList.Count;}}
    public int Width  {get{return DiceList != null && DiceList[0] != null ? DiceList[0].Length : 0;}}

    
    
}
