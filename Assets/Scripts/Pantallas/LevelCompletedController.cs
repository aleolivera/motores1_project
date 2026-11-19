using System;
using TMPro;
using UnityEngine;

public class LevelCompletedController : MonoBehaviour {
    [Header("Text references")]
    [SerializeField] private TextMeshProUGUI _tmpDeaths;
    [SerializeField] private TextMeshProUGUI _tmpDisables;
    [SerializeField] private TextMeshProUGUI _tmpTime;
    [SerializeField] private TextMeshProUGUI _tmpAlerts;

    private void Start () {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SetTmpTexts();
    }

    public void HandleContinueClick () => GameManager.Instance.LoadNextScene();

    private void SetTmpTexts () {
        TimeSpan time = TimeSpan.FromSeconds(GameManager.Instance.LevelCompletedTime);
        
        _tmpTime.text       = time.ToString("mm':'ss'.'ffff");
        _tmpAlerts.text     = GameManager.Instance.AlertCount.ToString();
        _tmpDeaths.text     = GameManager.Instance.PlayerDeathCount.ToString();
        _tmpDisables.text   = GameManager.Instance.EnemyDisabledCount.ToString();
    }

}
