using System.Collections.Generic;
using UnityEngine;

public class EliminationAssignment : Quest {
    [Header("Quest Data")]
    [SerializeField] private List<EnemyDetection> _enemyList;
    [SerializeField] private GameObject _spawnerObj;
    private float _maxDisableTime = 86400f;
    private float _counter = 0f;
    private float _CheckQuestTime = 1f;
    public List<EnemyDetection> EnemyList { get { return _enemyList; } }
    private Transform spawnItem;

    private void Start () {
        foreach(var enemy in _enemyList) {
            enemy.DisableTime = _maxDisableTime;
        }

        _spawnerObj.SetActive(false);
    }

    private void Update () {
        if (IsQuestCompleted) return;
        
        if(_counter < _CheckQuestTime) {
            _counter += Time.deltaTime;
            return;
        }

        CheckEnemiesDisabled();
        _counter = 0f;
    }

    void CheckEnemiesDisabled () {
        foreach (EnemyDetection enemy in _enemyList) {
            if (enemy != null && enemy.State == DetectionState.Disabled) {
                spawnItem = enemy.gameObject.transform;
                _enemyList.Remove(enemy);
                if (_enemyList.Count < 1) break;
            }
        }

        if (EnemyList.Count > 0) { return; }

        SpawnObject(spawnItem);
        IsQuestCompleted = true;
        LevelManager.Instance.QuestCompleted(Id);
    }

    void SpawnObject (Transform transform) {
        if (_spawnerObj == null) return;

        _spawnerObj.SetActive(true);
        _spawnerObj.transform.position = transform.position + new Vector3(0.5f,0f,0.5f);
        _spawnerObj.transform.rotation = Quaternion.identity;
    }


}
