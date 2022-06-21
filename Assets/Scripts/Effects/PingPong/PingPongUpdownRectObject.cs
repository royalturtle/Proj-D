using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPongUpdownRectObject : MonoBehaviour {
    RectTransform rectTransform;
    [SerializeField] float Distance = 1f, Speed = 1f;
    float Former = 0.0f;

    void Start() {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update() {
        if(rectTransform != null) {
            Former = Mathf.PingPong(Time.time * Speed, Distance);
            Vector2 formerPosition = rectTransform.anchoredPosition;
            formerPosition.y += Former;
            rectTransform.anchoredPosition = formerPosition;
        }
    }
}
