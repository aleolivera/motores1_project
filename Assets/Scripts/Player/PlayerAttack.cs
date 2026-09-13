using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputListener))]
public class PlayerAttack : MonoBehaviour {
    [Header("Attack")]
    [SerializeField] private int _damage = 10;
    [SerializeField] private int _normalDamage = 10;
    
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
    }
    void Start() {
        _damage = _normalDamage;
    }

    // Update is called once per frame
    void Update() {
        handleAttack();
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

    
}
