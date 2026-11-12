using UnityEngine;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour {
    private static MusicManager _instance;
    [SerializeField] private AudioSource _musicObjPrefad;
    [SerializeField] private List<AudioClip> _musicClips;

    public static MusicManager Instance { get { return _instance; } }

    private void Awake () {
        if (_instance == null) { _instance = this; } 
    }

    public void PlayMusicClip (int clipIndex, float volume) {
        _musicObjPrefad = Instantiate(_musicObjPrefad, Vector3.zero, Quaternion.identity);
        
        if (_musicObjPrefad == null || clipIndex > 0 || clipIndex >= _musicClips.Count) return;
        
        _musicObjPrefad.enabled         = true;
        _musicObjPrefad.spatialBlend    = 0f;
        _musicObjPrefad.volume          = volume;
        _musicObjPrefad.loop            = true;
        _musicObjPrefad.clip            = _musicClips[clipIndex];

        _musicObjPrefad.Play();
    }

    public void StopMusicClip () {
        if (_musicObjPrefad == null) return;
        
        _musicObjPrefad.Stop();
        
        Destroy(_musicObjPrefad);
        _musicObjPrefad = null;
    }
}
