using UnityEngine;

public class HunterHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 3;

    private int currentHealth;
    private Vector3 spawnPosition;
    private FSM_Hunter hunter;

    public bool IsDead => currentHealth <= 0;

    private void Awake()
    {
        currentHealth = maxHealth;
        spawnPosition = transform.position;
        hunter = GetComponent<FSM_Hunter>();
    }

    public void TakeDamage(int damage)
    {
        if (IsDead || damage <= 0)
            return;

        currentHealth = Mathf.Max(0, currentHealth - damage);

        Debug.Log($"Hunter recibió daño. Vida: {currentHealth}/{maxHealth}");

        if (IsDead)
        {
            Debug.Log("Hunter murió.");

            if (hunter != null)
                hunter.Die();
        }
    }

    public void Respawn()
    {
        currentHealth = maxHealth;
        transform.position = spawnPosition;

        Debug.Log("Hunter reapareció con la vida restaurada.");
    }
}