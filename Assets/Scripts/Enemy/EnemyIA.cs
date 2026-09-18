using System.Collections.Generic;
using UnityEngine;
public enum EnemyStatus { OnPatrol, CheckingOut, OnAlert, Disabled }

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(EnemyHealth))]
public class EnemyIA : MonoBehaviour {
    [Header("Movement")]
    [SerializeField] private float _normalSpeed = 2f;
    [SerializeField] private float _maxSpeed = 4f;

    [Header("Rotation")]
    [SerializeField] private float _rotationSpeed = 7f;
    [SerializeField] private float _patrolRotationSpeed = 3f;
    [SerializeField] private float _alertRotationSpeed = 10f;

    [Header("Patrol")]
    [SerializeField] private List<Transform> _patrolLocations;
    
    [Header("Time")]
    [SerializeField] private float _patrolTime = 4f;
    [SerializeField] private float _checkTime = 6f;
    [SerializeField] private float _alertTime = 6f;
    [SerializeField] private float _disableTime = 20f;

    [Header("Debug")]
    [SerializeField] private EnemyStatus _status = EnemyStatus.OnPatrol;
    [SerializeField] private float _timeCounter = 0f;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private Vector3 _targetPosition;
    [SerializeField] private Vector3 _patrolPosition;
    [SerializeField] private Quaternion _patrolRotation;
    [SerializeField] private int _indexLocation = 0;

    private CharacterController _controller;
    private EnemyHealth _health;
    private VisionDetection _detection;

    public float PatrolTime {
        get { return _patrolTime; }
        set { _patrolTime = value; }
    }
    public EnemyStatus Status {
        get         { return _status; }
        private set { _status = value; }
    }

    void Awake() {
        _speed          = _normalSpeed;
        _timeCounter    = _patrolTime;
        _patrolPosition = transform.position;
        _rotationSpeed  = _patrolRotationSpeed;
    }

    void Start() {
        _controller = GetComponent<CharacterController>();
        _health     = GetComponent<EnemyHealth>(); 
        _detection  = GetComponent<VisionDetection>();

        if(_patrolLocations.Count == 0) {
            _targetPosition = transform.position;
            Debug.Log("EnemyMovement: Patrol location is empty.");
        } else {
            _targetPosition = _patrolLocations[_indexLocation].position;
        }

        if(_controller == null)
            Debug.Log("EnemyMovement: Character Controller not found.");
        if(_health == null)
            Debug.Log("EnemyMovement: Enemy health not found.");
        if(_detection == null)
            Debug.Log("EnemyMovement: Vision detection not found.");

        _detection.GizmoColor = Color.green;
    }

    void Update() {
        switch(_status) {
            case EnemyStatus.OnPatrol:      PatrolBehaviour();      break;
            case EnemyStatus.CheckingOut:   CheckingOutBehaviour(); break;
            case EnemyStatus.OnAlert:       AlertBehaviour();       break;
            case EnemyStatus.Disabled:      DisabledBehaviour();    break;

            default: _status = EnemyStatus.OnPatrol; break;
        }
    }
    
    private void PatrolBehaviour() {
        Vector3 playerPosition = _detection.CheckVision();

        if(playerPosition != Vector3.zero) {
            _targetPosition = playerPosition;
            ChangeToStatus(EnemyStatus.CheckingOut);
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

    private void CheckingOutBehaviour() {
        Vector3 playerPosition = _detection.CheckVision();
        if(playerPosition != Vector3.zero) {
            _targetPosition = playerPosition;
            _timeCounter = 0f;

            if(Vector3.Distance(transform.position, _targetPosition) < _detection.AlertDistance) {
                Debug.Log("Player Found!, On alert!!");

                ChangeToStatus(EnemyStatus.OnAlert);
                return;
            }
        }

        if(Vector3.Distance(_targetPosition, transform.position) > 1f) {
            MoveToTarget();
        }

        if(_timeCounter < _checkTime) {
            _timeCounter += Time.deltaTime;
        } else {
            Debug.Log("Nothing to report, On patrol...");

            ChangeToStatus(EnemyStatus.OnPatrol);
            TargetNextPatrolLocation();
            return;
        }
    }

    private void AlertBehaviour() {
        Vector3 playerPosition = _detection.CheckVision();
        if(playerPosition != Vector3.zero) {
            _targetPosition = playerPosition;
            _timeCounter = 0f;
        }

        if(Vector3.Distance(_targetPosition, transform.position) > 0.5f) {
            MoveToTarget();
        }

        if(_timeCounter < _alertTime) {
            _timeCounter += Time.deltaTime;
        } else {
            Debug.Log("Player escaped!... Back to patrol");

            ChangeToStatus(EnemyStatus.OnPatrol);
            TargetNextPatrolLocation();
            return;
        }

    }

    private void DisabledBehaviour() {
        if(_timeCounter < _disableTime) {
            _timeCounter += Time.deltaTime;
        } else {
            Debug.Log("Waking up... Back to patrol");

            ChangeToStatus(EnemyStatus.OnPatrol);
            _health.RestoreHealth(_health.MaxHealth);
            TargetNextPatrolLocation();
        }
    }

    public void DisableEnemy() {
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
            if(_status == EnemyStatus.OnPatrol) {
                PatrolLocation location = other.gameObject.GetComponent<PatrolLocation>();

                _patrolTime = location.PatrolTime;
                _timeCounter = 0f;

                _patrolPosition = location.transform.position;
                _patrolRotation = location.transform.rotation;
                
                TargetNextPatrolLocation();
            } 
        }
    }

    private void TargetNextPatrolLocation() {
        if(++_indexLocation >= _patrolLocations.Count) {
            _indexLocation = 0;
        }
        _targetPosition = _patrolLocations[_indexLocation].position;
    }

    

    private void ChangeToStatus(EnemyStatus status) {
        _status = status;

        switch(status) {
            case EnemyStatus.OnPatrol:
                _timeCounter = _patrolTime;
                _speed = _normalSpeed;
                _rotationSpeed = _patrolRotationSpeed;
                _detection.GizmoColor = Color.green;
                break;

            case EnemyStatus.CheckingOut:
                _timeCounter = 0f;
                _speed = _normalSpeed;
                _detection.GizmoColor = Color.yellow;
                break;

            case EnemyStatus.OnAlert:
                _timeCounter = 0f;
                _speed = _maxSpeed;
                _rotationSpeed = _alertRotationSpeed;
                _detection.GizmoColor = Color.red;
                break;

            case EnemyStatus.Disabled:
                _timeCounter = 0f;
                _speed = 0f;
                _detection.GizmoColor = Color.white;
                break;

            default:
                break;
        }
    }

    public void RespondToAlarm(Vector3 position) {
        if(_status != EnemyStatus.Disabled) {
            ChangeToStatus(EnemyStatus.OnAlert);
            _targetPosition = position;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag.Equals("Player")) {
            switch(_status) {
                case EnemyStatus.OnPatrol:
                    ChangeToStatus(EnemyStatus.CheckingOut);
                    break;

                case EnemyStatus.CheckingOut:
                    ChangeToStatus(EnemyStatus.OnAlert);
                    break;
                default:
                    break;
            }
            _targetPosition = collision.transform.position;
        }
    }
}
