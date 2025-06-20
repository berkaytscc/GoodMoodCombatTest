using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SwordHitBox : MonoBehaviour
{
    public float BaseDamage = 10f;
    public LayerMask DamageLayer;

    [SerializeField] private ParticleSystem _basicHitParticle;

    private Collider _collider;
    private HashSet<Collider> _alreadyHit = new HashSet<Collider>(); // to avoid duplicates
    float _currentDamage;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
        _currentDamage = BaseDamage;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (_alreadyHit.Contains(collision.collider)) return;
        _alreadyHit.Add(collision.collider);

        if ((DamageLayer.value & (1 << collision.gameObject.layer)) == 0) return;

        if (collision.gameObject.TryGetComponent<IDamageable>(out var d))
        {
            if(d.TryTakeDamage(_currentDamage))
            {
                ParticleSystem ps = Instantiate(
                _basicHitParticle,
                collision.contacts[0].point,
                Quaternion.identity
                );

                ps.Play();
                StartCoroutine(DestroyParticleWhenFinished(ps));
            }
        }
    }

    private IEnumerator DestroyParticleWhenFinished(ParticleSystem ps)
    {
        var main = ps.main;
        float total = main.duration + main.startLifetime.constantMax;
        yield return new WaitForSeconds(total);

        Destroy(ps.gameObject);
    }
}
