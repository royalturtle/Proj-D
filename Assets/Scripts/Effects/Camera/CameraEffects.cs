using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour {
    Vector3 originPos;
    
    void Start() {
        FixCurrentPosition();
    }

    public void FixCurrentPosition() {
        originPos = transform.localPosition;
    }

    // https://zprooo915.tistory.com/27
    public IEnumerator Shake(float amount, float duration, float delay = 0.0f) {
        float timer = 0;
        while(timer <= delay) {
            timer += Time.deltaTime;
        }
        timer = 0;
        while(timer <= duration) {
            transform.localPosition = (Vector3)Random.insideUnitCircle * amount + originPos;

            timer += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originPos;
    }
}
