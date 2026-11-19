using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public enum WifeMovementState { Idle, Running }

[RequireComponent(typeof(SphereCollider))]
public class WifeController : MonoBehaviour
{
    [Header("Wife Settings")]
    [SerializeField] private float _speed = 2f;
    [SerializeField] private Quest _questRef;
    public bool IsActive { get; set; }

    [Header("Debug")]
    [SerializeField] GameObject _playerRef;
    NavMeshAgent _agent;
    SphereCollider _collider;
    Animator _animator;
    
    public WifeMovementState State { get; set; }
    void Start(){
        _agent = GetComponent<NavMeshAgent>();
        _collider = GetComponent<SphereCollider>();
        _animator = GetComponentInChildren<Animator>();
        _collider.isTrigger = true;
    }
    private void Update () {
        if ( !IsActive || _playerRef == null) return;

        if(Vector3.Distance(transform.position, _playerRef.gameObject.transform.position) > 1f) {
            State = WifeMovementState.Running;
            MoveTowardPlayer();
            return;
        }

        StopMovement();
    }

    void MoveTowardPlayer () {
        _agent.isStopped = false;
        _agent.speed = _speed;
        _agent.SetDestination(_playerRef.transform.position);
        _animator.SetBool("IsRunning", true);

    }

    void StopMovement () {
        _agent.isStopped = true;
        _agent.speed = 0f;
        _agent.SetDestination(transform.position);
        _animator.SetBool("IsRunning", false);
    }

    // Update is called once per frame
    private void OnTriggerEnter (Collider other) {
        if (other.gameObject.CompareTag("Player") && _questRef.IsQuestCompleted) {
            _animator.SetBool("IsDisabled", false);
            _playerRef = other.gameObject;
        }
    }
}
