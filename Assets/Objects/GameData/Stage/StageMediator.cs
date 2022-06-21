
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageMediator : MonoBehaviour {
    InGameSceneManager GameManager;
    StageManager stageManager;

    [SerializeField] GimicObject Obj;
    [SerializeField] BossObject BossObj;
    [SerializeField] BossCollection bossCollection;
    [SerializeField] BossHpObject HpObj;
    [SerializeField] AttackObject AttackObj;
    [SerializeField] StageTextObject StageTxtObj;
    [SerializeField] List<TorchObject> TorchObjList;

    public bool IsClear {
        get {
            return stageManager != null ? stageManager.IsClear : false;
        }
    }
    public bool IsDeath {
        get {
            return Utils.NotNull(stageManager, stageManager.CurrentStage, stageManager.CurrentStage.Hp) ?
                stageManager.CurrentStage.Hp.IsDeath : false;
        }
    }
    public int StageIndex {
        get {
            return stageManager != null ? stageManager.StageIndex : 0;
        }
    }

    AudioSource _sfxAudio;
    [SerializeField] AudioClip _enemyAppearSFX, _enemyDeathSFX, _gimicSetSFX, _damageSFX;
    [SerializeField] List<AudioClip> _attackSFXList;

    void Awake() {
        _sfxAudio = GetComponent<AudioSource>();
    }

    public void Ready(InGameSceneManager gameManager, StageManager stage) {
        List<string> TorchTriggerList = new List<string> {"Red","Blue", "Brown", "Purple", "Green", "Black"};
        for(int i = 0; i < stage.StageIndex; i++) {
            TorchObjList[i].SetTrigger("Empty");
        }

        for(int i = stage.StageIndex; i < TorchTriggerList.Count && i < TorchObjList.Count; i++) {
            TorchObjList[i].SetTrigger(TorchTriggerList[i]);
        }

        GameManager = gameManager;
        stageManager = stage;

        if(Utils.NotNull(AttackObj)) {
            AttackObj.Ready(
                attackAct:Attack,
                finishAct:AttackFinish
            );
        }
    }

    #region Enemy
    public void SetEnemy() {
        if(Utils.NotNull(BossObj, bossCollection)) {
            BossObj.Ready(bossCollection.GetController(stageManager.StageIndex));
        }
        if(Utils.NotNull(HpObj)) {
            HpObj.Ready(
                name:stageManager.CurrentStage.Name,
                hpMax:stageManager.CurrentStage.Hp.HpMax,
                hpCurrent:stageManager.CurrentStage.Hp.HpCurrent);
            HpObj.Open();
        }
        if(Utils.NotNull(StageTxtObj)) {
            StageTxtObj.SetData(stageManager.StageIndex + 1);
        }
    }

    public IEnumerator ReadyEnemy() {
        if(Utils.NotNull(stageManager, stageManager.CurrentStage)) {
            if(Utils.NotNull(BossObj, bossCollection)) {
                BossObj.PortalTurn(true);
                PlaySFX(_enemyAppearSFX);
                if(Utils.NotNull(GameManager, GameManager.Scenario)){
                    GameManager.Scenario.CameraShake(amount:0.2f, duration:1.5f);
                }
                yield return StartCoroutine(WaitForRealSeconds(1.5f));
            }
            SetEnemy();
            if(Utils.NotNull(BossObj)) {
                BossObj.PortalTurn(false);
            }
        }
        yield return StartCoroutine(WaitForRealSeconds(1.5f));
    }
    #endregion

    #region Gimic
    public void OpenGimic() {
        if(Utils.NotNull(Obj, stageManager, stageManager.CurrentStage, stageManager.CurrentStage.CurrentGimic)) {
            Obj.OpenData(stageManager.CurrentStage.CurrentGimic);
        }
    }

    public void UpdateResult(int result) {
        if(Utils.NotNull(stageManager, stageManager.CurrentStage)) {
            stageManager.CurrentStage.UpdateResult(result);
        }
    }
    
    public IEnumerator UpdateGimic() {
        if(Utils.NotNull(GameManager)) {
            if(Utils.NotNull(stageManager, stageManager.CurrentStage, stageManager.CurrentStage.CurrentGimic)) {
                yield return StartCoroutine(WaitForRealSeconds(0.5f));

                // Update Gimic Status
                StageBase stage = stageManager.CurrentStage;
                GimicBase gimic = stage.CurrentGimic;

                if(Utils.NotNull(Obj)) {
                    yield return StartCoroutine(Obj.UpdateData(gimic));
                }
                PlaySFX(_gimicSetSFX);

                yield return StartCoroutine(WaitForRealSeconds(0.5f));

                // Gimic Failed
                if(stage.CurrentGimic.IsFailed) {
                    stage.Failed();
                    GameManager.SetMode(InGameSceneModes.GET_DAMAGE); 
                }
                // Gimic Success
                else if(stage.CurrentGimic.IsSuccess) {
                    stage.Success();
                    GameManager.SetMode(InGameSceneModes.ATTACK_BEFORE);
                }
                // Else
                else {
                    GameManager.SetMode(InGameSceneModes.READY_TURN);
                }
            }
            else {
                GameManager.SetMode(InGameSceneModes.READY_TURN);    
            }
        }
        yield return null;
    }

    public void CloseGimic() {
        if(Obj != null) {
            Obj.CloseData();
        }
    }
    #endregion

    #region Attack
    public IEnumerator AttackStart() {
        if(Utils.NotNull(AttackObj, stageManager, stageManager.CurrentStage, stageManager.CurrentStage.AttackTime)) {
            AttackObj.Open(stageManager.CurrentStage.AttackTime);

            // Start Attack After Delay
            yield return StartCoroutine(WaitForRealSeconds(1.0f));
            AttackObj.StartAttack();
        }
        yield return null;
    }

    public bool Attack(int value = 1) {
        bool isEnd = false;
        if(Utils.NotNull(GameManager) && GameManager.Mode == InGameSceneModes.ATTACK) {
            isEnd = false;
            if(Utils.NotNull(stageManager, stageManager.CurrentStage, stageManager.CurrentStage.Hp, HpObj)) {
                if(Utils.NotNull(GameManager.Scenario)) {
                    GameManager.Scenario.Attack();
                    if(Utils.NotNull(GameManager.Scenario.CharacterObj)) {
                        GameManager.Scenario.CharacterObj.Attack();
                    }
                }
                
                PlaySFX(_attackSFXList[UnityEngine.Random.Range(0, _attackSFXList.Count)]);
                
                stageManager.CurrentStage.Hp.AddHpCurrent(-value);
                HpObj.GetDamage(stageManager.CurrentStage.Hp.HpCurrent);
                isEnd = stageManager.CurrentStage.Hp.IsDeath;
            }
        }
        return isEnd;
    }

    void AttackFinish() {
        if(Utils.NotNull(GameManager) && GameManager.Mode == InGameSceneModes.ATTACK) {
            GameManager.SetMode(InGameSceneModes.ATTACK_AFTER);
        }
    }

    public IEnumerator CloseAttack() {
        if(Utils.NotNull(AttackObj)) {
            AttackObj.Close();
        }
        yield return StartCoroutine(WaitForRealSeconds(1.0f));
    }
    #endregion

    public void GetDamage() {
        PlaySFX(_damageSFX);
    }

    #region Next
    public void NextStage() {
        if(Utils.NotNull(stageManager)) {
            PlaySFX(_enemyDeathSFX);
            if(Utils.NotNull(TorchObjList, TorchObjList[stageManager.StageIndex])) {
                TorchObjList[stageManager.StageIndex].Off();
            }
            stageManager.NextStage();
        }
    }
    #endregion

    void PlaySFX(AudioClip clip) {
        if(_sfxAudio) {
            _sfxAudio.clip = clip;
            _sfxAudio.Play();
        }
    }

    IEnumerator WaitForRealSeconds (float seconds) {
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup-startTime < seconds) {
            yield return null;
        }
    }
}
