using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingPongUpdownObject : MonoBehaviour {
    [SerializeField] float Distance = 1f, Speed = 1f;
    public bool IsWorking = true;
    float Former = 0.0f;

    void Start() {
        Former = transform.localPosition.y;
    }

    void Update() {
        if(IsWorking) {
            Vector2 formerPosition = transform.localPosition;
            formerPosition.y = Former + Mathf.PingPong(Time.time * Speed, Distance);;
            transform.localPosition = formerPosition;
        }
        
    }
}
