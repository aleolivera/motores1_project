using UnityEngine;

public class QuestItem : Item {
    SFXPlayer _sfxPlayer;

    private void Start () {
        _sfxPlayer = GetComponent<SFXPlayer>();
        if (_sfxPlayer == null) {
            Debug.LogError("QuestItem: SFXPlayer not found");
        }
    }

    public void PlayQuestItemCollectedSfx () => _sfxPlayer.PlaySound(0, 1f);
    
}
