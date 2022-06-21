using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorDictionary {
    public Color32 Red {get; private set;}
    public Color32 Blue {get; private set;}
    public Color32 Green {get; private set;}
    public Color32 Orange {get; private set;}

    public ColorDictionary() {
        Red = new Color32(255, 0, 0, 255);
        Green = new Color32(0, 255, 0, 255);
        Blue = new Color32(0, 0, 255, 255);
        Orange = new Color32(254, 134, 18, 255);
    }

    
}
