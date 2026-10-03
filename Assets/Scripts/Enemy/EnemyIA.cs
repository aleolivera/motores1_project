using System;
using System.Collections.Generic;
using UnityEngine;

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
    
    [Header("Debug")]
    [SerializeField] private float _timeCounter = 0f;
    [SerializeField] private float _visionCounter = 0f;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private Transform _player;
    [SerializeField] private float _playerVisibility;

    [SerializeField] private Vector3 _targetPosition;
    [SerializeField] private Quaternion _patrolRotation;
    [SerializeField] private int _indexLocation = 0;

    private CharacterController _controller;
    private EnemyHealth _health;
    private EnemyAttack _attack;
    private EnemyVision _vision;
    private EnemyDetection _detection;
    private EnemyStateMachine _stateMachine;
    public Vector3 PlayerPosition{ get { return _player.position; } }

    void Awake() {
        _speed          = _normalSpeed;
        _rotationSpeed  = _patrolRotationSpeed;
    }

    void Start() {
        _controller     = GetComponent<CharacterController>();
        _health         = GetComponent<EnemyHealth>();
        _attack         = GetComponent<EnemyAttack>();
        _vision         = GetComponent<EnemyVision>();
        _detection      = GetComponent<EnemyDetection>();
        _stateMachine   = GetComponent<EnemyStateMachine>();

        foreach (Transform t in _patrolLocations) {
            if(t == null) {
                Debug.Log("EnemyMovement: Patrol location is empty.");
            } else {
                _targetPosition = transform.position;
            }
        }

        if(_controller == null) {
            Debug.Log("EnemyMovement: Character Controller not found.");
        } else {
            _controller.detectCollisions = false;
        }
        if(_health == null) {
            Debug.Log("EnemyMovement: Enemy health not found.");
        }
        if(_attack == null) {
            Debug.Log("EnemyMovement: Enemy attack not found.");
        }
        if(_vision == null) {
            Debug.Log("EnemyMovement: Enemy vision not found.");
        }
        if(_detection == null) {
            Debug.Log("EnemyMovement: Enemy detection not found.");
        }
        
        _targetPosition = _patrolLocations[_indexLocation].position;
        _timeCounter    = _detection.PatrolTime;
    }

    void VisionCheck () {
        if(_visionCounter > 0f) {
            _visionCounter -= Time.deltaTime;
        } else {
            _playerVisibility   = _vision.GetVisibility(out _player);
            _visionCounter      = _detection.VisionTime;
        }
    }

    void Update() {
        VisionCheck();
        switch (_detection.State) {
            case DetectionState.OnPatrol:   PatrolBehaviour();      break;
            case DetectionState.OnCaution:  CautionBehaviour();     break;
            case DetectionState.OnAlert:    AlertBehaviour();       break;
            case DetectionState.Disabled:   DisabledBehaviour();    break;
            case DetectionState.Checking:   CheckingBehaviour();    break;

            default: _detection.State = DetectionState.OnPatrol;    break;
        }
    }

    private void CheckingBehaviour () {
        if (_player != null) {
            ChangeToStatus(DetectionState.OnAlert);
            return;
        }

        if (_timeCounter < _detection.CheckTime) {
            _timeCounter += Time.deltaTime;
            return;
        }

        TargetNextPatrolLocation();
        ChangeToStatus(DetectionState.OnPatrol);
    }

    private void PatrolBehaviour () {
        if (_stateMachine.State == EnemyActionState.WakeUp) return;

        if (_player != null) {
            _targetPosition = _player.position;
            ChangeToStatus(DetectionState.OnCaution);
            return;
        }

        if (_timeCounter < _detection.PatrolTime) {
            _timeCounter += Time.deltaTime;

            if (transform.rotation.y != _patrolRotation.eulerAngles.y) {
                RotateTo(_patrolRotation);
            }

        } else {
            MoveToTarget();
            _stateMachine.State = EnemyActionState.Walking;
        }
    }

    private void CautionBehaviour () {
        if (_player != null) {
            _targetPosition = _player.position;
            _timeCounter = 0f;

            float distanceToPlayer = Vector3.Distance(transform.position, _targetPosition);
            if (distanceToPlayer < _vision.Range * 0.5f) {
                ChangeToStatus(DetectionState.OnAlert);
                return;
            }
        }

        if(Vector3.Distance(_targetPosition, transform.position) > 1f) {
            MoveToTarget();
            _stateMachine.State = EnemyActionState.Walking;
        } else {
            _stateMachine.State = EnemyActionState.Idle;
            ChangeToStatus(DetectionState.Checking);
        }

        if(_timeCounter < _detection.CautionTime) {
            _timeCounter += Time.deltaTime;
            return;
        }

        Debug.Log("Nothing to report, On patrol...");
        TargetNextPatrolLocation();
        ChangeToStatus(DetectionState.OnPatrol);
    }

    private void AlertBehaviour () {
        //PLAYER en rango de vision
        if (_player != null) {
            _targetPosition = _player.position;
            _timeCounter = 0f;
            RotateToTarget();
            
            if (Vector3.Distance(transform.position, _targetPosition) > _vision.Range * 0.5f 
                && _stateMachine.State != EnemyActionState.Attacking) {
                
                MoveToTarget();
                _stateMachine.State = EnemyActionState.Running;
                return;
            }

            _stateMachine.State = EnemyActionState.Attacking;
            return;
            
        } 
        //PLAYER fuera de rango de vision
        if (Vector3.Distance(_targetPosition, transform.position) > 0.3f) {
            _stateMachine.State = EnemyActionState.Running;
            MoveToTarget();
        } else {
            _stateMachine.State =  EnemyActionState.Idle;
            ChangeToStatus(DetectionState.Checking);
            return;
        }
        
    }

    private void DisabledBehaviour () {
        if (_timeCounter < _detection.DisableTime) {
            _timeCounter += Time.deltaTime;
            return;
        } 
        
        Debug.Log("Waking up... ");

        _health.RestoreHealth(_health.MaxHealth);
        _stateMachine.State = EnemyActionState.WakeUp;
        ChangeToStatus(DetectionState.OnPatrol);
    }

    public void DisableEnemy () {
        _stateMachine.State = EnemyActionState.Unconscious;
        ChangeToStatus(DetectionState.Disabled);
    }

    private void MoveToTarget () {
        Vector3 moveTo = new Vector3(
                                _targetPosition.x - transform.position.x,
                                0f,
                                _targetPosition.z - transform.position.z);

        _controller.Move(moveTo.normalized * _speed * Time.deltaTime);
        RotateTo(moveTo);
    }

    private void RotateTo (Quaternion targetRotation) {
        transform.rotation = Quaternion.Slerp(
                                            transform.rotation,
                                            targetRotation,
                                            _rotationSpeed * Time.deltaTime);
    }

    private void RotateTo (Vector3 targetLocation) {
        transform.forward = Vector3.Slerp(
                                        transform.forward,
                                        targetLocation,
                                        _rotationSpeed * Time.deltaTime);
    }

    private void RotateToTarget () {
        transform.forward = _targetPosition - transform.position;
    }

    private void TargetNextPatrolLocation () {
        if (++_indexLocation >= _patrolLocations.Count) {
            _indexLocation = 0;
        }
        _targetPosition = _patrolLocations[_indexLocation].position;
    }

    private void ChangeToStatus (DetectionState status) {
        _detection.State = status;

        switch (status) {
            case DetectionState.OnPatrol:
                Debug.Log("On patrol!...");
                _timeCounter = _detection.PatrolTime;
                _speed = _normalSpeed;
                _rotationSpeed = _patrolRotationSpeed;
                break;

            case DetectionState.OnCaution:
                _timeCounter = 0f;
                _speed = _normalSpeed;
                break;

            case DetectionState.Checking:
                _timeCounter = 0f;
                _speed = 0f;
                break;

            case DetectionState.OnAlert:
                _timeCounter = 0f;
                _speed = _maxSpeed;
                _rotationSpeed = _alertRotationSpeed;
                break;

            case DetectionState.Disabled:
                _timeCounter = 0f;
                _speed = 0f;
                break;

            default:
                break;
        }
    }

    public void RespondToAlarm (Transform player) {
        if (player == null || _detection.State == DetectionState.Disabled) {
            return;
        }

        _player = player;
        ChangeToStatus(DetectionState.OnAlert);
    }

    public void BackToPatrol () {
        TargetNextPatrolLocation();
        ChangeToStatus (DetectionState.OnPatrol);
        _stateMachine.State = EnemyActionState.Idle;
    }

    private void OnTriggerEnter (Collider other) {
        if ( other.CompareTag("PatrolLocation") && 
             _detection.State == DetectionState.OnPatrol) {
            
            PatrolLocation location = other.gameObject.GetComponent<PatrolLocation>();
            _detection.PatrolTime   = location.PatrolTime;
            //_patrolTime             = location.PatrolTime;
            _patrolRotation         = location.transform.rotation;

            TargetNextPatrolLocation();

            _stateMachine.State = EnemyActionState.Idle;
            _timeCounter = 0f;
        }
    }

    private void OnCollisionEnter (Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            switch (_detection.State) {
                case DetectionState.OnPatrol:
                    ChangeToStatus(DetectionState.OnCaution);
                    break;

                case DetectionState.OnCaution:
                    ChangeToStatus(DetectionState.OnAlert);
                    break;

                default:
                    break;
            }
            _player = collision.transform;
        }
    }
}