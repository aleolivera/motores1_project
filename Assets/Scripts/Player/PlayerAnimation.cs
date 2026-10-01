using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {
    private const string IS_SNEAKING    = "IsSneaking";
    private const string IS_SPRINTING   = "IsSprinting";
    private const string TRG_ATTACK     = "Attack";
    private const string TRG_GATHER     = "Gather";
    private const string TRG_DEATH      = "Death";

    private Animator _animator;
    private PlayerAttack _attack;

    private void OnEnable () {
        PlayerStateMachine.OnStateChangedToAttack       += PlayAttack;
        PlayerStateMachine.OnStateChangedToDead         += PlayDeath;
        PlayerStateMachine.OnStateChangedToIdle         += PlayIdle;
        PlayerStateMachine.OnStateChangedToInteracting  += PlayInteract;
        PlayerStateMachine.OnStateChangedToSprinting    += PlaySprinting;
        PlayerStateMachine.OnStateChangedToSneaking     += PlaySneaking;
    }
    private void OnDisable() {
        PlayerStateMachine.OnStateChangedToAttack       -= PlayAttack;
        PlayerStateMachine.OnStateChangedToDead         -= PlayDeath;
        PlayerStateMachine.OnStateChangedToIdle         -= PlayIdle;
        PlayerStateMachine.OnStateChangedToInteracting  -= PlayInteract;
        PlayerStateMachine.OnStateChangedToSprinting    -= PlaySprinting;
        PlayerStateMachine.OnStateChangedToSneaking     -= PlaySneaking;
    }

    void Start () {
        _animator   = GetComponent<Animator>();
        _attack     = GetComponentInParent<PlayerAttack>();
        
        if(_animator == null) 
            Debug.LogWarning("Player Animation: Animator not found");
        if (_attack == null) 
            Debug.LogWarning("Player Animation: Player Attack not found");
        
    }

    public void PlayIdle () {
        _animator.SetBool(IS_SNEAKING, false);
        _animator.SetBool(IS_SPRINTING, false);
    }

    public void PlaySneaking () {
        if (_animator.GetBool(IS_SNEAKING)) return;

        _animator.SetBool(IS_SNEAKING, true);
        _animator.SetBool(IS_SPRINTING, false);
    }

    public void PlaySprinting () {
        if (_animator.GetBool(IS_SPRINTING)) return;

        _animator.SetBool(IS_SPRINTING, true);
        _animator.SetBool(IS_SNEAKING, false);
    }

    public void PlayAttack () {
        _animator.SetTrigger(TRG_ATTACK);
        PlayerStateMachine.Instance.CurrentState = PlayerState.Idle;
    }
    public void PlayInteract () {
        _animator.SetTrigger(TRG_GATHER);
        PlayerStateMachine.Instance.CurrentState = PlayerState.Idle;
    }

    public void PlayDeath() {
        _animator.SetTrigger(TRG_DEATH);
    }

    public void EnableWeaponTrigger(int enabled) {
        if (enabled == 0) {
            _attack.EnableMeleeWeaponCollider(false);
        } else {
            _attack.EnableMeleeWeaponCollider(true);
        }
    }
}
