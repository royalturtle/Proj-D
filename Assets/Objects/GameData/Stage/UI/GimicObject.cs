using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GimicObject : MonoBehaviour {
    GimicBase _gimic;
    [SerializeField] TextMeshProUGUI DescriptionTxt;
    [SerializeField] DiceResultListObject GimicDice;
    [SerializeField] List<GimicTextObject> GimicTxtList;
    [SerializeField] DiceEyesCollection eyesCollection;
    [SerializeField] GimicCheckObject _checkObject;

    [SerializeField] GimicMarbleUI _marbleUI;

    private void Start() {
        LanguageSingleton.Instance.LocalizeChanged += SetDescription;
    }

    private void OnDestroy() {
        LanguageSingleton.Instance.LocalizeChanged -= SetDescription;
    }

    public void OpenData(GimicBase gimic) {
        _gimic = gimic;
        if(Utils.NotNull(_gimic)) {
            SetDescription();

            if(Utils.NotNull(GimicDice, eyesCollection)) {
                GimicDice.Reset(eyesCollection.Question);
                _gimic.ReadyView(
                    dice:GimicDice,
                    txtList:GimicTxtList,
                    collection:eyesCollection,
                    offTextFunc:TurnOffText,
                    check:_checkObject
                );

                switch(_gimic.Type) {
                    case GimicTypes.MARBLE:
                        GimicMarble marble = (GimicMarble)_gimic;
                        if(_marbleUI) {
                            _marbleUI.gameObject.SetActive(true);
                            marble.ReadyViewCustom(ui:_marbleUI);
                        }
                        break;
                }
            }

            gameObject.SetActive(true);
        }
    }

    void ResetOtherGimics() {
        if(_marbleUI) {
            _marbleUI.gameObject.SetActive(false);
        }
    }

    void SetDescription() {
        if(Utils.NotNull(_gimic, DescriptionTxt)) {
            DescriptionTxt.text = _gimic.Description;
        }
    }
    
    void TurnOffText(int from = 0) {
        for(int i = from; i < GimicTxtList.Count; i++) {
            if(GimicTxtList[i] != null) {
                GimicTxtList[i].TurnOff();
            }
        }
    }

    public IEnumerator UpdateData(GimicBase gimic) {
        if(Utils.NotNull(gimic, GimicDice)) {
            gimic.UpdateView(
                GimicDice.DiceList, 
                GimicTxtList, 
                eyesCollection,
                check:_checkObject);

            switch(_gimic.Type) {
                case GimicTypes.MARBLE:
                    GimicMarble marble = (GimicMarble)_gimic;
                    if(_marbleUI) {
                        yield return StartCoroutine(_marbleUI.UpdateData(gimic:marble, isAnimation:true));
                    }
                    break;
            }
        }
        yield return null;
    }

    public void CloseData() {
        gameObject.SetActive(false);
    }
}
