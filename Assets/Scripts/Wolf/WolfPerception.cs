using UnityEngine;

public class WolfPerception : MonoBehaviour
{
    [Header("Vision")]
    [SerializeField] private float visionRadius = 12f;
    [SerializeField, Range(0f, 360f)]
    private float visionAngle = 120f;

    [SerializeField] private LayerMask obstacleMask;

    public bool CanSeeHunter(Transform hunter)
    {
        if (hunter == null)
            return false;

        Vector3 direction = hunter.position - transform.position;
        direction.y = 0f;

        if (direction.magnitude > visionRadius)
            return false;

        if (Vector3.Angle(transform.forward, direction) > visionAngle / 2f)
            return false;

        Vector3 origin = transform.position + Vector3.up;
        Vector3 target = hunter.position + Vector3.up;

        if (Physics.Linecast(origin, target, obstacleMask))
            return false;

        return true;
    }
}