using UnityEngine;
using UnityEngine.UI;

public class PingPongUpdownRectEffect : EffectBase {
    RectTransform rectTransform;
    float Distance = 1f, Speed = 1f, Former = 0.0f;

    public PingPongUpdownRectEffect(
        GameObject obj,
        float distance,
        float speed) : base(obj) {
        rectTransform = obj.GetComponent<RectTransform>();
        Distance = distance;
        Speed = speed;
    }

    public override void Work() {
        Former = Mathf.PingPong(Time.time * Speed, Distance);
        Vector2 formerPosition = rectTransform.anchoredPosition;
        formerPosition.y += Former;
        rectTransform.anchoredPosition = formerPosition;
    }
}
