using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceSingleObject : SelectObject {
    Animator animator;
    [SerializeField] List<Sprite> SpriteList;
    [SerializeField] List<Image> ImgList;

    void Awake() {
        animator = GetComponent<Animator>();
    }

    public void SetData(DiceData dice, int additional=0) {
        if(Utils.NotNull(ImgList, dice, dice.Eyes)) {
            int i = 0;
            for(; i < ImgList.Count && i < dice.Eyes.Length; i++) {
                int eye = DiceManager.GetValue(dice.Eyes[i], additional);
                ImgList[i].sprite = Utils.IsValidIndex(SpriteList, eye) ? SpriteList[eye] : null;
            }
            for(; i < ImgList.Count; i++) {
                ImgList[i].sprite = null;
            }
        }
    }

    public void ChangeAnimation(bool value) {
        if(animator != null) {
            animator.SetTrigger(value ? "ChangeTurnOn" : "ChangeTurnOff");
        }
    }
}
