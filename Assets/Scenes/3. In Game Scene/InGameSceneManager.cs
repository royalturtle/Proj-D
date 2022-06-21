using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameSceneManager : SceneBase {
    InGameData GameData;
    Dictionary<InGameSceneModes, Func<IEnumerator>> ModeActionDict;
    bool IsGameStart;

    ColorDictionary _colorDict;

    float CurrentTime, EndTime = 5.0f;

    [field:SerializeField] public DiceMediator Dice {get; private set;}
    [SerializeField] SkillMediator Skill;
    [field:SerializeField] public HpMediator Hp {get; private set;}
    [SerializeField] StageMediator Stage;
    [field:SerializeField] public ScenarioMediator Scenario {get; private set;}
    [SerializeField] BGMMediator _bgm;

    [SerializeField] WarningObject WarningObj;
    [SerializeField] TransitionObject TransitionObj;

    [SerializeField] GoogleAdMobScript _adMob;
    [SerializeField] AudioClip _successSFX, _failSFX, _attackTextSFX;

    AudioSource _sfxAudio;

    protected override void AwakeAfter() {
        _sfxAudio = GetComponent<AudioSource>();
        _adMob = GetComponent<GoogleAdMobScript>();
    }

    protected override void StartAfter() {
        _colorDict = new ColorDictionary();

        GameData = SaveManager.Load();
        // GameData.Ready(this);
        
        if(Utils.NotNull(GameData)) {
            if(Dice != null) {
                Dice.Ready(this, dice:GameData.Dice, releaseAction:RollDiceStart);
            }
            if(Skill != null) {
                Skill.Ready(this, GameData.Skill);
            }
            if(Hp != null) {
                Hp.Ready(this, GameData.Hp, GameData.Mp);
            }
            if(Stage != null) {
                Stage.Ready(this, GameData.Stage);
            }
        }
        
        ModeActionDict = new Dictionary<InGameSceneModes, Func<IEnumerator>> {
            {InGameSceneModes.SCENARIO, StartGame},
            {InGameSceneModes.BATTLE_START, BattleStart},
            {InGameSceneModes.READY_GIMIC, ReadyGimic},
            {InGameSceneModes.READY_TURN, ReadyTurn},
            {InGameSceneModes.ROLLING, Rolling},
            {InGameSceneModes.ROLLING_AFTER, RollingAfter},
            {InGameSceneModes.ATTACK_BEFORE, AttackBefore},
            {InGameSceneModes.ATTACK_AFTER, AttackAfter},
            {InGameSceneModes.GET_DAMAGE, GetDamage},
            {InGameSceneModes.STAGE_CLEAR, StageClear},
            {InGameSceneModes.GAME_CLEAR, GameClear},
            {InGameSceneModes.GAME_OVER, GameOver}
        };

        if(Scenario != null) {
            Scenario.SetDarkPanels(value:GameData.Mode == InGameSceneModes.SCENARIO, isAnimation:false);
        }

        
        if(TransitionObj != null) {
            if(GameData.Mode == InGameSceneModes.SCENARIO) {
                TransitionObj.SetOff();
            }
            else {
                TransitionObj.SetOn();
            }
        }

        if(GameData.Mode == InGameSceneModes.ROLLING_BEFORE) {
            LoadGameGimic();
        }
        else if(GameData.Mode == InGameSceneModes.ROLLING || GameData.Mode == InGameSceneModes.ROLLING_AFTER) {
            LoadGameGimic();
        }
        else if(GameData.FormerMode == InGameSceneModes.ATTACK_AFTER) {
            LoadAttackAfter();
        }
    }

    void LoadGameGimic() {
        if(Scenario != null) {
            Scenario.LoadGame();   
        }
        if(Stage != null) {
            Stage.SetEnemy();
        }
        
        // Ready Gimic
        if(Utils.NotNull(Stage)) {
            Stage.OpenGimic();            
        }

        if(Utils.NotNull(Dice)) {
            Dice.LoadGame();
        }

        // Allow All Button
        SetInteractable(value:true, isRolling:false);

        // Ready Skill
        if(Utils.NotNull(Skill)) {
            Skill.Reset(Hp);
        }

        if(TransitionObj != null) {
            TransitionObj.TurnOff();
        }
    }

    public void LoadAttackAfter() {
        if(Scenario != null) {
            Scenario.LoadAttackAfter();   
        }
        if(Stage != null) {
            Stage.SetEnemy();
        }
        if(Utils.NotNull(Dice)) {
            Dice.LoadAttackAfter();
        }


        // Allow All Button
        SetInteractable(value:true, isRolling:false);

        // Ready Skill
        if(Utils.NotNull(Skill)) {
            Skill.Reset(Hp);
        }
        if(TransitionObj != null) {
            TransitionObj.TurnOff();
        }
        
        SetMode(Stage.IsDeath ? InGameSceneModes.STAGE_CLEAR : InGameSceneModes.READY_GIMIC);
    }

    void Update() {
        if(GameData != null) {
            if(ModeActionDict.ContainsKey(GameData.Mode)) {
                InGameSceneModes mode = GameData.NextMode();
                StartCoroutine(ModeActionDict[mode]());
            }
            else {
                switch(GameData.Mode) {
                    case InGameSceneModes.ATTACK:
                        CurrentTime += Time.deltaTime;
                        if(CurrentTime >= EndTime) {
                            SetMode(InGameSceneModes.ATTACK_AFTER);
                        }
                        break;
                }
            }
        }
        CheckQuitGame();
    }

    IEnumerator StartGame() {
        if(Scenario != null) {
            yield return StartCoroutine(Scenario.GameStart());
        }
        SetMode(InGameSceneModes.BATTLE_START);
        yield return null;
    }

    IEnumerator BattleStart() {
        // Ready Enemy
        if(Utils.NotNull(Stage)) {
            
            if(_adMob && Stage.StageIndex == 2) {
                _adMob.AdStart();
            }
            yield return StartCoroutine(Stage.ReadyEnemy());
        }

        if(IsGameStart && Scenario != null) {
            Scenario.ReadyEnemyAnim();
        }

        // Next State
        SetMode(InGameSceneModes.READY_GIMIC);
        yield return null;
    }

    IEnumerator ReadyGimic() {
        if(Scenario != null) {
            if(!IsGameStart) {
                Scenario.StartAnim();
                IsGameStart = true;
            } else {
                Scenario.ReadyGimicAnim();
            }
        }

        // Ready Gimic
        if(Utils.NotNull(Stage)) {
            Stage.OpenGimic();            
        }
        
        // Next State
        SetMode(InGameSceneModes.READY_TURN);
        yield return null;
    }

    IEnumerator ReadyTurn() {

        // Ready Dice
        if(Dice != null) {
            Dice.Reset();
        }

        // Ready MP
        if(Utils.NotNull(Hp)) {
            Hp.AddMp();
        }

        // Ready Skill
        if(Utils.NotNull(Skill)) {
            Skill.Reset(Hp);
        }

        // Allow All Button
        SetInteractable(value:true, isRolling:false);

        // Next State
        SetMode(InGameSceneModes.ROLLING_BEFORE);
        Save();
        yield return null;
    }
    
    void RollDiceStart() {
        if(Utils.NotNull(Dice)) {
            bool rollResult = Dice.RollDice();
            if(rollResult) {
                SetMode(InGameSceneModes.ROLLING);
                Save();
            }
        }    
    }

    public void SkipChance() {
        if(Dice != null) {
            Dice.ResetChance();
        }
        SetMode(InGameSceneModes.ROLLING_AFTER);
        Save();
    }

    IEnumerator Rolling() {
        // Block Other Buttons
        SetInteractable(value:false, isRolling:false);

        // Show Dice Rolling
        if(Utils.NotNull(Dice)) {
            Dice.RollDiceUI();
        }
        
        yield return null;
    }

    IEnumerator RollingAfter() {
        if(Utils.NotNull(Dice) && Dice.IsChanceRemain) {
            if(Dice) {
                yield return StartCoroutine(WaitForRealSeconds(0.6f));
                // Allow All Button
                SetInteractable(value:true, isRolling:false);
                Dice.EmptyRollContent();
                Dice.SetActiveAdditional(true);
                SetMode(InGameSceneModes.ROLLING_BEFORE);
                Save();
            }
        }
        else {
            if(Dice) {
                Dice.SetActiveAdditional(false);
            }

            if(Utils.NotNull(Stage != null)) {
                Stage.UpdateResult(Dice.RecentResult);
            }

            if(Utils.NotNull(Stage)) {
                yield return StartCoroutine(Stage.UpdateGimic());
            }
        }
        yield return null;
    }

    IEnumerator AttackBefore() {
        PlayerPrefs.SetInt(GameConstants.PrefAttackCount, 0);

        // Block All Interactables
        SetInteractable(false);

        // Init Time
        CurrentTime = 0.0f;

        if(Scenario != null) {
            Scenario.ShowResultText(LanguageSingleton.Instance.GetGUI(20), color:_colorDict.Green);
            PlaySFX(_successSFX);
            Scenario.CloseGimicAnim();
            yield return StartCoroutine(WaitForRealSeconds(1.5f));
        }

        if(Scenario != null) {
            Scenario.ChangeResultText(LanguageSingleton.Instance.GetGUI(15), color:_colorDict.Orange);
            PlaySFX(_attackTextSFX);
        }
        
        // Ready Attack UI
        if(Utils.NotNull(Stage)) {
            Stage.CloseGimic();
            StartCoroutine(Stage.AttackStart());
        }

        SetMode(InGameSceneModes.ATTACK);
         
        yield return null;
    }

    IEnumerator AttackAfter() {
        if(Scenario != null) {
            Scenario.HideResultText();
            yield return StartCoroutine(WaitForRealSeconds(0.5f));
        }

        Save();

        // Close Attack UI
        if(Utils.NotNull(Stage)) {
            StartCoroutine(Stage.CloseAttack());
            SetMode(Stage.IsDeath ? InGameSceneModes.STAGE_CLEAR : InGameSceneModes.READY_GIMIC);
        }
        yield return null;
    }

    IEnumerator GetDamage() {
        if(Utils.NotNull(Stage)) {
            Stage.CloseGimic();
        }
        
        if(Scenario != null) {
            Scenario.ShowResultText(LanguageSingleton.Instance.GetGUI(21), color:_colorDict.Red);
            PlaySFX(_failSFX);
            Scenario.CloseGimicAnim();
            yield return StartCoroutine(WaitForRealSeconds(1.2f));
        }

        if(Utils.NotNull(Hp)) {
            if(Utils.NotNull(Scenario)) {
                Scenario.Damage();
            }
            if(Utils.NotNull(Stage)) {
                Stage.GetDamage();
            } 
            yield return StartCoroutine(Hp.GetDamage());

            SetMode(Hp.IsDeath ? InGameSceneModes.GAME_OVER : InGameSceneModes.READY_GIMIC);
        }
        
        if(Scenario != null) {
            Scenario.HideResultText();
            yield return StartCoroutine(WaitForRealSeconds(1.5f));
        }
        yield return null;
    }

    IEnumerator StageClear() {
        if(Utils.NotNull(Stage, TransitionObj)) {
            Stage.NextStage();

            if(Scenario != null) {
                Scenario.ClearStageAnim();
            }

            if(Scenario != null) {
                yield return StartCoroutine(Scenario.NextStage());
            }                

            if(Stage.IsClear) {
                SetMode(InGameSceneModes.GAME_CLEAR);
            }
            else {
                SetMode(InGameSceneModes.BATTLE_START);
            }

        }
        yield return null;
    }

    IEnumerator GameClear() {
        SaveManager.Delete();
        if(Scenario != null) {
            yield return StartCoroutine(Scenario.GameClear());
        }
        if(TransitionObj != null) {
            TransitionObj.TurnOn();
            yield return StartCoroutine(WaitForRealSeconds(2.0f));
            LoadClearScene();
        }
        yield return null;
    }

    IEnumerator GameOver() {
        SaveManager.Delete();
        if(Scenario != null) {
            Scenario.GameOver();
        }
        if(_bgm) {
            _bgm.GameOver();
        }
        yield return null;
    }

    public void SetInteractable(bool value, bool isSettingOn = false, bool isRolling = false) {
        if(Dice != null) {
            Dice.SetInteractable(value, isRolling);
        }

        if(Skill != null) {
            Skill.SetInteractable(value);
        }
    }

    public void OpenWarning(string text) {
        if(Utils.NotNull(WarningObj)) {
            WarningObj.Open(text);
        }
    }

    public InGameSceneModes Mode {
        get {
            return GameData == null ? InGameSceneModes.NONE : GameData.Mode;
        }
    }

    public bool IsMode(InGameSceneModes mode) {
        return Mode == mode;
    }

    public void SetMode(InGameSceneModes mode) {
        if(GameData != null) {
            GameData.SetMode(mode);
        }
    }

    public void Save() {
        SaveManager.Save(GameData);
    }

    public void AskRestart() {
        if(_askDialog != null) {
            _askDialog.Open(text:LanguageSingleton.Instance.GetGUI(11), okAction:Restart);
        }
    }

    public void AskTitleMenu() {
        if(_askDialog != null) {
            _askDialog.Open(text:LanguageSingleton.Instance.GetGUI(12), okAction:ToTitleMenu);
        }
    }

    public void Restart() {
        if(_adMob) {
            _adMob.AdStart();
        }
        if(TransitionObj != null) {
            TransitionObj.TurnOn(
                onAction:delegate {
                    SaveManager.CreateNewGame();
                    LoadInGameScene();
                },
                isTurnOnAuto:false
            );
        }
    }

    public void ToTitleMenu() {
        if(_adMob) {
            _adMob.AdStart();
        }
        if(TransitionObj != null) {
            TransitionObj.TurnOn(
                onAction:delegate {
                    LoadTitleScene();
                },
                isTurnOnAuto:false
            );
        }
    }

    IEnumerator WaitForRealSeconds (float seconds) {
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup-startTime < seconds) {
            yield return null;
        }
    }
    void PlaySFX(AudioClip clip) {
        if(_sfxAudio) {
            _sfxAudio.clip = clip;
            _sfxAudio.Play();
        }
    }
}
