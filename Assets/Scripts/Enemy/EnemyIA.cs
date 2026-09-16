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

    [Header("Patrol")]
    [SerializeField] private List<Transform> _patrolLocations;

    [Header("Vision")]
    [SerializeField] private float _checkDistance = 16f;
    [SerializeField] private float _alertDistance = 8f;
    [SerializeField] private LayerMask _detectionLayers;
    [SerializeField] private LayerMask _playerLayer;
    [SerializeField] private List<Transform> _visionLines = new List<Transform>(9);

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
    private EnemyHealth _health;

    private Color _lineColor = Color.green;

    public float PatrolTime {
        get { return _patrolTime; }
        set { _patrolTime = value; }
    }
    public EnemyStatus Status {
        get { return _status; }
        private set { _status = value; }
    }

    void Awake() {
        _speed = _normalSpeed;
        _timeCounter = _patrolTime;
        _patrolPosition = transform.position;
    }

    void Start() {
        _controller = GetComponent<CharacterController>();
        _health = GetComponent<EnemyHealth>();

        if(_patrolLocations.Count == 0) {
            _targetPosition = transform.position;
            Debug.Log("EnemyMovement: Patrol location is empty.");
        } else {
            _targetPosition = _patrolLocations[_indexLocation].position;
        }

        if(_visionLines.Count == 0)
            Debug.Log("EnemyMovement: Vision lines is empty.");

        if(_controller == null)
            Debug.Log("EnemyMovement: Character Controller not found.");
        if(_health == null)
            Debug.Log("EnemyMovement: Enemy health not found.");
    }

    void Update() {

        switch(_status) {
            case EnemyStatus.OnPatrol:      PatrolBehaviour();      break;
            case EnemyStatus.CheckingOut:   CheckingOutBehaviour(); break;
            case EnemyStatus.OnAlert:       AlertBehaviour();       break;
            case EnemyStatus.Disabled:      DisabledBehaviour();    break;

            default: _status = EnemyStatus.OnPatrol; break;
        }

        //DrawVisionLine();

    }
    private Vector3 CheckVisionLine() {
        bool hasHit = false;
        RaycastHit hit;
        foreach(Transform line in _visionLines) {
            hasHit = Physics.Raycast(
                                    line.position,
                                    line.forward,
                                    out hit,
                                    _checkDistance,
                                    _detectionLayers);
            if(hasHit) {
                int playerLayerHit = (_playerLayer.value & (1 << hit.collider.gameObject.layer));
                if(playerLayerHit != 0)
                    return hit.transform.position;
            }
        }
        return Vector3.zero;
    }

    private void PatrolBehaviour() {
        Vector3 playerPosition = CheckVisionLine();

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
        Vector3 playerPosition = CheckVisionLine();
        if(playerPosition != Vector3.zero) {
            _targetPosition = playerPosition;
            _timeCounter = 0f;

            if(Vector3.Distance(transform.position, _targetPosition) < _alertDistance) {
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
            _targetPosition = _patrolPosition;
            return;
        }
    }

    private void AlertBehaviour() {
        Vector3 playerPosition = CheckVisionLine();
        if(playerPosition != Vector3.zero) {
            _targetPosition = playerPosition;
            _timeCounter = 0f;
        }

        if(Vector3.Distance(_targetPosition, transform.position) > 1f) {
            MoveToTarget();
        }

        if(_timeCounter < _alertTime) {
            _timeCounter += Time.deltaTime;
        } else {
            Debug.Log("Player escaped!, On patrol... ");

            ChangeToStatus(EnemyStatus.OnPatrol);
            _targetPosition = _patrolPosition;
            return;
        }

    }

    private void DisabledBehaviour() {
        if(_timeCounter < _disableTime) {
            _timeCounter += Time.deltaTime;
        } else {
            Debug.Log("Waking up... On patrol");

            ChangeToStatus(EnemyStatus.OnPatrol);
            _health.RestoreFullHealth();
            _targetPosition = _patrolPosition;
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
        if(other.gameObject.tag.Equals("PatrolLocation") && _status == EnemyStatus.OnPatrol) {
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
            Debug.Log("Player collition: " + _status);
            switch(_status) {
                case EnemyStatus.OnPatrol:
                    ChangeToStatus(EnemyStatus.CheckingOut);
                    break;

                case EnemyStatus.CheckingOut:
                    ChangeToStatus(EnemyStatus.OnAlert);
                    break;

                case EnemyStatus.OnAlert:
                    //TODO: IMPLEMENTAR ATAQUE
                    Debug.Log("Attack!!...");

                    break;

                default:
                    break;
            }

            _targetPosition = collision.transform.position;
        }
    }

    private void ChangeToStatus(EnemyStatus status) {
        _status = status;

        switch(status) {
            case EnemyStatus.OnPatrol:
                _timeCounter = _patrolTime;
                _speed = _normalSpeed;
                _lineColor = Color.green;
                break;

            case EnemyStatus.CheckingOut:
                _timeCounter = 0f;
                _speed = _normalSpeed;
                _lineColor = Color.yellow;
                break;

            case EnemyStatus.OnAlert:
                _timeCounter = 0f;
                _speed = _maxSpeed;
                _lineColor = Color.red;
                break;

            case EnemyStatus.Disabled:
                _timeCounter = 0f;
                _speed = 0f;
                _lineColor = Color.white;
                break;

            default:
                break;
        }
    }

    private void OnDrawGizmos() {
        switch(_status) {
            case EnemyStatus.OnPatrol:
                Gizmos.color = Color.green; break;
            case EnemyStatus.CheckingOut:
                Gizmos.color = Color.yellow; break;
            case EnemyStatus.OnAlert:
                Gizmos.color = Color.red; break;
        }

        foreach(Transform line in _visionLines) {
            Gizmos.DrawLine(
                        line.position,
                        line.position + (line.forward * _checkDistance));
        }
    }
}
