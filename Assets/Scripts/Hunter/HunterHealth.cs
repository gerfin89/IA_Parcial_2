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
        if (hunter != null && hunter.IsInSafeZone())
            return;

        currentHealth = Mathf.Max(0, currentHealth - damage);
                

        if (IsDead)
        {
           
            FSM_Hunter hunter = GetComponent<FSM_Hunter>();

            if (hunter != null)
                hunter.Die();

        }
    }
    public void Respawn()
    {
        currentHealth = maxHealth;
        transform.position = spawnPosition;
              
    }
}