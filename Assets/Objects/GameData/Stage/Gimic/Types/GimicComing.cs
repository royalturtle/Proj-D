using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

[DataContract]
public class GimicComing : GimicBase {
    #region Variables
    [DataMember] public int LastAttackIndex {get; private set;}
    [DataMember] public int HpMax {get; private set;}
    [DataMember] public int HpCurrent {get; private set;}
    [DataMember] public int SummonMax {get; private set;}
    [DataMember] public int SummonCurrent {get; private set;}
    [DataMember] public int KillCount {get; private set;}
    [DataMember] public int Height {get; private set;}
    [DataMember] public int Width {get; private set;}
    [DataMember] public int[,] Grid {get; private set;}
    [DataMember] public List<List<int>> DiceInfo {get; private set;}
    [DataMember] public List<Vector2Int> MoveList {get; private set;}
    // public List<Vector2Int> MoveList {get; private set;}
    #endregion

    public GimicComing(int hp, int height, int width, int summonMax) : base(type:GimicTypes.COMING, opportunity:0) {
        HpMax = hp;
        Height = height;
        Width = width;
        SummonMax = summonMax;
        ResetAfter();
    }
    
    #region Reset
    protected override void ResetAfter() {
        KillCount = 0;
        LastAttackIndex = 0;
        HpCurrent = HpMax;
        SummonCurrent = SummonMax;
        MoveList = new List<Vector2Int>();
        InitGrid();
    }

    void InitGrid() {
        DiceInfo = new List<List<int>>();
        for(int i = 0; i < Height; i++) {
            DiceInfo.Add(new List<int>());
        }

        List<int> diceRandom = new List<int>{1,2,3,4,5,6};
        diceRandom = Utils.ShuffleList(diceRandom);
        for(int i = 0; i < diceRandom.Count; i++) {
            DiceInfo[i % Height].Add(diceRandom[i]);
        }
        DiceInfo = Utils.ShuffleList(DiceInfo);

        Grid = new int[Height, Width];
        CreateEnemy();
    }
    #endregion
    #region Result
    protected override void CheckResult() {
        IsFailed = HpCurrent <= 0;
        IsSuccess = !IsFailed && (KillCount == SummonMax);
    }
    #endregion

    #region Update
    void CreateEnemy() {
        if(SummonCurrent > 0) {
            int index = UnityEngine.Random.Range(0, Height);
            SummonCurrent--;
            Grid[index, Width - 1] = 1;
        }
    }

    void MoveEnemy() {
        if(MoveList == null) MoveList = new List<Vector2Int>();
        else MoveList.Clear();

        for(int x = 0; x < Width; x++) {
            for(int y = 0; y < Height; y++) {
                int value = Grid[y, x];
                if(value != 0) {
                    MoveList.Add(new Vector2Int(x - 1, y));

                    // Erase Previous
                    Grid[y, x] = 0;

                    // IF ENEMY COME
                    if(x <= 0) {
                        HpCurrent--;
                    } // ENEMY MOVE
                    else {
                        Grid[y, x - 1] = value;
                    }
                }
            }
        }
    }

	protected override void UpdateResultAfter() {
        int result = Recent;
        int index = GameConstants.NULL_INT;
        for(int i = 0; i < DiceInfo.Count && index == GameConstants.NULL_INT; i++) {
            for(int j = 0; j < DiceInfo[i].Count && index == GameConstants.NULL_INT; i++) {
                if(DiceInfo[i][j] == result) {
                    index = i;
                }
            }
        }

        if(index != GameConstants.NULL_INT) {

        }

        MoveEnemy();
        CreateEnemy();
    }
    #endregion

    public void ReadyView(GimicComingObject viewObj) {
        if(Utils.NotNull(viewObj)) {

        }
    }

    public void UpdateView(GimicComingObject viewObj) {
        if(Utils.NotNull(viewObj)) {

        }
    }
}
