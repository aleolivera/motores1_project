using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInputListener))]
public class PlayerMovement : MonoBehaviour {
    private PlayerInputListener _input;
    private CharacterController _controller;

    [Header("Move speed")]
    [SerializeField] private float _normalSpeed = 5f;
    [SerializeField] private float _sprintSpeed = 10f;

    [Header("Rotation")]
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private Transform _orientation;

    [Header("Ground detection")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundDistance = 0.4f;
    [SerializeField] private LayerMask _groundLayer;

    [Header("Jump and Gravity")]
    [SerializeField] private float _gravity = -9.8f;
    [SerializeField] private float _jumpForce = 5f;

    [Header("Debug Player States")]
    [SerializeField] private float _speed;
    [SerializeField] private float _verticalVelocity;
    [SerializeField] private bool _grounded;

    void Awake() {
        _input = GetComponent<PlayerInputListener>();
        if(_input == null ) {
            Debug.LogError("PlayerMovement: PlayerInputHandler not found.");
        }

        _controller = GetComponent<CharacterController>();
        if(_controller == null) {
            Debug.LogError("PlayerMovement: CharacterController not found.");
        }

        _orientation = GetComponent<Transform>();
        if(_orientation == null) {
            Debug.LogError("PlayerMovement: Orientation Obj not found.");
        }

        _groundCheck = GetComponent<Transform>();
        if(_groundCheck == null) {
            Debug.LogError("PlayerMovement: Ground Check Obj not found.");
        }
    }

    private void Start() {
        _speed = _normalSpeed;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update() {
        //Ground check and gravity
        GroundCheck();
        ApplyGravity();

        //Apply player movement
        HandleMovement();
        HandleRotation();
        HandleJump();
        HandleSprint();
        HandleAttack();
        HandleInteract();

        //Debug gizmos
        DrawDebugLines();
    }

    public void HandleSprint() {
        if (_input.Sprint)  { _speed = _sprintSpeed; } 
        else                { _speed = _normalSpeed; }
    }

    public void HandleJump() {
        if(_grounded && _input.Jump) {
            _verticalVelocity = Mathf.Sqrt(Mathf.Abs( -2f * _gravity * _jumpForce));
        }
    }
    public void HandleAttack() {
        if(_input.Attack) {
            Debug.LogWarning("Attack not implemented");
        }
    }
    public void HandleInteract() {
        if(_input.Interact) {
            Debug.LogWarning("Interact not implemented");
        }
    }

    public void HandleRotation() {
        Vector3 inputDirection =
                (_orientation.forward * _input.MoveTo.y) +
                (_orientation.right * _input.MoveTo.x);
        
        if(inputDirection != Vector3.zero) {
            transform.forward = Vector3.Slerp(
                                        transform.forward,
                                        inputDirection.normalized,
                                        _rotationSpeed * Time.deltaTime);
        }
    }

    public void HandleMovement() {
        Vector3 move = transform.forward * _input.MoveTo.y;
        move = move.normalized * _speed;

        move.y = _verticalVelocity;
        _controller.Move(move * Time.deltaTime);

    }

    public void ApplyGravity() {
        //Aplica gravedad al player
        if(_grounded && _verticalVelocity < 0) {
            _verticalVelocity = -2f;
        }

        _verticalVelocity += _gravity * Time.deltaTime;
    }

    public void GroundCheck() {
        _grounded = Physics.CheckSphere(
                                _groundCheck.position,
                                _groundDistance,
                                _groundLayer);
    }

    private void DrawDebugLines() {
        Vector3 from = (_orientation.position + _orientation.up);
        Vector3 to = from + _orientation.forward * 10f;

        Debug.DrawLine(from, to, Color.green);
    }


    private void OnDrawGizmos() {
        //Dibuja la esfera que checkea el contacto con el suelo
        Gizmos.color = (_grounded) ? Color.green : Color.red;
        Gizmos.DrawSphere(_groundCheck.position, _groundDistance);
    }

    
}
