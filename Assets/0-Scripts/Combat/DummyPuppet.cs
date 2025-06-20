﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

[RequireComponent(typeof(Animator), typeof(AudioSource))]
public class DummyPuppet : MonoBehaviour, IDamageable
{
    public event Action<float, float> OnHealthChanged;
    public float MaxHealth => _maxHealth;
    public float CurrentHealth => _currentHealth;

    [Header("Health Settings")]
    [Tooltip("Maximum HP of the dummy")]
    [SerializeField] private float _maxHealth = 100f;
    [Tooltip("Seconds to wait before auto-respawn")]
    [SerializeField] private float _respawnDelay = 5f;

    [Header("SFX")]
    [SerializeField] private AudioClip _hitSFX;
    [SerializeField] private AudioClip _deathSFX;

    [Header("Damage Popup")]
    [SerializeField] private GameObject _damageNumberPrefab;
    [SerializeField] private Transform _damageNumberSpawnPoint;

    [Header("UI (World‐Space)")]
    [SerializeField] private Slider _healthBarSlider;

    private float _currentHealth;
    private Animator _anim;
    private AudioSource _audio;
    private Vector3 _startPos;
    private Quaternion _startRot;
    private Collider[] _colliders;
    private Renderer[] _renderers;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
        _startPos = transform.position;
        _startRot = transform.rotation;
        _colliders = GetComponentsInChildren<Collider>();
        _renderers = GetComponentsInChildren<Renderer>();

        _currentHealth = _maxHealth;
        _anim.SetFloat("CurrentHealth", _currentHealth);
        if (_healthBarSlider != null)
        {
            _healthBarSlider.maxValue = _maxHealth;
            _healthBarSlider.value = _currentHealth;
        }
    }

    public bool TryTakeDamage(float amount)
    {
        if (_currentHealth <= 0f) return false;

        _currentHealth = Mathf.Max(_currentHealth - amount, 0f);

        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);

        if (_healthBarSlider != null)
            _healthBarSlider.value = _currentHealth;

        if (_damageNumberPrefab != null && _damageNumberSpawnPoint != null)
        {
            var go = Instantiate(
                _damageNumberPrefab,
                _damageNumberSpawnPoint.position,
                Quaternion.identity);
            if (go.TryGetComponent<FloatingDamage>(out var popup))
                popup.SetValue(amount);
        }

        if (_hitSFX != null) _audio.PlayOneShot(_hitSFX);

        _anim.SetTrigger("Hit");

        _anim.SetFloat("CurrentHealth", _currentHealth);
        _anim.SetBool("CanRespawn", false);
        if (_currentHealth <= 0f)
            Die();

        return true;
    }

    private void Die()
    {
        if (_deathSFX != null) _audio.PlayOneShot(_deathSFX);

        _anim.SetTrigger("Die");

        if (_healthBarSlider != null)
            _healthBarSlider.gameObject.SetActive(false);

        StartCoroutine(RespawnRoutine());
    }

    private IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(_respawnDelay);
        _anim.SetBool("CanRespawn", true);

        transform.position = _startPos;
        transform.rotation = _startRot;

        _currentHealth = _maxHealth;
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);

        if (_healthBarSlider != null)
        {
            _healthBarSlider.gameObject.SetActive(true);
            _healthBarSlider.value = _currentHealth;
        }

        foreach (var c in _colliders) c.enabled = true;
        foreach (var r in _renderers) r.enabled = true;
    }
}
