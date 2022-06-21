using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GimicMarbleCell : MonoBehaviour {
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] RectTransform _playerCell;

    public void SetData(int value) {
        if(_text) {
            _text.text = value.ToString();
        }
    }

    public void SetPlayer(RectTransform player) {
        if(Utils.NotNull(player, _playerCell)) {
            player.parent = _playerCell;
            player.anchorMin = new Vector2(0, 0);
            player.anchorMax = new Vector2(1, 1);
            player.offsetMin = Vector2.zero;
            player.offsetMax = Vector2.zero;
        }
    }
}
