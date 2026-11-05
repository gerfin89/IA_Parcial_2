using UnityEngine;

[RequireComponent(typeof(HunterNavigation))]
[RequireComponent(typeof(WolfPerception))]
public class WolfController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 4f;

    [Header("Combat")]
    [SerializeField] private float attackRange = 2f;
    public float AttackRange => attackRange;

    [SerializeField] private int attackDamage = 1;
    [SerializeField] private float attackCooldown = 1.5f;
    public int AttackDamage => attackDamage;
    public float AttackCooldown => attackCooldown;

    [Header("Patrol")]
    [SerializeField] private Transform[] waypoints;

    [Header("Target")]
    [SerializeField] private Transform hunterTarget;

    public Transform HunterTarget => hunterTarget;

    public Transform[] Waypoints => waypoints;

    private HunterNavigation navigation;
    private WolfPerception perception;
    public WolfPerception Perception => perception;
    public HunterNavigation Navigation => navigation;
    private StateMachine stateMachine;

    public float MoveSpeed => moveSpeed;

    private void Awake()
    {
        navigation = GetComponent<HunterNavigation>();
        perception = GetComponent<WolfPerception>();

        stateMachine = new StateMachine();

        WolfPatrolState patrolState = new WolfPatrolState(this, stateMachine);
        stateMachine.RegisterState(WolfState.Patrol, patrolState);

        WolfHowlState howlState = new WolfHowlState(this, stateMachine);
        stateMachine.RegisterState(WolfState.Howl, howlState);

        WolfChaseState chaseState = new WolfChaseState(this, stateMachine);
        stateMachine.RegisterState(WolfState.Chase, chaseState);

        WolfAttackState attackState = new WolfAttackState(this, stateMachine);
        stateMachine.RegisterState(WolfState.Attack, attackState);
    }

    private void Start()
    {
        stateMachine.ChangeState(WolfState.Patrol);
    }

    private void Update()
    {
        stateMachine.Update();
    }

    public bool IsHunterInAttackRange()
    {
        if (hunterTarget == null)
            return false;

        Vector3 wolfPosition = transform.position;
        Vector3 hunterPosition = hunterTarget.position;

        wolfPosition.y = 0f;
        hunterPosition.y = 0f;

        return Vector3.Distance(wolfPosition, hunterPosition) <= attackRange;
    }
}