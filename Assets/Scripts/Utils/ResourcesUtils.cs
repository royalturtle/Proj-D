using UnityEngine;
using UnityEngine.UI;

public static class ResourcesUtils {
    public static Sprite GetSkillImage(SkillTypes type) {
        // Time Check Needed
        return Resources.Load<Sprite>("Images/Skill/" + ((int)type).ToString());
    }

}
