using UnityEngine;
using UnityEngine.UI;

public class GuageObject : MonoBehaviour {
    [SerializeField] Image GaugeImg;
    public float Guage {get{return GaugeImg != null ? GaugeImg.fillAmount : 0.0f;}}

    public void Open(float value=0.0f) {
        gameObject.SetActive(true);
        SetGuage(value);
    }

    public void Close() {
        gameObject.SetActive(false);
    }

    public void SetGuage(float value) {
        if(GaugeImg != null) {
            GaugeImg.fillAmount = value;
        }
    }
}
