using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GimicComingSingleObject : MonoBehaviour {
    [SerializeField] List<Image> DiceList;
    [SerializeField] RectTransform CharacterCell;
    [SerializeField] List<RectTransform> CellList;
    public int Length {get {return CellList == null ? 0 : CellList.Count; }}
    
    public void Ready(List<int> eyes, DiceEyesCollection collection) {
        if(Utils.NotNull(eyes, DiceList, collection)) {
            int i = 0;
            for(i = 0; i < DiceList.Count && i < eyes.Count; i++) {
                DiceList[i].sprite = collection.GetWhite(eyes[i]);
                DiceList[i].gameObject.SetActive(true);
            }

            for(; i < DiceList.Count; i++) {
                DiceList[i].gameObject.SetActive(false);
            }
        }
    }

    public void CreateEnemy(int index) {
        
    }
    public void MoveEnemy(int from, int to) {

    }
}
