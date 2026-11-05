using UnityEngine;

public class WolfChaseState : State
{
    private readonly WolfController wolf;
    private FSM_Hunter hunter;
    private GraphNode lastDestination;
    private float pathTimer;
    private const float PathUpdateInterval = 0.5f;
    private DangerOverlayEffect dangerEffect;

    public WolfChaseState(WolfController wolf,StateMachine stateMachine) : base(stateMachine)
    {
        this.wolf = wolf;
    }

    public override void Enter()
    {
        base.Enter();

        pathTimer = 0f;
        Debug.Log("Wolf: iniciando persecución");
        lastDestination = null;
        hunter = wolf.HunterTarget != null? wolf.HunterTarget.GetComponent<FSM_Hunter>(): null;
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
        if (wolf.HunterTarget == null)
            return;
        if (IsHunterProtected())
        {
            stateMachine.ChangeState(WolfState.Patrol);
            return;
        }

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

        if (destination == lastDestination)
            return;

        lastDestination = destination;
        wolf.Navigation.CalculatePathTo(destination, false);
    }

    private void FollowHunter()
    {
        if (wolf.Navigation.HasClearPathTo(wolf.HunterTarget.position))
        {
            MoveDirectlyToHunter();
            return;
        }

        FollowSafePath();
    }

    private void FollowSafePath()
    {
        GraphNode nextNode = wolf.Navigation.PathFollower.NextNode;

        if (nextNode == null)
            return;

        if (wolf.IsPositionInSafeZone(nextNode.Position))
            return;

        wolf.Navigation.FollowPath(wolf.MoveSpeed);
    }

    private void MoveDirectlyToHunter()
    {
        Vector3 destination = wolf.HunterTarget.position;
        destination.y = wolf.transform.position.y;

        Vector3 nextPosition = Vector3.MoveTowards(wolf.transform.position,destination,wolf.MoveSpeed * Time.deltaTime);

        if (wolf.IsPositionInSafeZone(nextPosition))
            return;

        wolf.transform.position = nextPosition;
    }

    private bool IsHunterProtected()
    {
        return hunter != null && hunter.IsInSafeZone();
    }
}