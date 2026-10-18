using System.Collections.Generic;
using UnityEngine;

public class BoidSpawner : MonoBehaviour
{
    [SerializeField] private GameObject boidPrefab;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private GoatPen goatPen;

    public void SpawnBoid()
    {
        if (boidPrefab == null || spawnPoints == null || spawnPoints.Count == 0)
        {
            Debug.LogWarning("BoidSpawner: falta el prefab o los puntos de aparición.");
            return;
        }

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

        GameObject boid = Instantiate(boidPrefab,spawnPoint.position,spawnPoint.rotation);

        BoidSteering steering = boid.GetComponent<BoidSteering>();

        if (steering != null)
            steering.SetGoatPen(goatPen);
    }
}