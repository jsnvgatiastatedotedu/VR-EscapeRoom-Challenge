using UnityEngine;

public class DoorLockIndicators : MonoBehaviour
{
    public LockManager lockManager;
    public Renderer ledKeypad;
    public Renderer ledKey;
    public Renderer ledValve;
    public Color lockedColor = Color.red;
    public Color unlockedColor = Color.green;

    void Update()
    {
        if (!lockManager) return;

        Set(ledKeypad, lockManager.keypadUnlocked);
        Set(ledKey,    lockManager.keyUnlocked);
        Set(ledValve,  lockManager.valveUnlocked);
    }

    void Set(Renderer r, bool on)
    {
        if (!r) return;
        var m = r.material;
        m.color = on ? unlockedColor : lockedColor;
        if (m.HasProperty("_EmissionColor"))
            m.SetColor("_EmissionColor", (on ? unlockedColor : lockedColor) * (on ? 1.5f : 0.5f));
    }
}
