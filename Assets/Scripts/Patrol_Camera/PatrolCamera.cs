using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum PatrolCameraState { Stopped, Rotating, OnAlert }
public class PatrolCamera : MonoBehaviour {
    [Header("Target Rotations")]
    [SerializeField] private Transform _patrolCameraObj;
    [SerializeField] private float _rotationAngleRange = 90f;
    [SerializeField] private float _rotationSpeed= 5f;
    [SerializeField] private float _stopTime = 3f;
    [SerializeField] private float _alertTime = 3f;
    [SerializeField] private float _alarmRadious = 12f;

    [Header("Debug")]
    [SerializeField] private int _currentRotationIndex;
    [SerializeField] private List<Quaternion> _rotations;
    [SerializeField] private Quaternion _targetRotation;
    [SerializeField] private Vector3 _playerPosition;
    [SerializeField] private float _timeCounter;
    [SerializeField] private PatrolCameraState _state;
    [SerializeField] private bool _triggerAlarm = false;


    SphereCollider _collider;
    VisionDetection _detection;

    private void Awake() {
        _state          = PatrolCameraState.Stopped;
        _timeCounter    = _stopTime;
        _playerPosition = Vector3.zero;
    }
    void Start() {
        _detection  = GetComponent<VisionDetection>();
        _collider   = GetComponent<SphereCollider>();
        
        if(_detection == null) 
            Debug.LogError("PatrolCamera: Vision detection not found");
        
        if(_collider == null) 
            Debug.LogError("PatrolCamera: Collider not found");

        if(_patrolCameraObj == null) 
            Debug.LogError("PatrolCamera: Camera Object not found");
        
        PatrolCameraConfig();
        _detection.GizmoColor = Color.green;

    }
    void PatrolCameraConfig() {
        float limitRotationAngle = _rotationAngleRange / 2;
        _rotations.Add(
            Quaternion.Euler(
                _patrolCameraObj.localRotation.x,
                _patrolCameraObj.localRotation.y + limitRotationAngle,
                _patrolCameraObj.localRotation.z));
        _rotations.Add(
            Quaternion.Euler(
                _patrolCameraObj.localRotation.x,
                _patrolCameraObj.localRotation.y - limitRotationAngle,
                _patrolCameraObj.localRotation.z));

        _patrolCameraObj.localRotation = _rotations[0];

        _collider.radius = _alarmRadious;
    }

    void Update() {
        switch(_state) {
            case PatrolCameraState.Stopped:     Stopped();  break;
            case PatrolCameraState.Rotating:    Rotating(); break;
            case PatrolCameraState.OnAlert:     OnAlert();  break;
            
            default: break;
        }
    }

    private void Stopped() {
        Vector3 playerPosition = _detection.CheckVision();
        if(playerPosition != Vector3.zero) {
            ChangeStateTo(PatrolCameraState.OnAlert);
            return;
        } else {
            PlayerNotFound();
        }

        if(_timeCounter < _stopTime) {  
            _timeCounter += Time.deltaTime;
        } else {
            ChangeToNextTargetTransform();
            ChangeStateTo(PatrolCameraState.Rotating);
        }
    }
    private void Rotating() {
        if(_detection.CheckVision() != Vector3.zero) {
            ChangeStateTo(PatrolCameraState.OnAlert);
            return;
        } else {
            PlayerNotFound();
        }

        if(_targetRotation == _patrolCameraObj.localRotation) {
            ChangeStateTo(PatrolCameraState.Stopped);
            return;
        }

        RotateToTarget(true);
    }

    void ChangeToNextTargetTransform() {
        if(++_currentRotationIndex >= _rotations.Count) {
            _currentRotationIndex = 0;
        }
        _targetRotation = _rotations[_currentRotationIndex];
    }

    private void OnAlert() {
        Vector3 playerPosition = _detection.CheckVision();
        if(playerPosition != Vector3.zero) {
            _timeCounter = 0f;
            _triggerAlarm = true;
            PlayerFound(playerPosition);
            //_patrolCameraObj.LookAt(playerPosition);
            //RotateTo(playerPosition);
        } else {
            PlayerNotFound();
            RotateToTarget(true);
        }

        if(_timeCounter < _alertTime) {
            _timeCounter += Time.deltaTime;
        } else {
            _triggerAlarm = false;
            ChangeStateTo(PatrolCameraState.Stopped);
        }
    }

    private void PlayerNotFound() {
        _playerPosition = Vector3.zero;
    }
    private void PlayerFound(Vector3 position) {
        _playerPosition = position;
    }

    private void ActivateAlarm(EnemyIA enemy) {
        enemy.RespondToAlarm(_playerPosition);
    }

    void ChangeStateTo(PatrolCameraState state) {
        switch(state) {
            case PatrolCameraState.OnAlert: 
                _detection.GizmoColor = Color.red;
                break;
            default:
                _detection.GizmoColor = Color.green;
                break;
        }

        _state = state;
        _timeCounter = 0f;
    }
    private void RotateTo(Vector3 position) {
        Vector3 look = position - _patrolCameraObj.position;
        look.y = 0;
        
        if(look == Vector3.zero) return;
        Quaternion rotation = Quaternion.LookRotation(look);
        
        _patrolCameraObj.localRotation = 
                    Quaternion.Slerp(_patrolCameraObj.localRotation, rotation, _rotationSpeed * Time.deltaTime);

        /*
        Vector3 to = Vector3.up * position.x;
        
        if(to.y > _rotations[0].y ) {
            to.y = _rotations[0].y;
        } else if(to.y < _rotations[1].y) {
            to.y = _rotations[1].y;
        }
        _patrolCameraObj.forward = Vector3.Slerp(
                                        _patrolCameraObj.forward,
                                        to,
                                        _rotationSpeed * Time.deltaTime);
           */ 
    }

    private void RotateToTarget(bool applySoftRotation) {
        _patrolCameraObj.localRotation = Quaternion.Slerp(
                                            _patrolCameraObj.localRotation,
                                            _targetRotation,
                                            (applySoftRotation) ? _rotationSpeed * Time.deltaTime : 1f);
    }

    private void OnTriggerStay(Collider collider) {
        //Debug.Log("Collider: " + collider.name + " state: " + _state + "alarm: " +_triggerAlarm);

        if(collider.gameObject.CompareTag("Enemy") && 
            _state == PatrolCameraState.OnAlert && 
            _triggerAlarm) {
            Debug.Log("Enemy alerted!");
            EnemyIA enemy = collider.gameObject.GetComponent<EnemyIA>();
            ActivateAlarm(enemy);
        }
    }

    private void OnDrawGizmos() {
        
        switch(_state) {
            case PatrolCameraState.OnAlert:
                Gizmos.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0.3f);
                break;
            default:
                Gizmos.color = new Color(Color.green.r, Color.green.g, Color.green.b, 0.3f);
                break;
        }

        Gizmos.DrawSphere(transform.position, _alarmRadious);
    }
}
