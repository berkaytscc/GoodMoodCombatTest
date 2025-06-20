using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SwordHitBox : MonoBehaviour
{
    [Tooltip("Base damage for this swing—can be overridden per combo stage.")]
    public float BaseDamage = 10f;

    [Tooltip("Layers this hitbox can damage (e.g. Dummy, Enemy)")]
    public LayerMask DamageLayer;

    private Collider _collider;
    private HashSet<Collider> _alreadyHit = new HashSet<Collider>();
    float _currentDamage;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
        _currentDamage = BaseDamage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_alreadyHit.Contains(collision.collider)) return;
        _alreadyHit.Add(collision.collider);

        if ((DamageLayer.value & (1 << collision.gameObject.layer)) == 0) return;

        if (collision.gameObject.TryGetComponent<IDamageable>(out var d))
        {
            d.TakeDamage(_currentDamage);
        }
    }

    public void SetDamage(float damage)
    {
        _currentDamage = damage;
    }

    public void EnableHitBox()
    {
        _alreadyHit.Clear();
        _collider.enabled = true;
    }

    public void DisableHitBox()
    {
        _collider.enabled = false;
    }
}
