using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DoorHandleGuard : MonoBehaviour
{
    public LockManager lockManager;

    UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable interactable;

    void Awake()
    {
        interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();
        interactable.selectEntered.AddListener(OnSelect);
        interactable.activated.AddListener(OnActivated);
    }

    void OnDestroy()
    {
        if (!interactable) return;
        interactable.selectEntered.RemoveListener(OnSelect);
        interactable.activated.RemoveListener(OnActivated);
    }

    void OnSelect(SelectEnterEventArgs args)
    {
        if (lockManager && !lockManager.AllUnlocked())
            lockManager.TryOpenWhileLocked();
    }

    void OnActivated(ActivateEventArgs args)
    {
        if (lockManager && !lockManager.AllUnlocked())
            lockManager.TryOpenWhileLocked();
    }
}
