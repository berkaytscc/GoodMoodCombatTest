using UnityEngine;

[RequireComponent(typeof(InputBridge), typeof(Animator))]
public class PlayerCombat : MonoBehaviour
{
    [Header("Combo Settings")]
    [Tooltip("Number of hits in your combo")]
    [SerializeField] private int maxComboStage = 3;

    // internal state
    private InputBridge _input;
    private Animator _anim;
    private int _comboStage;       // 0 = not attacking, 1..max = current attack
    private bool _isAttacking;      // true from first swing until final animation end
    private bool _comboBuffered;    // did we click during the window?
    private bool _comboWindowOpen;  // are we currently accepting a click?

    private void Awake()
    {
        _input = GetComponent<InputBridge>();
        _anim = GetComponent<Animator>();
    }

    private void OnEnable() => _input.OnAttackPerformed += HandleAttackInput;
    private void OnDisable() => _input.OnAttackPerformed -= HandleAttackInput;

    private void HandleAttackInput()
    {
        Debug.Log($"[Combat] AttackInput received. isAttacking={_isAttacking}, stage={_comboStage}, windowOpen={_comboWindowOpen}, buffered={_comboBuffered}");
        if (!_isAttacking)
        {
            _comboStage = 1;
            _isAttacking = true;
            _anim.SetTrigger("Attack1");
            Debug.Log("[Combat] Starting Attack1");
        }
        else if (_comboWindowOpen && !_comboBuffered && _comboStage < maxComboStage)
        {
            _comboBuffered = true;
            Debug.Log("[Combat] Combo buffered for next stage");
        }
    }

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
            _comboStage = Mathf.Min(_comboStage + 1, maxComboStage);
            _anim.SetTrigger("Attack" + _comboStage);
            Debug.Log($"[Combat] Advancing to Attack{_comboStage}");
        }
        else
        {
            Debug.Log("[Combat] No buffer → ResetCombo");
            ResetCombo();
        }
    }



    private void ResetCombo()
    {
        _comboStage = 0;
        _isAttacking = false;
        _comboBuffered = false;
        _comboWindowOpen = false;
    }
}
