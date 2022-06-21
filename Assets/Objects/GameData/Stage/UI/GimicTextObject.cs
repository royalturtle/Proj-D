using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GimicTextObject : MonoBehaviour{
    [SerializeField] TextMeshProUGUI Txt;
    [SerializeField] RectTransform rectTransform;

    public void Ready(string text, float xPos1, float xPos2) {
        SetText(text);

        if(rectTransform != null) {
            rectTransform.anchorMin = new Vector2(xPos1, 0);
            rectTransform.anchorMax = new Vector2(xPos2, 1);
        }

        gameObject.SetActive(true);
    }

    public void SetText(string text) {
        if(Txt != null) {
            Txt.text = text;
        }
    }

    public void TurnOff() {
        gameObject.SetActive(false);
    }

}
