using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceEyesCollection : MonoBehaviour {
    [field:SerializeField] public Sprite Question {get; private set;}
    [field:SerializeField] public List<Sprite> White {get; private set;}
    [field:SerializeField] public List<Sprite> Red   {get; private set;}
    [field:SerializeField] public List<Sprite> Green {get; private set;}
    [field:SerializeField] public List<Sprite> Gray  {get; private set;}
    [field:SerializeField] public List<Sprite> Blue  {get; private set;}

    public Sprite GetWhite(int index) {
        return GetSprite(White, index);
    }

    public Sprite GetRed(int index) {
        return GetSprite(Red, index);
    }

    public Sprite GetGreen(int index) {
        return GetSprite(Green, index);
    }

    public Sprite GetGray(int index) {
        return GetSprite(Gray, index);
    }

    public Sprite GetBlue(int index) {
        return GetSprite(Blue, index);
    }

    Sprite GetSprite(List<Sprite> list, int index) {
        return Utils.IsValidIndex(list, index) ? list[index] : null;
    }
}
