using System.Collections.Generic;
using UnityEngine;

public class AStarPathTester : MonoBehaviour
{
    [SerializeField] private GraphNode startNode;
    [SerializeField] private GraphNode goalNode;
    [SerializeField] private bool allowSafeZones = true;

    private readonly AStarPathfinder pathfinder = new();
    private List<GraphNode> currentPath = new();

    [SerializeField] private GraphPathFollower pathFollower;

    [ContextMenu("Calcular ruta A*")]
    private void CalculatePath()
    {
        currentPath = pathfinder.FindPath(startNode, goalNode, allowSafeZones);
               
        if (pathFollower != null)
        {
            pathFollower.SetPath(currentPath);
        }
    }

    private void Update()
    {
        if (pathFollower == null)
            return;

        pathFollower.MoveAlongPath(5f);
    }

    private void OnDrawGizmos()
    {
        if (currentPath == null || currentPath.Count == 0)
            return;

        Gizmos.color = Color.yellow;

        for (int i = 0; i < currentPath.Count - 1; i++)
        {
            Gizmos.DrawLine(
                currentPath[i].Position + Vector3.up * 1.5f,
                currentPath[i + 1].Position + Vector3.up * 1.5f
            );
        }
    }
}
