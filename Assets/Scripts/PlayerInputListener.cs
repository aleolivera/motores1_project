using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputListener : MonoBehaviour {
    private PlayerInput input;
    public Vector2 MoveTo { get; private set; }
    public bool Jump { get; private set; }
    public Vector2 Rotation { get; private set; }
    public Vector2 Look { get; private set; }
    public bool Sprint { get; private set; }

    void Start() {
        input = GetComponent<PlayerInput>();
        if(input == null) {
            Debug.LogError("PlayerInputHandler: PlayerInput not found.");
        }
    }

    public void OnMovement(InputAction.CallbackContext ctx) {
        MoveTo = ctx.ReadValue<Vector2>();
    }
    public void OnJump(InputAction.CallbackContext ctx) {
        if(ctx.performed)       { Jump = true; } 
        else if(ctx.canceled)   { Jump = false; }
    }

    public void OnSprint(InputAction.CallbackContext ctx) {
        if(ctx.performed)       { Sprint = true; } 
        else if(ctx.canceled)   { Sprint = false; }
    }
}
