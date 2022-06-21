using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMMediator : MonoBehaviour {
    AudioSource _audio;
    Animator _animator;
    [SerializeField] AudioClip _gameOverClip, _gameClearClip;

    void Awake() {
        _audio = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
    }

    public void GameOver() {
        if(_audio) {
            _audio.Stop();
            _audio.clip = _gameOverClip;
            _audio.loop = false;
            _audio.Play();
        }
    }

    public void SlowDown() {
        if(_animator) {
            _animator.SetTrigger("TurnOff");
        }
    }

    public void GameClear() {
        if(_audio) {
            _audio.Stop();
            _animator.SetTrigger("On");
            _audio.clip = _gameClearClip;
            _audio.loop = false;
            _audio.Play();
        }
    }

}
