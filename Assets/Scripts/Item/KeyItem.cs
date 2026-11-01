using UnityEngine;

public class KeyItem : Item {
    SFXPlayer _sfxPlayer;
    private void Start () {
        _sfxPlayer = GetComponent<SFXPlayer>();
        if (_sfxPlayer == null) {
            Debug.LogError("Lock: SFXPlayer not found");
        }        
    }

    public void PlayKeyItemCollectedSfx () => _sfxPlayer.PlaySound(0, 1f);
}
