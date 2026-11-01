using System.Collections.Generic;
using UnityEngine;

public enum EnemyStatus { OnPatrol, Checking, OnAlert, Disabled }

[RequireComponent(typeof(CharacterController))]
public class EnemyMovement : MonoBehaviour {
    [Header("Movement")]
    [SerializeField] private float _normalSpeed = 2f;
    [SerializeField] private float _maxSpeed = 4f;
    [Header("Rotation")]
    [SerializeField] private float _rotationSpeed = 10f;

    [Header("Patrol")]
    [SerializeField] private List<Transform> _patrolLocations;

    [Header("Vision")]
    [SerializeField] private float _visionLength = 10f;
    [SerializeField] private LayerMask _visionLayer;
    [SerializeField] private Transform _visionLine;

    [Header("Time")]
    [SerializeField] private float _patrolTime = 2f;
    [SerializeField] private float _checkTime = 5f;
    [SerializeField] private float _alertTime = 5f;
    [SerializeField] private float _disableTime = 5f;

    [Header("Debug")]
    [SerializeField] private EnemyStatus _status = EnemyStatus.OnPatrol;
    [SerializeField] private float _timeCounter = 0f;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private Vector3 _targetPosition;
    [SerializeField] private Vector3 _patrolPosition;
    [SerializeField] private Quaternion _patrolRotation;
    [SerializeField] private int _indexLocation = 0;

    private CharacterController _controller;

    private Color lineColor = Color.green;

    public float PatrolTime { 
        get { return _patrolTime; } 
        set { _patrolTime = value; } 
    }

    void Awake() {
        _speed = _normalSpeed;
        _timeCounter = _patrolTime;
        _patrolPosition = transform.position;
    }

    void Start() {
        _controller     = GetComponent<CharacterController>();
        
        if(_patrolLocations.Count == 0) {
            _targetPosition = transform.position;
            Debug.Log("EnemyMovement: Patrol location is empty.");
        } else {
            _targetPosition = _patrolLocations[_indexLocation].position;
        }

        if(_visionLine == null) 
            Debug.Log("EnemyMovement: Vision line is missing.");
        if(_controller== null)
            Debug.Log("EnemyMovement: Character Controller not found.");


    }

    void Update() {

        switch(_status) {
            case EnemyStatus.OnPatrol:  PatrolStatus();     break;
            //case EnemyStatus.Checking:  CheckStatus();      break;
            //case EnemyStatus.OnAlert:   AlertStatus();      break; 
            //case EnemyStatus.Disabled:  DisabledStatus();   break; 
                
            default:
                _status = EnemyStatus.OnPatrol;
                break;
        }

        DrawVisionLine();
        
    }

    private Vector3 CheckVisionLine() {
        RaycastHit hit;
        bool playerHit = Physics.Raycast(_visionLine.position, _visionLine.forward, out hit, _visionLength, _visionLayer);

        return (playerHit) ? hit.transform.position : Vector3.zero ; 
    }

    private void PatrolStatus() {
        Vector3 playerPosition = CheckVisionLine();

        if(playerPosition != Vector3.zero) {
            Debug.Log("Checking for Player!");

            _targetPosition = playerPosition;
            ChangeToStatus(EnemyStatus.Checking);
            return;
        }

        if(_timeCounter < _patrolTime) {
            _timeCounter += Time.deltaTime;
            
            if(transform.rotation.y != _patrolRotation.eulerAngles.y) {
                RotateTo(_patrolRotation);
            }
        } else {
            MoveToTarget();
        }
    }
    private void CheckStatus() {
        Vector3 playerPosition = CheckVisionLine();
        if(playerPosition != Vector3.zero) {
            _targetPosition = playerPosition;
            _timeCounter = 0f;
            
            if(Vector3.Distance(transform.position,_targetPosition) < _visionLength / 2) {
                Debug.Log("Player Found!, On alert!!");

                ChangeToStatus(EnemyStatus.OnAlert);
                return;
            }
        }
        MoveToTarget();

        if(_timeCounter < _checkTime) {
            _timeCounter += Time.deltaTime;
        } else {
            Debug.Log("Nothing to report, On patrol...");

            ChangeToStatus(EnemyStatus.OnPatrol);
            _targetPosition = _patrolPosition;
            return;
        }

    }

