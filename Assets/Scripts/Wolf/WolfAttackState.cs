
using UnityEngine;

public class WolfAttackState : State
{
    private readonly WolfController wolf;
    private HunterHealth hunterHealth;
    private float attackTimer;

    public WolfAttackState(WolfController wolf,StateMachine stateMachine) : base(stateMachine)
    {
        this.wolf = wolf;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Wolf: iniciando ataque");
        hunterHealth = wolf.HunterTarget != null? wolf.HunterTarget.GetComponent<HunterHealth>(): null;

        attackTimer = 0f;
    }

    public override void Update()
    {
        if (ShouldReturnToChase())
        {
            stateMachine.ChangeState(WolfState.Chase);
            return;
        }

        AttackHunter();
    }

    private bool ShouldReturnToChase()
    {
        if (hunterHealth == null || hunterHealth.IsDead)
            return false;

        return !wolf.IsHunterInAttackRange();
    }

    private void AttackHunter()
    {
        if (hunterHealth == null || hunterHealth.IsDead)
            return;

        attackTimer -= Time.deltaTime;

        if (attackTimer > 0f)
            return;

        hunterHealth.TakeDamage(wolf.AttackDamage);
        attackTimer = wolf.AttackCooldown;
    }
}