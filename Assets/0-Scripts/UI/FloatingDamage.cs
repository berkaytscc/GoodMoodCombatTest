using System.Collections;
using TMPro;
using UnityEngine;

public class FloatingDamage : MonoBehaviour
{
    [Tooltip("How fast the number floats upward")]
    public float floatSpeed = 1f;

    [Tooltip("How long before it disappears")]
    public float duration = 1f;

    [Tooltip("Maximum random radius in XZ plane")]
    public float randomRadius = 0.5f;

    [Tooltip("Always faces the camera")]
    public bool _faceToCamera;

    private TextMeshPro _tmp;
    private Color _originalColor;

    void Awake()
    {
        _tmp = GetComponent<TextMeshPro>();
        _originalColor = _tmp.color;
    }

    /// <summary>
    /// Set the damage value and kick off the animation.
    /// </summary>
    public void SetValue(float amount)
    {
        _tmp.text = Mathf.RoundToInt(amount).ToString();

        Vector2 rand = Random.insideUnitCircle * randomRadius;
        transform.position += new Vector3(rand.x, 0f, rand.y);

        StartCoroutine(FloatAndFade());
    }

    private IEnumerator FloatAndFade()
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            FaceToCamera();

            transform.position += Vector3.up * (floatSpeed * Time.deltaTime);

            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            _tmp.color = new Color(
                _originalColor.r,
                _originalColor.g,
                _originalColor.b,
                alpha
            );

            elapsed += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }

    private void FaceToCamera()
    {
        if (_faceToCamera && Camera.main != null)
        {
            transform.LookAt(Camera.main.transform);
            transform.Rotate(0f, 180f, 0f);
        }
    }
}
