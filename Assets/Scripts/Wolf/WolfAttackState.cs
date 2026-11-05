
using UnityEngine;

public class WolfAttackState : State
{
    private readonly WolfController wolf;
    private HunterHealth hunterHealth;
    private float attackTimer;
    private DangerOverlayEffect dangerEffect;

    public WolfAttackState(WolfController wolf,StateMachine stateMachine) : base(stateMachine)
    {
        this.wolf = wolf;
    }

    public override void Enter()
    {
        base.Enter();
        
        hunterHealth = wolf.HunterTarget != null? wolf.HunterTarget.GetComponent<HunterHealth>(): null;

        attackTimer = 0f;
        dangerEffect = Object.FindFirstObjectByType<DangerOverlayEffect>();

        if (dangerEffect != null)
        {
            dangerEffect.AddThreat();
        }
    }
    public override void Exit()
    {
        base.Exit();

        if (dangerEffect != null)
        {
            dangerEffect.RemoveThreat();
        }
    }

    public override void Update()
    {
        if (hunterHealth == null || hunterHealth.IsDead)
        {
            stateMachine.ChangeState(WolfState.Patrol);
            return;
        }

        if (IsHunterProtected())
        {
            stateMachine.ChangeState(WolfState.Patrol);
            return;
        }

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

    private bool IsHunterProtected()
    {
        FSM_Hunter hunter = wolf.HunterTarget != null ? wolf.HunterTarget.GetComponent<FSM_Hunter>(): null;

        return hunter != null && hunter.IsInSafeZone();
    }
}