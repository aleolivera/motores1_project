using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInputListener))]
[RequireComponent(typeof(GroundDetection))]
public class PlayerMovement : MonoBehaviour {
    private PlayerInputListener _input;
    private CharacterController _controller;
    private GroundDetection _groundCheck;

    [Header("Move speed")]
    [SerializeField] private float _normalSpeed = 5f;
    [SerializeField] private float _sprintSpeed = 10f;

    [Header("Rotation")]
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private Transform _orientation;
    /*
    [Header("Ground detection")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundDistance = 0.4f;
    [SerializeField] private LayerMask _groundLayer;
    */

    [Header("Jump and Gravity")]
    [SerializeField] private float _gravity = -9.8f;
    [SerializeField] private float _jumpForce = 5f;

    [Header("Debug Player States")]
    [SerializeField] private float _speed;
    [SerializeField] private float _verticalVelocity;
    [SerializeField] private bool _jumping;
    [SerializeField] private float _jumpingCooldown = 2f;
    [SerializeField] private float _jumpingCooldownCounter = 0f;

    void Awake() {
        _speed = _normalSpeed;
        _jumping = false;
        _jumpingCooldownCounter = _jumpingCooldown;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start() {
        _input = GetComponent<PlayerInputListener>();
        _controller = GetComponent<CharacterController>();
        _orientation = GetComponent<Transform>();
        _groundCheck = GetComponent<GroundDetection>();
        
        if(_input == null ) {
            Debug.LogError("PlayerMovement: PlayerInputHandler not found.");
        }
        if(_controller == null) {
            Debug.LogError("PlayerMovement: CharacterController not found.");
        }
        if(_orientation == null) {
            Debug.LogError("PlayerMovement: Orientation Obj not found.");
        }
        if(_groundCheck == null) {
            Debug.LogError("PlayerMovement: Ground Check Obj not found.");
        }
    }

    void Update() {
        //Ground check and gravity
        //GroundCheck();
        ApplyGravity();

        //Apply player movement
        HandleMovement();
        HandleRotation();
        HandleJump();
        HandleSprint();
        HandleInteract();
        
        JumpingCooldown();
    }

    public void HandleSprint() {
        if (_input.Sprint && _groundCheck.IsGrounded)  { 
            _speed = _sprintSpeed; 
        } 
        else { 
            _speed = _normalSpeed; 
        }
    }

    public void HandleJump() {
        if(_groundCheck.IsGrounded && _input.Jump) {
            _verticalVelocity = Mathf.Sqrt(Mathf.Abs( -2f * _gravity * _jumpForce));
            _jumpingCooldownCounter = 0f;
        }
    }
    
    public void HandleInteract() {
        if(_input.Interact) {
            Debug.LogWarning("Interact not implemented");
        }
    }

    public void HandleRotation() {
        if(_input.MoveTo == Vector2.up || _input.MoveTo == Vector2.down) {
            return;
        }

        Vector3 inputRotation =
                (_orientation.forward * _input.MoveTo.y) +
                (_orientation.right * _input.MoveTo.x);

        if(inputRotation != Vector3.zero) {
            transform.forward = Vector3.Slerp(
                                        transform.forward,
                                        inputRotation.normalized,
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
        if(_groundCheck.IsGrounded && _verticalVelocity < 0) {
            _verticalVelocity = -2f;
        }

        _verticalVelocity += _gravity * Time.deltaTime;
    }

    private void JumpingCooldown() {
        if(_jumpingCooldownCounter < _jumpingCooldown) {
            _jumpingCooldownCounter += Time.deltaTime;
            _jumping = false;
        } else {
            _jumping = true;
        }
    }

    private void OnDrawGizmos() {
        Vector3 from = _orientation.position;
        Vector3 to = from + _orientation.forward * 10f;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(from, to);
    }
}
