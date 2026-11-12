using UnityEngine;
using UnityEngine.Audio;

public class SoundFXManager : MonoBehaviour {
    private static SoundFXManager _instance;
    private AudioSource _soundFXprefad;

    public static SoundFXManager Instance { get { return _instance; } }

    private void Awake () {
        if(_instance == null) {
            _instance = this;
        }
    }

    public void PlaySFXClip (AudioClip sfxClip, Transform obj, float volume) {
        _soundFXprefad = SoundPool.Instance.GetObject();
        
        if (_soundFXprefad == null) return;

        _soundFXprefad.gameObject.transform.position = obj.position;
        _soundFXprefad.gameObject.transform.rotation = obj.rotation;

        _soundFXprefad.spatialBlend = 1f;
        _soundFXprefad.volume       = volume;
        _soundFXprefad.clip         = sfxClip;
        
        _soundFXprefad.Play();

        SoundPool.Instance.ReturnObject(_soundFXprefad, _soundFXprefad.clip.length);
    }
}
