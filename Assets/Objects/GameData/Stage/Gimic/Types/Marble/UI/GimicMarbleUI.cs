using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GimicMarbleUI : MonoBehaviour {
    [SerializeField] RectTransform _playerObj;
    [SerializeField] List<GimicMarbleCell> _cellList;
    [SerializeField] TextMeshProUGUI _sumText;
    int formerPosition;

    public void Ready(List<int> cellList, int position, int sum) {
        for(int i = 0; i < _cellList.Count && i < cellList.Count; i++) {
            _cellList[i].SetData(cellList[i]);
        }
        formerPosition = position;

        SetPosition(position);
        SetSum(sum);
    }

    public IEnumerator UpdateData(GimicMarble gimic, bool isAnimation=false) {
        if(gimic != null) {
            if(!isAnimation) {
                SetPosition(gimic.Position);
            }
            else {
                yield return StartCoroutine(MovePosition(gimic.Position));
            }
            formerPosition = gimic.Position;
        
            SetSum(gimic.Sum);
        }
        
        yield return null;
    }

    public IEnumerator UpdateData(int position, int sum, bool isAnimation=false) {
        if(!isAnimation) {
            SetPosition(position);
        }
        else {
            yield return StartCoroutine(MovePosition(position));
        }
        formerPosition = position;
        
        SetSum(sum);
        yield return null;
    }

    public void SetPosition(int position) {
        if(Utils.IsValidIndex(_cellList, position)) {
            _cellList[position].SetPlayer(_playerObj);
        }
    }

    IEnumerator MovePosition(int position) {
        Debug.Log(formerPosition);
        if(0 <= position && position < _cellList.Count) {
            while(formerPosition != position) {
                if(formerPosition + 1 >= _cellList.Count) {
                    formerPosition = -1;
                }
                SetPosition(++formerPosition);
                yield return StartCoroutine(WaitForRealSeconds(0.25f));
            }
        }
        yield return null;
    }

    public void SetSum(int value) {
        if(_sumText) {
            _sumText.text = value.ToString();
        }
    }

    IEnumerator WaitForRealSeconds (float seconds) {
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup-startTime < seconds) {
            yield return null;
        }
    }
}
