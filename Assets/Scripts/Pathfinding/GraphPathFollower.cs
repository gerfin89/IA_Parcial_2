using System.Collections.Generic;
using UnityEngine;

public class GraphPathFollower : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float nodeReachDistance = 0.3f;

    private List<GraphNode> currentPath = new();
    private int currentNodeIndex;

    public bool HasReachedDestination =>currentPath.Count > 0 && currentNodeIndex >= currentPath.Count;
    public GraphNode NextNode =>currentNodeIndex < currentPath.Count? currentPath[currentNodeIndex]: null;

    public void SetPath(List<GraphNode> newPath)
    {
        currentPath = newPath ?? new List<GraphNode>();
        currentNodeIndex = 0;
    }
    public void MoveAlongPath(float speed)
    {
        if (currentNodeIndex >= currentPath.Count)
            return;

        GraphNode targetNode = currentPath[currentNodeIndex];

        if (targetNode == null)
            return;

        Vector3 destination = targetNode.Position;
        destination.y = transform.position.y;

        transform.position = Vector3.MoveTowards(
            transform.position,
            destination,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, destination) <= nodeReachDistance)
        {
            currentNodeIndex++;
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (currentPath == null || currentNodeIndex >= currentPath.Count)
            return;

        GraphNode targetNode = currentPath[currentNodeIndex];

        if (targetNode == null)
            return;

        Gizmos.color = Color.yellow;

        Vector3 destination = targetNode.Position;
        destination.y = transform.position.y;

        Gizmos.DrawLine(transform.position, destination);
        Gizmos.DrawWireSphere(destination, 0.4f);
    }
}
