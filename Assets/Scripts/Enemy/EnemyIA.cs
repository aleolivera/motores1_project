using System.Collections.Generic;
using UnityEngine;
public enum DetectionState { OnPatrol, OnCaution, OnAlert, Disabled }
public enum MovementState { Idle, Walking, Running, Attacking, Unconscious }

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
    [SerializeField] private DetectionState _detectionState = DetectionState.OnPatrol;
    [SerializeField] private float _timeCounter = 0f;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private MovementState _movementState = MovementState.Idle;

    [SerializeField] private Vector3 _targetPosition;
    //[SerializeField] private Vector3 _patrolPosition;
    [SerializeField] private Quaternion _patrolRotation;
    [SerializeField] private int _indexLocation = 0;

    private CharacterController _controller;
    private EnemyHealth _health;
    private VisionDetection _detection;

    public float PatrolTime {
        get { return _patrolTime; }
        set { _patrolTime = value; }
    }
    public DetectionState DetectionState {
        get { return _detectionState; }
        private set { _detectionState = value; }
    }
    public MovementState MovementState {
        get { return _movementState; }
        set { _movementState = value; }
    }

    void Awake() {
        _speed = _normalSpeed;
        _timeCounter = _patrolTime;
        //_patrolPosition = transform.position;
        _rotationSpeed = _patrolRotationSpeed;
    }

    void Start() {
        _controller = GetComponent<CharacterController>();
        _health = GetComponent<EnemyHealth>();
        _detection = GetComponent<VisionDetection>();

        foreach(Transform t in _patrolLocations) {
            if(t == null)
                Debug.Log("EnemyMovement: Patrol location is empty.");
            else
                _targetPosition = transform.position;
        }

        _targetPosition = _patrolLocations[_indexLocation].position;

        if(_controller == null)
            Debug.Log("EnemyMovement: Character Controller not found.");

        if(_health == null)
            Debug.Log("EnemyMovement: Enemy health not found.");

        if(_detection == null)
            Debug.Log("EnemyMovement: Vision detection not found.");

        _detection.GizmoColor = Color.green;

        _movementState = MovementState.Idle;
    }

    void Update() {
        switch(_detectionState) {
            case DetectionState.OnPatrol: PatrolBehaviour(); break;
            case DetectionState.OnCaution: CheckingOutBehaviour(); break;
            case DetectionState.OnAlert: AlertBehaviour(); break;
            case DetectionState.Disabled: DisabledBehaviour(); break;

            default: _detectionState = DetectionState.OnPatrol; break;
        }
    }

    private void PatrolBehaviour() {
        Vector3 playerPosition = _detection.CheckVision();

        if(playerPosition != Vector3.zero) {
            _targetPosition = playerPosition;
            ChangeToStatus(DetectionState.OnCaution);
            return;
        }

        if(_timeCounter < _patrolTime) {
            _timeCounter += Time.deltaTime;

            if(transform.rotation.y != _patrolRotation.eulerAngles.y) {
                RotateTo(_patrolRotation);
            }

        } else {
            MoveToTarget();
            _movementState = MovementState.Walking;
        }
    }

    private void CheckingOutBehaviour() {
        Vector3 playerPosition = _detection.CheckVision();
        if(playerPosition != Vector3.zero) {
            _targetPosition = playerPosition;
            _timeCounter = 0f;

            if(Vector3.Distance(transform.position, _targetPosition) < _detection.AlertDistance) {
                Debug.Log("Player Found!, On alert!!");

                ChangeToStatus(DetectionState.OnAlert);
                return;
            }
        }

        if(Vector3.Distance(_targetPosition, transform.position) > 1f) {
            MoveToTarget();
            _movementState = MovementState.Walking;
        }

        if(_timeCounter < _checkTime) {
            _timeCounter += Time.deltaTime;
            return;
        }

        Debug.Log("Nothing to report, On patrol...");
        ChangeToStatus(DetectionState.OnPatrol);
        TargetNextPatrolLocation();
    }

    private void AlertBehaviour() {
        Vector3 playerPosition = _detection.CheckVision();
        if(playerPosition != Vector3.zero) {
            _targetPosition = playerPosition;
            _timeCounter = 0f;
        }

        if(Vector3.Distance(_targetPosition, transform.position) > 0.5f) {
            MoveToTarget();
            _movementState = MovementState.Running;
        } else {
            _movementState = MovementState.Idle;
        }

        if(_timeCounter < _alertTime) {
            _timeCounter += Time.deltaTime;
        } else {
            Debug.Log("Player escaped!... Back to patrol");

            ChangeToStatus(DetectionState.OnPatrol);
            TargetNextPatrolLocation();
        }
    }

    private void DisabledBehaviour() {
        if(_timeCounter < _disableTime) {
            _timeCounter += Time.deltaTime;
        } else {
            Debug.Log("Waking up... Back to patrol");

            ChangeToStatus(DetectionState.OnPatrol);
            _health.RestoreHealth(_health.MaxHealth);
            TargetNextPatrolLocation();
            _movementState = MovementState.Idle;
        }
    }

    public void DisableEnemy() {
        ChangeToStatus(DetectionState.Disabled);
        _movementState = MovementState.Unconscious;
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

    private void TargetNextPatrolLocation() {
        if(++_indexLocation >= _patrolLocations.Count) {
            _indexLocation = 0;
        }
        _targetPosition = _patrolLocations[_indexLocation].position;
    }

    private void ChangeToStatus(DetectionState status) {
        _detectionState = status;

        switch(status) {
            case DetectionState.OnPatrol:
                _timeCounter = _patrolTime;
                _speed = _normalSpeed;
                _rotationSpeed = _patrolRotationSpeed;
                _detection.GizmoColor = Color.green;
                break;

            case DetectionState.OnCaution:
                _timeCounter = 0f;
                _speed = _normalSpeed;
                _detection.GizmoColor = Color.yellow;
                break;

            case DetectionState.OnAlert:
                _timeCounter = 0f;
                _speed = _maxSpeed;
                _rotationSpeed = _alertRotationSpeed;
                _detection.GizmoColor = Color.red;
                break;

            case DetectionState.Disabled:
                _timeCounter = 0f;
                _speed = 0f;
                _detection.GizmoColor = Color.white;
                break;

            default:
                break;
        }
    }

    public void RespondToAlarm(Vector3 playerPosition) {
        if(playerPosition == Vector3.zero || _detectionState == DetectionState.Disabled) {
            return;
        }

        ChangeToStatus(DetectionState.OnAlert);
        _targetPosition = playerPosition;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("PatrolLocation") && _detectionState == DetectionState.OnPatrol) {
            PatrolLocation location = other.gameObject.GetComponent<PatrolLocation>();

            _patrolTime = location.PatrolTime;
            _timeCounter = 0f;

            //_patrolPosition = location.transform.position;
            _patrolRotation = location.transform.rotation;

            TargetNextPatrolLocation();

            _movementState = MovementState.Idle;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.tag.Equals("Player")) {
            switch(_detectionState) {
                case DetectionState.OnPatrol:
                    ChangeToStatus(DetectionState.OnCaution);
                    break;

                case DetectionState.OnCaution:
                    ChangeToStatus(DetectionState.OnAlert);
                    break;

                default:
                    break;
            }
            _targetPosition = collision.transform.position;
        }
    }
}
