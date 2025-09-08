using UnityEngine;
using UnityEngine.Events;

public enum LockType { Keypad, Key, Valve }

public class LockManager : MonoBehaviour
{
    [Header("References")]
    public Rigidbody doorRb;
    public HingeJoint doorHinge;
    public Renderer indicatorRenderer;

    [Header("SFX")]
    public AudioSource doorAudio;
    public AudioClip lockedClank;
    public AudioClip latchRelease;

    [Header("State (read-only)")]
    public bool keypadUnlocked;
    public bool keyUnlocked;
    public bool valveUnlocked;

    public UnityEvent onAllUnlocked;

    void Awake()
    {
        // Start locked
        SetDoorLocked(true);
        UpdateIndicator();
    }

    public void SetLockState(LockType type, bool value)
    {
        switch (type)
        {
            case LockType.Keypad: keypadUnlocked = value; break;
            case LockType.Key:    keyUnlocked    = value; break;
            case LockType.Valve:  valveUnlocked  = value; break;
        }

        UpdateIndicator();

        if (AllUnlocked())
        {
            // Unlock door
            SetDoorLocked(false);
            if (doorAudio && latchRelease) doorAudio.PlayOneShot(latchRelease);
            onAllUnlocked?.Invoke();
        }
    }

    public bool AllUnlocked() => keypadUnlocked && keyUnlocked && valveUnlocked;

    public void TryOpenWhileLocked()
    {
        if (!AllUnlocked())
        {
            if (doorAudio && lockedClank) doorAudio.PlayOneShot(lockedClank);
        }
    }

    void SetDoorLocked(bool locked)
    {
        if (doorRb) doorRb.isKinematic = locked;

        if (doorHinge)
        {
            var limits = doorHinge.limits;
            limits.min = 0f;
            limits.max = 105f;
            doorHinge.limits = limits;
            doorHinge.useLimits = true;
        }
    }

    void UpdateIndicator()
    {
        if (!indicatorRenderer) return;
        
        var mat = indicatorRenderer.material;
        if (AllUnlocked())
        {
            mat.SetColor("_EmissionColor", Color.green * 1.5f);
            mat.color = Color.green;
        }
        else
        {
            mat.SetColor("_EmissionColor", Color.red * 0.5f);
            mat.color = Color.red;
        }
    }
}
