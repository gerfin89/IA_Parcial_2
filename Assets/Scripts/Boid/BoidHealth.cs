using UnityEngine;
using UnityEngine.UI;

public class BoidHealth : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] private int maxHealth = 2;
    [SerializeField] private Slider healthBar;

    private int _currentHealth;
    private bool _isDown;

    [Header("Feedback visual")]
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Color _damagedColor = Color.pink;
    [SerializeField] private Color _originalColor;

    public bool IsDown => _isDown;

    private void Awake()
    {
        _currentHealth = maxHealth;

        if (_renderer != null)
        {
            _originalColor = _renderer.material.color;
        }

        if (healthBar != null)
        {
            healthBar.minValue = 0;
            healthBar.maxValue = maxHealth;
            healthBar.wholeNumbers = true;
            healthBar.value = _currentHealth;
        }
    }

    public void TakeDamage(int amount)
    {
        if (_isDown) return;

        _currentHealth -= amount;

        if (healthBar != null)
        {
            healthBar.value = Mathf.Max(0, _currentHealth);
        }

        if (_currentHealth <= 0)
        {
            Down();
        }
    }

    public void ForceDown()
    {
        if (_isDown) return;

        Down();
    }

    private void Down()
    {
        _isDown = true;
        _currentHealth = 0;

        if (healthBar != null)
        {
            healthBar.value = 0;
        }

        if (_renderer != null)
        {
            _renderer.material.color = _damagedColor;
        }
    }

    public void ResetHealth()
    {
        _currentHealth = maxHealth;
        _isDown = false;

        if (healthBar != null)
        {
            healthBar.value = _currentHealth;
        }

        if (_renderer != null)
        {
            _renderer.material.color = _originalColor;
        }
    }
}