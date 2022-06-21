using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillObject : SelectObject {
    [SerializeField] Image Img;
    [SerializeField] TextMeshProUGUI MpTxt;
    public SkillData Skill {get; private set;}

    [SerializeField] GameObject disableObj;
    [SerializeField] List<Image> RelatedImgList;
    Color32 onColor, offColor;

    public void SetData(SkillData skill) {
        Skill = skill;
        if(Skill != null) {
            if(Img != null) {
                Img.sprite = ResourcesUtils.GetSkillImage(Skill.Type);
            }
            SetMp(Skill.Mp);
        }
        onColor = new Color32(255, 255, 255, 255);
        offColor = new Color32(128, 128, 128, 255);
    }

    public void SetMp(int mp) {
        if(MpTxt != null) {
            MpTxt.text = mp.ToString();
        }
    }

    public void Check(int mp) {
        if(Utils.NotNull(Btn, Skill)) {
            bool isAble = Skill.Mp <= mp;

            // Btn.interactable = isAble;

            if(Utils.NotNull(Img)) {
                Img.color = isAble ? onColor : offColor;
            }

            if(Utils.NotNull(MpTxt)) {
                MpTxt.color = isAble ? onColor : offColor;
            }

            for(int i = 0; i < RelatedImgList.Count; i++) {
                RelatedImgList[i].color = isAble ? onColor : offColor;
            }

            /*
            if(Utils.NotNull(disableObj)) {
                disableObj.SetActive(!isAble);
            }*/
        }
    }

    public void Use() {
        if(Anim != null) {
            Anim.SetTrigger("Use");
        }
    }
}
