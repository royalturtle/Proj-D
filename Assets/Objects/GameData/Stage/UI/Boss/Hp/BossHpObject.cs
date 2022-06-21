using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHpObject : MonoBehaviour {
    [SerializeField] TextMeshProUGUI NameTxt, HpTxt;
    [SerializeField] Image GuageImg;
    int HpMax, HpCurrent;
    float HpPerc { get { return (1.0f * HpCurrent) / (1.0f * HpMax);}}

    public void Ready(string name, int hpMax, int hpCurrent = -1) {
        if(NameTxt != null) {
            NameTxt.text = name;
        }
        HpMax = hpMax;
        SetHp(hpCurrent < 0 ? HpMax : hpCurrent);
    }

    public void Open() {
        gameObject.SetActive(true);
    }

    public void GetDamage(int hp) {        
        SetHp(hp < 0 ? 0 : (hp > HpMax ? HpMax : hp));
    }

    void SetHp(int value) {
        Debug.Log("Damage " + HpPerc.ToString());
        HpCurrent = value;
        if(GuageImg != null) {
            GuageImg.fillAmount = HpPerc;
        }
        if(HpTxt != null) {
            HpTxt.text = HpCurrent.ToString();
        }
    }

    public void Close() {

    }
}
