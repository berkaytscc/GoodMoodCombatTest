using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class DummyHealthBar : UIElement
{
    [Tooltip("Drag in your DummyPuppet instance here")]
    [SerializeField] private DummyPuppet _dummyPuppet;

    private Slider _slider;

    protected override void Start()
    {
        base.Start();
        UIManager.Instance.RegisterUIElement(this);
    }

    public override void Initialize()
    {
        _slider = GetComponent<Slider>();

        if (_dummyPuppet == null)
        {
            Debug.LogError("DummyHealthBar: no DummyPuppet assigned", this);
            gameObject.SetActive(false);
            return;
        }

        _slider.maxValue = _dummyPuppet.MaxHealth;
        _slider.value = _dummyPuppet.CurrentHealth;

        _dummyPuppet.OnHealthChanged += HandleHealthChanged;
    }

    private void HandleHealthChanged(float current, float max)
    {
        _slider.value = current;
    }

    private void OnDestroy()
    {
        if (_dummyPuppet != null)
            _dummyPuppet.OnHealthChanged -= HandleHealthChanged;
    }
}
