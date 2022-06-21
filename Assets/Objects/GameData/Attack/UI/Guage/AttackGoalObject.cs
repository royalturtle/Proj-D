using UnityEngine;
using UnityEngine.UI;

public class AttackGoalObject : MonoBehaviour {
    [SerializeField] Image FillImg;
    public bool IsEnable {get; private set;}
    public float Start {get; private set;}
    public float End {get; private set;}

    public void SetData(bool isEnable, float start = 0.0f, float end = 0.0f) {
        IsEnable = isEnable;
        Start = start;
        End = end;

        gameObject.SetActive(IsEnable);
        if(Utils.NotNull(FillImg)) {
            RectTransform rect = GetComponent<RectTransform>();
            if(rect != null) {
                rect.rotation = Quaternion.Euler(0.0f, 0.0f, -(Start * 360.0f));
            }
            FillImg.fillAmount = End - Start;
        }
    }

}
