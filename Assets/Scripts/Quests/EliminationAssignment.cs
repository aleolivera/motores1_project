using System.Collections.Generic;
using UnityEngine;

public class EliminationAssignment : Quest {
    [Header("Quest Data")]
    [SerializeField] private List<EnemyDetection> _enemyList;
    [SerializeField] private GameObject _spawnerObj;
    private float _maxDisableTime = 86400f;
    private float _counter = 0f;
    private float _CheckQuestTime = 1f;

    private LinkedList<EnemyDetection> _enemyLinkedList = new LinkedList<EnemyDetection>();
    public LinkedList<EnemyDetection> EnemyList { get { return _enemyLinkedList; } }
    private Transform spawnItem;

    private void Start () {
        foreach(var enemy in _enemyList) {
            enemy.DisableTime = _maxDisableTime;
            _enemyLinkedList.AddLast(enemy);
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
        LinkedListNode<EnemyDetection> node = _enemyLinkedList.First;

        while (node != null) {
            LinkedListNode<EnemyDetection> next = node.Next;

            if (node.Value.State == DetectionState.Disabled) {
                spawnItem = node.Value.transform;
                _enemyLinkedList.Remove(node);
            }

            node = next;
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