    private void AlertStatus() {
        Vector3 playerPosition = CheckVisionLine();
        if(playerPosition != Vector3.zero) {
            _targetPosition = playerPosition;
            _timeCounter = 0f;
        }

        MoveToTarget();

        if(_timeCounter < _alertTime) {
            _timeCounter += Time.deltaTime;
        } else {
            Debug.Log("Player escaped!, On patrol... ");

            ChangeToStatus(EnemyStatus.OnPatrol);
            _targetPosition = _patrolPosition;
            return;
        }

    }

    private void DisabledStatus() {
        if(_timeCounter < _disableTime) {
            _timeCounter += Time.deltaTime;
        } else {
            Debug.Log("Waking up... On patrol");

            ChangeToStatus(EnemyStatus.OnPatrol);
            _targetPosition = _patrolPosition;
        }
    }

    private void DisableEnemy() {
        ChangeToStatus(EnemyStatus.Disabled);
    }

    private void MoveToTarget() {
        Vector3 moveTo = new Vector3(
                                _targetPosition.x - transform.position.x,
                                0f,
                                _targetPosition.z - transform.position.z);


        _controller.Move(moveTo.normalized * _speed * Time.deltaTime);
        RotateTo(moveTo);
    }

    private void RotateTo(Quaternion targetRotation) {
        transform.rotation = Quaternion.Slerp(
                                            transform.rotation,
                                            targetRotation,
                                            _rotationSpeed * Time.deltaTime);
    }

    private void RotateTo(Vector3 targetLocation) {
        transform.forward = Vector3.Slerp(
                                        transform.forward,
                                        targetLocation,
                                        _rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag.Equals("PatrolLocation")) {
            PatrolLocation location = other.gameObject.GetComponent<PatrolLocation>();

            _patrolTime = location.PatrolTime;
            _timeCounter = 0f;
            
            _patrolPosition = _targetPosition;
            _patrolRotation = location.transform.rotation;

            if(++_indexLocation >= _patrolLocations.Count) {
                _indexLocation = 0;
            }

            _targetPosition = _patrolLocations[_indexLocation].position;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag.Equals("Player")) {
            if(_status == EnemyStatus.OnPatrol) {
                ChangeToStatus(EnemyStatus.Checking);
                _targetPosition = collision.transform.position;
            }
            if(_status == EnemyStatus.Checking) {
                ChangeToStatus(EnemyStatus.OnAlert);
                _targetPosition = collision.transform.position;
            }
            if(_status == EnemyStatus.OnAlert) {
                // Implementar Attack();
                _targetPosition = collision.transform.position;
            }
        }
    }

    private void ChangeToStatus(EnemyStatus status) {
        _status = status;
        
        switch(status) {
            case EnemyStatus.OnPatrol: 
                _timeCounter = _patrolTime;
                _speed = _normalSpeed;
                lineColor = Color.green;
                break;

            case EnemyStatus.Checking:
                _timeCounter = 0f;
                _speed = _normalSpeed;
                lineColor = Color.yellow;
                break;

            case EnemyStatus.OnAlert: 
                _timeCounter = 0f;
                _speed = _maxSpeed;
                lineColor = Color.red;
                break;

            case EnemyStatus.Disabled: 
                _timeCounter = 0f;
                _speed = 0f;
                lineColor= Color.white;
                break;

            default: 
                break;
        }
    }

    private void DrawVisionLine() {
        Vector3 from = (_visionLine.position + _visionLine.up);
        Vector3 to = from + _visionLine.forward * 10f;

        Debug.DrawLine(from, to, Color.green);
    }
}
