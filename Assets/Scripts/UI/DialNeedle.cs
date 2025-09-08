using UnityEngine;

public class DialNeedle : MonoBehaviour
{
    [Tooltip("ValveController that owns the current angle")]
    public ValveController valve;
    [Tooltip("Min/max needle rotation around Z (degrees)")]
    public Vector2 needleRange = new Vector2(-90f, 90f);

    void LateUpdate()
    {
        if (!valve) return;
        float t = Mathf.InverseLerp(valve.angleLimits.x, valve.angleLimits.y, valve.CurrentAngle);
        float z = Mathf.Lerp(needleRange.x, needleRange.y, t);
        transform.localRotation = Quaternion.Euler(0, 0, z);
    }
}
