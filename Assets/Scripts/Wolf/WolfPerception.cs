using UnityEngine;
using static UnityEngine.Rendering.HableCurve;

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
        HunterHealth hunterHealth = hunter.GetComponent<HunterHealth>();

        if (hunterHealth == null || hunterHealth.IsDead)
            return false;
        FSM_Hunter hunterController = hunter.GetComponent<FSM_Hunter>();

        if (hunterController != null && hunterController.IsInSafeZone())
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
    private void OnDrawGizmosSelected()
    {
        Vector3 origin = transform.position + Vector3.up * 0.1f;

        int segments = 30;
        float gizmoAngle = 60f;
        float halfAngle = gizmoAngle / 2f;

        Gizmos.color = Color.yellow;

        Vector3 previousPoint = origin +
            Quaternion.Euler(0f, -halfAngle, 0f) *
            transform.forward * visionRadius;

        for (int i = 1; i <= segments; i++)
        {
            float angle = -halfAngle + gizmoAngle * i / segments;

            Vector3 direction =
                Quaternion.Euler(0f, angle, 0f) * transform.forward;

            Vector3 currentPoint = origin + direction * visionRadius;

            Gizmos.DrawLine(previousPoint, currentPoint);
            previousPoint = currentPoint;
        }

        Gizmos.color = Color.red;

        Vector3 left =
            Quaternion.Euler(0f, -halfAngle, 0f) * transform.forward;

        Vector3 right =
            Quaternion.Euler(0f, halfAngle, 0f) * transform.forward;

        Gizmos.DrawLine(origin, origin + left * visionRadius);
        Gizmos.DrawLine(origin, origin + right * visionRadius);
    }
}