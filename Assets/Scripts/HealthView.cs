using System.Collections;
using TMPro;
using UnityEngine;

public class HealthView : MonoBehaviour
{
    [SerializeField] private float _smoothDecreaseDuration = 0.5f;
    [SerializeField] private Health _health;
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private Color _damageHealthColor;
    [SerializeField] private AnimationCurve _colorBehaviour;
    [SerializeField] private Animator _heartAnimator;
    [SerializeField] private AnimationClip _heartPulseAnimation;

    private Color _originalHealthColor;

    private void Start()
    {
        _originalHealthColor = _healthText.color;
        _healthText.text = _health.MaxHealth.ToString("");
    }

    private void OnEnable()
    {
        _health.HealthChanged += TakeDamage;
    }

    private void OnDisable()
    {
        _health.HealthChanged -= TakeDamage;
    }

    private void TakeDamage(float currentHealth)
    {
        _heartAnimator.Play(_heartPulseAnimation.name);
        StartCoroutine(DecreaseHealthSmoothly(currentHealth));
    }
    private IEnumerator DecreaseHealthSmoothly(float target)
    {
        float delapsedTime = 0f;
        float previousValue = float.Parse(_healthText.text);

        while (delapsedTime < _smoothDecreaseDuration)
        {
            delapsedTime += Time.deltaTime;
            float normalizedPosition = delapsedTime / _smoothDecreaseDuration;
            float intermidateValue = Mathf.Lerp(previousValue, target, normalizedPosition);
            _healthText.text = intermidateValue.ToString("");

            _healthText.color = Color.Lerp(_originalHealthColor, _damageHealthColor, _colorBehaviour.Evaluate(normalizedPosition));

            yield return null;
        }
    }
}
