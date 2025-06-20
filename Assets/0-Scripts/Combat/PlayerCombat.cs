using UnityEngine;

[RequireComponent(typeof(InputBridge), typeof(Animator))]
public class PlayerCombat : MonoBehaviour
{
    [Header("Combo Settings")]
    [Tooltip("Number of hits in your combo")]
    [SerializeField] private int _maxComboStage = 3;

    [SerializeField] private SwordHitBox _swordHitBox;

    // internal state
    private InputBridge _input;
    private Animator _anim;
    private int _comboStage;
    private bool _isAttacking;
    private bool _comboBuffered;
    private bool _comboWindowOpen;

    private void Awake()
    {
        _input = GetComponent<InputBridge>();
        _anim = GetComponent<Animator>();
    }

    private void OnEnable() => _input.OnAttackPerformed += HandleAttackInput;
    private void OnDisable() => _input.OnAttackPerformed -= HandleAttackInput;

    public void OnComboWindowOpen()
    {
        _comboWindowOpen = true;
        Debug.Log("[Combat] Combo window OPEN for stage " + _comboStage);
    }

    public void OnAttackAnimationComplete()
    {
        Debug.Log("[Combat] AttackAnimationComplete called. buffered=" + _comboBuffered);
        if (_comboBuffered)
        {
            _comboBuffered = false;
            _comboWindowOpen = false;
            _comboStage = Mathf.Min(_comboStage + 1, _maxComboStage);
            _anim.ResetTrigger("Attack" + (_comboStage - 1));
            _anim.SetTrigger("Attack" + _comboStage);
            Debug.Log($"[Combat] Advancing to Attack{_comboStage}");
        }
        else
        {
            Debug.Log("[Combat] No buffer → ResetCombo");
            ResetCombo();
        }
    }

    public void EnableSwordHitBoxExternal()
    {
        if (_comboStage == _maxComboStage)
        {
            _swordHitBox.SetDamage(_swordHitBox.BaseDamage * 2f);
        }
        else
        {
            _swordHitBox.SetDamage(_swordHitBox.BaseDamage);
        }

        _swordHitBox.EnableHitBox();
    }

    public void DisableSwordHitBoxExternal()
    {
        _swordHitBox.DisableHitBox();
    }

    private void HandleAttackInput()
    {
        Debug.Log($"[Combat] AttackInput received. isAttacking={_isAttacking}, stage={_comboStage}, windowOpen={_comboWindowOpen}, buffered={_comboBuffered}");
        if (!_isAttacking)
        {
            _comboStage = 1;
            _isAttacking = true;
            _anim.SetBool("IsAttacking", true);
            _anim.SetTrigger("Attack1");
            Debug.Log("[Combat] Starting Attack1");
        }
        else if (_comboWindowOpen && !_comboBuffered && _comboStage < _maxComboStage)
        {
            _comboBuffered = true;
            Debug.Log("[Combat] Combo buffered for next stage");
        }
    }

    private void ResetCombo()
    {
        _comboStage = 0;
        _isAttacking = false;
        _comboBuffered = false;
        _comboWindowOpen = false;
        _anim.SetBool("IsAttacking", false);
    }
}
