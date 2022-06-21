using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GimicCheckObject : MonoBehaviour {
    [SerializeField] List<GimicCheckUnitObject> _checkList;
    [SerializeField] RectTransform _frame;
    [SerializeField] Sprite _correctSprite, _wrongSprite, _emptySprite;

    public void Ready(int count = 0) {
        if(count <= 0) {
            gameObject.SetActive(false);
        }
        else {
            if(_frame) {
                float length = count / _checkList.Count;
                length = length < 0.0f ? 0.0f : (length > 1.0f ? 1.0f : length);
                _frame.anchorMax = new Vector2(length, 1.0f);
            }
            
            int i = 0;
            for(i = 0; i < _checkList.Count && i < count; i++) {
                _checkList[i].gameObject.SetActive(true);
            }

            for(; i < _checkList.Count; i++) {
                _checkList[i].gameObject.SetActive(false);
            }
            Empty();

            gameObject.SetActive(true);
        }
    }

    void Empty() {
        for(int i = 0; i < _checkList.Count; i++) {
            _checkList[i].SetImage(_emptySprite, isAnimation:false);
        }
    }

    public void SetResult(bool result, int index, bool isAnimation) {
        if(Utils.IsValidIndex(_checkList, index)) {
            _checkList[index].SetImage(result ? _correctSprite : _wrongSprite, isAnimation:isAnimation);
        }
    }
}
