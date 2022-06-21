using UnityEngine;
using TMPro;

public class HpObject : MonoBehaviour {
    [SerializeField] protected TextMeshProUGUI Txt;
    protected int Hp;

    public void SetData(int hp) {
        Hp = hp;
        if(Txt != null) {
            Txt.text = hp.ToString();
        }
    }

    public void SetData(HpData hp) {
        if(Utils.NotNull(hp)) {
            SetData(hp.HpCurrent);
        }
    }

    public void Ready(HpData hp) {
        SetData(hp);
    }

    public void GetDamage(HpData hp) {
        SetData(hp);
    }
}
