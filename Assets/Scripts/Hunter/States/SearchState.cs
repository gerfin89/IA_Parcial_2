
using UnityEngine;

public class SearchState : State
{
    private readonly FSM_Hunter hunter;
    private GraphNode searchDestination;

    public SearchState(FSM_Hunter hunter, StateMachine stateMachine): base(stateMachine)
    {
        this.hunter = hunter;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Hunter está buscando Boids");
        SelectSearchDestination();
    }

    public override void Update()
    {
        if (SearchForBoids())
            return;

        MoveToSearchDestination();
    }

    private bool SearchForBoids()
    {
        Transform target = hunter.Perception.BoidInVision(10f);

        if (target == null)
            return false;
     
        stateMachine.ChangeState(FarmerState.Attack);

        return true;
    }

    private void SelectSearchDestination()
    {
        var nodes = hunter.Navigation.GraphNodes;

        if (nodes == null || nodes.Count == 0)
            return;

        for (int attempt = 0; attempt < nodes.Count; attempt++)
        {
            GraphNode node = nodes[Random.Range(0, nodes.Count)];

            if (node == null || node.IsSafeZone)
                continue;

            if (hunter.Navigation.GetPath(node, false).Count == 0)
                continue;

            searchDestination = node;
            hunter.Navigation.CalculatePathTo(searchDestination, false);
            return;
        }
    }
    private void MoveToSearchDestination()
    {
        if (searchDestination == null)
        {
            SelectSearchDestination();
            return;
        }

        hunter.Navigation.FollowPath(hunter.MoveSpeed);

        Vector3 hunterPosition = hunter.transform.position;
        Vector3 destination = searchDestination.Position;

        hunterPosition.y = 0f;
        destination.y = 0f;

        if (Vector3.Distance(hunterPosition, destination) <= 1f)
            SelectSearchDestination();
    }
}