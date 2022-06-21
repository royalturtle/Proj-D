using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

// https://www.youtube.com/watch?v=XJJl19N2KFM&ab_channel=CodeMonkey
public class CutoutMaskUI : Image {
    public override Material materialForRendering {
        get {
            Material material = new Material(base.materialForRendering);
            material.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
            return material;
        }
    }
}
