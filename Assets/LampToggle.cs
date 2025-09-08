using UnityEngine;

public class LampToggle : MonoBehaviour
{
    public Light lamp;
    public float onIntensity = 2.5f;

    public void Toggle()
    {
        if (!lamp) return;
        lamp.intensity = (lamp.intensity > 0.1f) ? 0f : onIntensity;
    }
}
