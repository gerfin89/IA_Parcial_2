using UnityEngine;

public class WolfChaseState : State
{
    private readonly WolfController wolf;

    private float pathTimer;
    private const float PathUpdateInterval = 0.5f;

    public WolfChaseState(WolfController wolf,StateMachine stateMachine) : base(stateMachine)
    {
        this.wolf = wolf;
    }

    public override void Enter()
    {
        base.Enter();

        pathTimer = 0f;
        Debug.Log("Wolf: iniciando persecución");
    }

    public override void Update()
    {
        if (wolf.HunterTarget == null)
            return;

        if (wolf.IsHunterInAttackRange())
        {
            stateMachine.ChangeState(WolfState.Attack);
            return;
        }

        UpdatePath();
        FollowHunter();
    }

    private void UpdatePath()
    {
        pathTimer -= Time.deltaTime;

        if (pathTimer > 0f)
            return;

        pathTimer = PathUpdateInterval;

        GraphNode destination = wolf.Navigation.GetNearestNode(wolf.HunterTarget.position);

        if (destination == null || destination.IsSafeZone)
            return;

        wolf.Navigation.CalculatePathTo(destination, false);
    }

    private void FollowHunter()
    {
        wolf.Navigation.FollowPath(wolf.MoveSpeed);
    }
}