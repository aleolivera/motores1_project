using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class EnemyMovement : MonoBehaviour {
    [Header("Movement")]
    [SerializeField] private float speed = 2f;


    [Header("Patrol Locations")]
    [SerializeField] private List<Transform> _patrolLocations;
    
    [Header("Debug")]
    [SerializeField] private Vector3 _targetPosition;
    [SerializeField] private Quaternion _targetRotation;
    [SerializeField] private Vector3 _patrolPosition;
    [SerializeField] private Transform _orientation;
    [SerializeField] private int _indexLocation = 0;
    [SerializeField] private float _patrolTime = 2f;
    [SerializeField] private float _patrolTimeCounter;

    private CharacterController _controller;
    public float PatrolTime { 
        get {return _patrolTime; } 
        set {_patrolTime = value; } 
    }

    void Awake() {
        _patrolTimeCounter = _patrolTime;
    }

    void Start() {
        _orientation    = GetComponent<Transform>();
        _controller     = GetComponent<CharacterController>();
        
        if(_patrolLocations.Count == 0) {
            _targetPosition = transform.position;
            Debug.Log("EnemyMovement: Patrol location is empty.");
        } else {
            _targetPosition = _patrolLocations[_indexLocation].position;
        }

    }

    void Update() {
        if(_patrolTimeCounter < _patrolTime) {
            _patrolTimeCounter += Time.deltaTime;
            
            transform.rotation = _targetRotation;
        } else {
            Vector3 moveTo = new Vector3(
                                    (_targetPosition.x - transform.position.x),
                                    0,
                                    _targetPosition.z - transform.position.z);
            _controller.Move(moveTo.normalized * speed * Time.deltaTime);
            transform.forward = moveTo;
            
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag.Equals("PatrolLocation")) {
            PatrolLocation location = other.gameObject.GetComponent<PatrolLocation>();

            _patrolTime = location.PatrolTime;
            _patrolTimeCounter = 0f;
            
            _patrolPosition = _targetPosition;

            if(++_indexLocation >= _patrolLocations.Count) {
                _indexLocation = 0;
            }

            _targetPosition = _patrolLocations[_indexLocation].position;
            _targetRotation = _patrolLocations[_indexLocation].rotation;
        }
    }


}
