using UnityEngine;

public class FleeState : State
{
    private readonly FSM_Hunter hunter;
    private GraphNode targetSafeZone;

    public FleeState(FSM_Hunter hunter, StateMachine stateMachine): base(stateMachine)
    {
        this.hunter = hunter;
    }

    public override void Enter()
    {
        base.Enter();

        hunter.ChangeColor(Color.yellow);

        targetSafeZone = hunter.Navigation.GetClosestSafeZone(hunter.SafeZoneA,hunter.SafeZoneB);

        if (targetSafeZone == null)
        {
            Debug.LogWarning("Hunter: no hay rutas disponibles hacia las zonas seguras.");
            return;
        }

        hunter.Navigation.CalculatePathTo(targetSafeZone, true);
    }

    public override void Update()
    {
        if (targetSafeZone == null)
            return;

        hunter.Navigation.FollowPath(hunter.MaxSpeed);
    }
}