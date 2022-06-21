using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageTextObject : MonoBehaviour {
    [SerializeField] TextMeshProUGUI Txt;

    public void SetData(int stage)  {
        if(Txt != null) {
            Txt.text = "STAGE " + stage.ToString();
        }
    }
}
