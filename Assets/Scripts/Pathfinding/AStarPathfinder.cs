using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinder
{
    public List<GraphNode> FindPath(GraphNode start, GraphNode goal, bool allowSafeZones)
    {
        if (start == null || goal == null)
            return new List<GraphNode>();

        List<GraphNode> openSet = new();
        HashSet<GraphNode> closedSet = new();

        Dictionary<GraphNode, GraphNode> cameFrom = new();

        Dictionary<GraphNode, float> gScore = new();
        Dictionary<GraphNode, float> fScore = new();

        openSet.Add(start);

        gScore[start] = 0f;
        fScore[start] = start.GetDistanceTo(goal);


        while (openSet.Count > 0)
        {
            GraphNode current = openSet[0];

            foreach (GraphNode node in openSet)
            {
                if (fScore[node] < fScore[current])
                {
                    current = node;
                }
            }

            if (current == goal)
            {
                return ReconstructPath(cameFrom, current);
            }

            openSet.Remove(current);
            closedSet.Add(current);



            foreach (GraphNode neighbor in current.Neighbors)
            {
                if (neighbor == null || closedSet.Contains(neighbor))
                    continue;

                if (!allowSafeZones && neighbor.IsSafeZone)
                    continue;

                if (!current.HasClearPathTo(neighbor))
                    continue;

                float tentativeGScore = gScore[current] + current.GetDistanceTo(neighbor);

                if (gScore.TryGetValue(neighbor, out float previousGScore) && tentativeGScore >= previousGScore)
                {
                    continue;
                }

                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeGScore;

                fScore[neighbor] = tentativeGScore + neighbor.GetDistanceTo(goal);

                if (!openSet.Contains(neighbor))
                {
                    openSet.Add(neighbor);
                }
            }
        }        
        return new List<GraphNode>();
    }

    private List<GraphNode> ReconstructPath(Dictionary<GraphNode, GraphNode> cameFrom,GraphNode current)
    {
        List<GraphNode> path = new();

        path.Add(current);

        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(current);
        }

        path.Reverse();

        return path;
    }

}
