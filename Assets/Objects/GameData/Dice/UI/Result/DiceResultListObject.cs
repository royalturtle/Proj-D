using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceResultListObject : MonoBehaviour {
    [SerializeField] RectTransform rectTransform;
    [field:SerializeField] public List<DiceResultEyeObject> DiceList {get; private set;}
    public int ActiveCount {get; private set;}

    public void Ready(int actives, float xPos1, float xPos2) {
        ActiveCount = actives;
        if(DiceList != null) {
            for(int i = 0; i < DiceList.Count; i++) {
                DiceList[i].gameObject.SetActive(i < ActiveCount);
            }
        }

        if(rectTransform != null) {
            rectTransform.anchorMin = new Vector2(xPos1, 0);
            rectTransform.anchorMax = new Vector2(xPos2, 1);
        }

        gameObject.SetActive(true);
    }

    public void Reset(Sprite sprite) {
        for(int i = 0; i < DiceList.Count; i++) {
            DiceList[i].Reset(sprite);
        }
    }


}
