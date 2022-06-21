using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    [field:SerializeField] public bool interactable {get; private set;}
    public bool IsPressing {get; private set;}

    public Action HoldAction, ReleaseAction, DownAction;

    [SerializeField] Image Img;
    [SerializeField] Sprite UpSprite, DownSprite;

    public void OnPointerDown(PointerEventData eventData) {
        if(interactable) {
            if(!IsPressing) {
                IsPressing = true;
                if(DownAction != null) {
                    DownAction();
                }
                if(Img != null && DownSprite != null) {
                    Img.sprite = DownSprite;
                }
            }
            else {
                if(HoldAction != null) {
                    HoldAction();
                }
            }            
        }
    }

    public void OnPointerUp(PointerEventData eventData) {
        if(interactable) {
            if(Img != null && UpSprite != null) {
                Img.sprite = UpSprite;
            }
            
            if(ReleaseAction != null) {
                ReleaseAction();
            }

            IsPressing = false;
        }
    }

    public void SetInteractable(bool value) {
        interactable = value;
        if(Img != null) {
            Img.color = new Color32(255, 255, 255, (byte)(interactable ? 255 : 150));
        }
    }
}
