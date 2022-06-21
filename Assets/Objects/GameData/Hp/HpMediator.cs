using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpMediator : MonoBehaviour {
    InGameSceneManager GameManager;

    [SerializeField] HpObject hpObj;
    [SerializeField] MpObject mpObj;

    public HpData Hp {get; private set;}
    public MpData Mp {get; private set;}

    public void Ready(InGameSceneManager gameManager, HpData hp, MpData mp) {
        GameManager = gameManager;
        Hp = hp;
        Mp = mp;

        if(Utils.NotNull(Hp, hpObj)) {
            hpObj.Ready(Hp);
        }

        if(Utils.NotNull(Mp, mpObj)) {
            mpObj.Ready(Mp.Current);
        }
    }

    public bool UseMp(int value) {
        bool result = false;
        if(Utils.NotNull(Mp, mpObj)) {
            result = Mp.Use(value);
            if(result) {
                mpObj.Subtract(Mp.Current);
            }
        }
        return result;
    }

    public void AddMp(int value = 1) {
        if(Utils.NotNull(Mp, mpObj)) {
            Mp.Add(value);
            mpObj.Add(Mp.Current);
        }
    }

    public IEnumerator GetDamage(int value = 1) {
        if(Utils.NotNull(GameManager, Hp)) {
            if(Utils.NotNull(Hp)) {
                Hp.AddHpCurrent(-value);
            }

            if(Utils.NotNull(hpObj)) {
                hpObj.GetDamage(Hp);
            }
        }
        yield return StartCoroutine(WaitForRealSeconds(0.5f));
    }

    public bool IsSkillUseable(SkillData skill) {
        bool result = false;
        if(Utils.NotNull(skill, Mp)) {
            result = skill.Mp <= Mp.Current;
        }
        return result;
    }

    public bool IsDeath {
        get {
            bool result = false;

            if(Utils.NotNull(Hp)) {
                result = Hp.IsDeath;
            }

            return result;
        }
    }
    
    IEnumerator WaitForRealSeconds (float seconds) {
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup-startTime < seconds) {
            yield return null;
        }
    }
}
