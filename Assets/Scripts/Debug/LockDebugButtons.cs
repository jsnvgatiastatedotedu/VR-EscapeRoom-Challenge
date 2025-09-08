using UnityEngine;

public class LockDebugButtons : MonoBehaviour
{
    public LockManager lockManager;

    public void UnlockKey()   { lockManager?.SetLockState(LockType.Key, true); }
    public void UnlockValve() { lockManager?.SetLockState(LockType.Valve, true); }
}
