using UnityEngine;

public class GroundDetection : MonoBehaviour {
    [Header("Ground detection")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundDistance = 0.4f;
    [SerializeField] private LayerMask _groundLayer;

    [Header("Debug")]
    [SerializeField] private bool _grounded;

    public bool IsGrounded {
        get { return _grounded; }
        private set { _grounded = value; }
    }

    void Start() {
        if(_groundCheck == null) {
            Debug.LogError("Ground Detection: Ground Check Obj not found.");
        }
    }

    void Update() {
        GroundCheck();
    }

    public void GroundCheck() {
        _grounded = Physics.CheckSphere(
                                _groundCheck.position,
                                _groundDistance,
                                _groundLayer);
    }

    private void OnDrawGizmos() {
        //Dibuja la esfera que checkea el contacto con el suelo
        Gizmos.color = (_grounded) ? Color.green : Color.red;
        Gizmos.DrawSphere(_groundCheck.position, _groundDistance);
    }
}
