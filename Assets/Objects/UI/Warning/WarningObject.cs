using UnityEngine;
using TMPro;

public class WarningObject : MonoBehaviour {
    [SerializeField] Animator animator;
    [SerializeField] TextMeshProUGUI Txt;

    AudioSource _sfxAudio;

    void Awake() {
        _sfxAudio = GetComponent<AudioSource>();
    }

    public void Open(string text) {
        if(Utils.NotNull(animator, Txt)) {
            Txt.text = text;
            _sfxAudio.Play();
            animator.SetTrigger("Open");
        }
    }
}
