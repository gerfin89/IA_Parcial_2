using UnityEngine;

public class GoatPen : MonoBehaviour
{
    [SerializeField] private float height;
    [SerializeField] private float width;
    [SerializeField] private bool drawGizmos;

   
    public Vector3 OutOfGoatPen(Vector3 position)
    {
        Vector3 newPosition = position;

        if(position.x > width/2) newPosition.x = -width/2;
        if (position.x < -width / 2) newPosition.x = width / 2;
        if (position.z > height / 2) newPosition.z = -height / 2;
        if (position.z < -height / 2) newPosition.z = height / 2;

        return newPosition;
    }

    public bool IsOutside(Vector3 position)
    {
        Vector3 localPosition = transform.InverseTransformPoint(position);

        return Mathf.Abs(localPosition.x) > width / 2f ||
               Mathf.Abs(localPosition.z) > height / 2f;
    }
    private void OnDrawGizmos()
    {
        if (!drawGizmos) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(width, 0, height));
    }
}


