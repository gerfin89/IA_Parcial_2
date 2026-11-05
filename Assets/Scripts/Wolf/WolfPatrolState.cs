using UnityEngine;

public class WolfPatrolState : State
{
    private readonly WolfController wolf;
    private int waypointIndex;

    private const float ReachDistance = 0.3f;

    public WolfPatrolState(WolfController wolf, StateMachine stateMachine) : base(stateMachine)
    {
        this.wolf = wolf;
    }

    public override void Enter()
    {
        base.Enter();

        waypointIndex = 0;
        
    }

    public override void Update()
    {
        if (DetectHunter())
            return;

        Patrol();
    }

    private bool DetectHunter()
    {
        if (wolf.HunterTarget == null)
            return false;

        HunterHealth health =
            wolf.HunterTarget.GetComponent<HunterHealth>();

        if (health == null || health.IsDead)
            return false;

        if (!wolf.Perception.CanSeeHunter(wolf.HunterTarget))
            return false;

        stateMachine.ChangeState(WolfState.Howl);
        return true;
    }

    private void Patrol()
    {
        if (wolf.Waypoints == null || wolf.Waypoints.Length < 2)
            return;

        Transform waypoint = wolf.Waypoints[waypointIndex];

        if (waypoint == null)
            return;

        MoveToWaypoint(waypoint);
    }

    private void MoveToWaypoint(Transform waypoint)
    {
        Vector3 destination = waypoint.position;
        destination.y = wolf.transform.position.y;

        Vector3 nextPosition = Vector3.MoveTowards( wolf.transform.position,destination,wolf.MoveSpeed * Time.deltaTime);

        if (wolf.IsPositionInSafeZone(nextPosition))
            return;

        wolf.transform.position = nextPosition;

        CheckWaypointArrival(destination);
    }

    private void CheckWaypointArrival(Vector3 destination)
    {
        if (Vector3.Distance(wolf.transform.position, destination) <= ReachDistance) waypointIndex = (waypointIndex + 1) % wolf.Waypoints.Length;
    }
}