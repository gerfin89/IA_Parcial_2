using System.Collections.Generic;
using UnityEngine;

public class HunterPerception : MonoBehaviour
{
    private readonly List<BoidSteering> nearbyBoids = new();

    private void Update()
    {
        nearbyBoids.RemoveAll(boid => boid == null);
    }

    public Transform BoidInVision(float visionRadius)
    {
        Transform inVision = null;
        float minDistance = Mathf.Infinity;

        foreach (BoidSteering boid in nearbyBoids)
        {
            if (boid == null)
                continue;

            BoidHealth health = boid.GetComponent<BoidHealth>();

            if (health != null && health.IsDown)
                continue;

            float distance = Vector3.Distance(transform.position,boid.transform.position);

            if (distance <= visionRadius && distance < minDistance)
            {
                minDistance = distance;
                inVision = boid.transform;
            }
        }

        return inVision;
    }

    public Transform FallendBoidInVision(float visionRadius)
    {
        Transform inVision = null;
        float minDistance = Mathf.Infinity;

        foreach (BoidSteering boid in nearbyBoids)
        {
            if (boid == null)
                continue;

            BoidHealth health = boid.GetComponent<BoidHealth>();

            if (health == null || !health.IsDown)
                continue;

            if (boid.IsRespawning)
                continue;

            float distance = Vector3.Distance(transform.position,boid.transform.position);

            if (distance <= visionRadius && distance < minDistance)
            {
                minDistance = distance;
                inVision = boid.transform;
            }
        }

        return inVision;
    }

    private void OnTriggerEnter(Collider other)
    {
        BoidSteering boid = other.GetComponentInParent<BoidSteering>();

        if (boid != null && !nearbyBoids.Contains(boid))
        {
            nearbyBoids.Add(boid);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        BoidSteering boid = other.GetComponentInParent<BoidSteering>();

        if (boid != null)
        {
            nearbyBoids.Remove(boid);
        }
    }
}