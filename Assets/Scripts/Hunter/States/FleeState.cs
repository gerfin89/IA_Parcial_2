using UnityEngine;

public class FleeState : State
{
    private readonly FSM_Hunter hunter;
    private GraphNode targetSafeZone;

    private float safeZoneTimer;
    private const float SafeZoneWaitTime = 5f;

    public FleeState(FSM_Hunter hunter, StateMachine stateMachine): base(stateMachine)
    {
        this.hunter = hunter;
    }

    public override void Enter()
    {
        base.Enter();

        hunter.ChangeColor(Color.yellow);

        safeZoneTimer = 0f;

        targetSafeZone = hunter.Navigation.GetClosestSafeZone(hunter.SafeZoneA,hunter.SafeZoneB);

        if (targetSafeZone == null)
            return;

        hunter.Navigation.CalculatePathTo(targetSafeZone, true);
    }

    public override void Update()
    {
        if (targetSafeZone == null)
            return;

        if (!hunter.IsInSafeZone())
        {
            safeZoneTimer = 0f;
            hunter.Navigation.FollowPath(hunter.MaxSpeed);
            return;
        }

        WaitInSafeZone();
    }

    private void WaitInSafeZone()
    {
        safeZoneTimer += Time.deltaTime;

        if (safeZoneTimer < SafeZoneWaitTime)
            return;

        hunter.FinishFlee();
    }
}