using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MpObject : MonoBehaviour {
    [SerializeField] List<MpGuageObject> _mpObjList;
    [SerializeField] TextMeshProUGUI Txt;

    int Current;

    public void Ready(int count) {
        Current = count;
        for(int i = 0; i < Current; i++) {
            _mpObjList[i].SetData(true);
        }

        UpdateText();
    }

    public void Add(int count) {
        for(int i = Current; i < count && i < _mpObjList.Count; i++) {
            _mpObjList[i].Turn(true);
        }
        Current = count;
        UpdateText();
    }

    public void Subtract(int count) {
        for(int i = count; i < Current && i < _mpObjList.Count; i++) {
            _mpObjList[i].Turn(false);
        }
        Current = count;
        UpdateText();
    }

    void UpdateText() {
        if(Txt != null) {
            Txt.text = Current.ToString();
        }
    }
}
