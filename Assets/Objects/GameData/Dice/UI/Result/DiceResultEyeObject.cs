using UnityEngine;
using UnityEngine.UI;

public class DiceResultEyeObject : MonoBehaviour {
    [SerializeField] Image Img;
    [SerializeField] Animator animator;
    [SerializeField] ParticleSystem setParticle;

    Sprite tmpSprite = null;

    void Update() {
        if(animator != null && animator.GetCurrentAnimatorStateInfo(0).IsName("On")){
            if(Utils.NotNull(Img)) {
                Img.sprite = tmpSprite;
                if(setParticle != null) {
                    setParticle.Play();
                }
                animator.SetTrigger("Release");
            }
        }
    }

    public void SetData(Sprite sprite) {
        if(Utils.NotNull(Img, animator)) {
            tmpSprite = sprite;
            animator.SetTrigger("Set");
        }
    }

    public void Reset(Sprite sprite) {
        tmpSprite = sprite;
        if(Img != null) {
            Img.sprite = tmpSprite;
        }
        
    }
}
