using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillSingleDescriptionObject : MonoBehaviour {
    [SerializeField] GameObject Obj;
    [SerializeField] Image IconImg;
    [SerializeField] TextMeshProUGUI DescriptionTxt, ManaTxt;

    public void SetData(SkillData skill) {
        if(Utils.NotNull(Obj)) {
            Obj.SetActive(skill != null);
            if(Utils.NotNull(skill)) {
                if(Utils.NotNull(IconImg)) {
                    IconImg.sprite = ResourcesUtils.GetSkillImage(skill.Type);
                }
                if(Utils.NotNull(DescriptionTxt)) {
                    DescriptionTxt.text = skill.Description;
                }
                if(Utils.NotNull(ManaTxt)) {
                    ManaTxt.text = skill.Mp.ToString();
                }
            }
        }
    }
}
