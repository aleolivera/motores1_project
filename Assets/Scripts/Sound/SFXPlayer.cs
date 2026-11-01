using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class SFXPlayer : MonoBehaviour {
    [Header("SFX Settings")]
    [SerializeField] private List<AudioClip> _clips;

    public void PlaySound(int clipIndex, float volume = 1f) {
        if (clipIndex < 0 || clipIndex >= _clips.Count || _clips.Count < 1) return;
        
        SoundFXManager.Instance.PlaySFXClip(_clips[clipIndex], gameObject.transform, volume);
    }
}
