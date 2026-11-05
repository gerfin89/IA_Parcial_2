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
        Debug.Log("Wolf: iniciando patrullaje");
    }

    public override void Update()
    {
        if (wolf.Perception.CanSeeHunter(wolf.HunterTarget))
        {
            Debug.Log("¡Wolf detectó al Hunter!");

            stateMachine.ChangeState(WolfState.Howl);
            return;
        }
        if (wolf.Waypoints == null || wolf.Waypoints.Length < 2)
            return;

        Transform waypoint = wolf.Waypoints[waypointIndex];

        if (waypoint == null)
            return;

        Vector3 destination = waypoint.position;
        destination.y = wolf.transform.position.y;

        wolf.transform.position = Vector3.MoveTowards(wolf.transform.position,destination,wolf.MoveSpeed * Time.deltaTime);

        if (Vector3.Distance(wolf.transform.position, destination)<= ReachDistance)
        {
            waypointIndex = (waypointIndex + 1) % wolf.Waypoints.Length;
        }
    }
}