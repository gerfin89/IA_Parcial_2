using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GraphPathFollower))]
public class HunterNavigation : MonoBehaviour
{
    [Header("Pathfinding")]
    [SerializeField] private List<GraphNode> graphNodes;
    [SerializeField] private LayerMask obstacleMask;

    private GraphPathFollower pathFollower;
    private readonly AStarPathfinder pathfinder = new();

    public GraphPathFollower PathFollower => pathFollower;

    private void Awake()
    {
        pathFollower = GetComponent<GraphPathFollower>();
    }

    public GraphNode GetNearestNode(Vector3 position)
    {
        GraphNode nearestNode = null;
        float minDistance = Mathf.Infinity;

        foreach (GraphNode node in graphNodes)
        {
            if (node == null)
                continue;

            float distance = Vector2.Distance(
                new Vector2(position.x, position.z),
                new Vector2(node.Position.x, node.Position.z)
            );

            if (distance < minDistance)
            {
                minDistance = distance;
                nearestNode = node;
            }
        }

        return nearestNode;
    }

    public void CalculatePathTo(GraphNode goal, bool allowSafeZones)
    {
        GraphNode start = GetNearestNode(transform.position);

        if (start == null || goal == null)
            return;

        CalculatePath(start, goal, allowSafeZones);
    }

    public void CalculatePath(
        GraphNode start,
        GraphNode goal,
        bool allowSafeZones)
    {
        if (start == null || goal == null)
            return;

        List<GraphNode> path = pathfinder.FindPath(
            start,
            goal,
            allowSafeZones
        );

        if (pathFollower == null)
            return;

        pathFollower.SetPath(path);
    }

    public void FollowPath(float speed)
    {
        if (pathFollower == null)
            return;

        pathFollower.MoveAlongPath(speed);
    }

    public bool HasClearPathTo(Vector3 destination)
    {
        Vector3 origin = transform.position + Vector3.up;
        Vector3 target = destination + Vector3.up;
        Vector3 direction = target - origin;

        if (direction.sqrMagnitude <= Mathf.Epsilon)
            return true;

        return !Physics.Raycast(
            origin,
            direction.normalized,
            direction.magnitude,
            obstacleMask
        );
    }
}