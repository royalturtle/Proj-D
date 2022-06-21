using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectManagerObject<T> : MonoBehaviour where T : SelectObject{
    public int Selected {get; protected set;}

    protected virtual List<T> GetList { get{ return null;}}
    public virtual T GetSelected {
        get {
            T result = null;
            List<T> list = GetList;
            if(Utils.NotNull(list) && Utils.IsValidIndex(list, Selected)) {
                result = list[Selected];
            }
            return result;
        }
    }

    void Start() {
        List<T> list = GetList;
        if(list != null) {
            for(int i = 0; i < list.Count; i++) {
                list[i].Ready(index:i, selectAction:SelectAction);
            }
        }
        ClearSelect();
        StartAfter();
    }

    protected virtual void StartAfter() {}

    public void ClearSelect() {
        Selected = GameConstants.NULL_INT;
        List<T> list = GetList;
        if(list != null) {
            for(int i = 0; i < list.Count; i++) {
                list[i].Diselected();
            }
        }
        ClearAction();
    }

    protected void SelectAction(int index) {
        List<T> list = GetList;
        if(list != null) {
            if(Selected == index) {
                Selected = GameConstants.NULL_INT;
                list[index].Diselected();
                ClearAction();
            }
            else {
                if(Utils.IsValidIndex(list, Selected)) {
                    list[Selected].Diselected();
                }
                Selected = index;
                list[Selected].Pressed();
                CheckAction();
            }
            Debug.Log(Selected);
        }
    }

    protected virtual void ClearAction() {

    }

    protected virtual void CheckAction() {

    }

    public void SetInteractable(bool value) {
        List<T> list = GetList;
        if(list != null) {
            for(int i = 0; i < list.Count; i++) {
                list[i].SetInteractable(value);
            }
        }
        SetInteractableAfter(value);
    }

    protected virtual void SetInteractableAfter(bool value) {}
}
