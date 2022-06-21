using UnityEngine;
using UnityEngine.UI;

public class ToggleObject : MonoBehaviour {
    [SerializeField] protected Toggle Tgl;

    void Start() { 
        if(Tgl != null) Tgl.onValueChanged.AddListener(ToggleChanged);

        StartAter(); 
    }
    
    protected virtual void StartAter() {}
    protected void Ready(bool isOn) {
        if(Tgl != null) Tgl.isOn = isOn;
    }

    public virtual void ToggleChanged(bool isOn) {}
}
