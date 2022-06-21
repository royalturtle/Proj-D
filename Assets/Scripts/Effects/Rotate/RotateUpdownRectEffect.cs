using UnityEngine;
using UnityEngine.UI;

public class RotateUpdownRectEffect : EffectBase {
    RectTransform rectTransform;
    float Speed = 1f, Former = 0.0f;

    public RotateUpdownRectEffect(GameObject obj, float speed) : base(obj) {
        rectTransform = obj.GetComponent<RectTransform>();
        Speed = speed;
    }

    public override void Work() {
        Former += Time.deltaTime * Speed;
        Former = Mathf.Repeat(Former, 360.0f);
        // Vector3 former = rectTransform.localRotation;
        // former.x = Former;
        // rectTransform.localRotation = former;
        rectTransform.localRotation = Quaternion.AngleAxis(Former, Vector3.left);
    }
}