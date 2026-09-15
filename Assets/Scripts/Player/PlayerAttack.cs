using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputListener))]
public class PlayerAttack : MonoBehaviour {
    [Header("Attack")]
    [SerializeField] private int _damage = 10;
    [SerializeField] private int _normalDamage = 10;
    [SerializeField] private float _attackCooldown = 2f;
    [SerializeField] private float _cooldownCounter;

    PlayerInputListener _input;
    
    /*
    [Header("Components")]
    [SerializeField] private PlayerInputListener _input;
    [SerializeField] private AudioClip _attackSound;
    [SerializeField] private Animator _animator;
    */

    private void Awake() {
        /*
        if(_animator == null)
            _animator = GetComponentInParent<Animator>();
        */
        _damage = _normalDamage;
        _cooldownCounter = _attackCooldown;
    }
    void Start() { 
        _input = GetComponent<PlayerInputListener>();
        if(_input == null) {
            Debug.LogWarning("PlayerAttack: PlayerInputListener not found");
        }
    }

    // Update is called once per frame
    void Update() {
        if(CanAttack()) {
            handleAttack();
        }
    }

    public void handleAttack() {

        /*
        if(_input.Attack) {
            _animator.SetBool("attack", true);
            //TODO: Reproducir audio clip
        } 
        else {
            _animator.SetBool("attack", true);
        }
        */
    }

    private bool CanAttack() {
        if(_cooldownCounter < _attackCooldown) {
            _cooldownCounter += Time.deltaTime;
            return false;
        }
        return true;
    }

    
}
