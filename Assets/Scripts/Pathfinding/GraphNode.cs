
using System.Collections.Generic;
using UnityEngine;

public class GraphNode : MonoBehaviour
{
    [SerializeField] private bool isSafeZone;

    [SerializeField] private LayerMask obstacleMask;

    [SerializeField] private List<GraphNode> neighbors = new();

    public bool IsSafeZone => isSafeZone;

    public IReadOnlyList<GraphNode> Neighbors => neighbors;

    public Vector3 Position => transform.position;

    public float GetDistanceTo(GraphNode other)
    {
        return Vector2.Distance(new Vector2(Position.x, Position.z),new Vector2(other.Position.x, other.Position.z));
    }

    public bool HasClearPathTo(GraphNode other)
    {
        Vector3 origin = Position + Vector3.up;
        Vector3 destination = other.Position + Vector3.up;

        Vector3 direction = destination - origin;
        float distance = direction.magnitude;

        return !Physics.Raycast(origin,direction.normalized,distance,obstacleMask);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isSafeZone ? Color.magenta : Color.cyan;

        Gizmos.DrawSphere(transform.position, 0.5f);

        Gizmos.color = Color.yellow;

        foreach (GraphNode neighbor in neighbors)
        {
            if (neighbor == null) continue;

            Gizmos.color = HasClearPathTo(neighbor)? Color.green : Color.red;

            Gizmos.DrawLine(transform.position,neighbor.transform.position);
        }
    }
}

