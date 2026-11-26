using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class WifeMovement : MonoBehaviour {
    [Header("Movement Settings")]
    [SerializeField] private float _normalSpeed = 2f;
    [SerializeField] private float _maxSpeed = 5f;

    [Header("Debug")]
    [SerializeField] private float _speed;
    
    //COMPONENT
    NavMeshAgent _agent;

    //METHODS
    void Awake () => _speed = _normalSpeed;
    void Start() => _agent = GetComponent<NavMeshAgent>();
    
    public void Sprint(Vector3 position) {
        _speed = _maxSpeed;
        Move(position);
    }

    public void Run (Vector3 position) {
        _speed = _normalSpeed;
        Move(position);
    }

    public void Stop () {
        _agent.isStopped = true;
        _agent.speed = 0f;
        _agent.SetDestination(transform.position);
    }
    
    private void Move(Vector3 position) {
        _agent.isStopped = false;
        _agent.speed = _speed;
        _agent.SetDestination(position);
    }
}
