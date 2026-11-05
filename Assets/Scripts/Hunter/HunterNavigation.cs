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

            float distance = Vector2.Distance(new Vector2(position.x, position.z),new Vector2(node.Position.x, node.Position.z));

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

    public void CalculatePath(GraphNode start, GraphNode goal,bool allowSafeZones)
    {
        if (start == null || goal == null)
            return;

        List<GraphNode> path = pathfinder.FindPath(start,goal,allowSafeZones);

        if (pathFollower == null)
            return;

        pathFollower.SetPath(path);
    }

    public float GetPathDistance(List<GraphNode> path)
    {
        if (path == null || path.Count == 0)
            return Mathf.Infinity;

        float distance = 0f;

        for (int i = 0; i < path.Count - 1; i++)
        {
            distance += path[i].GetDistanceTo(path[i + 1]);
        }

        return distance;
    }

    public GraphNode GetClosestSafeZone(GraphNode safeZoneA, GraphNode safeZoneB)
    {
        List<GraphNode> pathA = GetPath(safeZoneA, true);
        List<GraphNode> pathB = GetPath(safeZoneB, true);

        float distanceA = GetPathDistance(pathA);
        float distanceB = GetPathDistance(pathB);

        if (float.IsPositiveInfinity(distanceA) && float.IsPositiveInfinity(distanceB))
            return null;

        return distanceA <= distanceB ? safeZoneA : safeZoneB;
    }

    public List<GraphNode> GetPath(GraphNode goal, bool allowSafeZones)
    {
        GraphNode start = GetNearestNode(transform.position);

        if (start == null || goal == null)
            return new List<GraphNode>();

        return pathfinder.FindPath(start, goal, allowSafeZones);
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

        return !Physics.Raycast( origin, direction.normalized, direction.magnitude,obstacleMask);
    }
}