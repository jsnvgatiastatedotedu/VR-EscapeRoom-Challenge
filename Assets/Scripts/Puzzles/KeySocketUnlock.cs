using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class KeySocketUnlock : MonoBehaviour
{
    public LockManager lockManager;
    public AudioSource sfx;
    public AudioClip insertClip;
    public AudioClip rejectClip;

    UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socket;

    void Awake()
    {
        socket = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>();
        socket.selectEntered.AddListener(OnSelectEntered);
        socket.selectExited.AddListener(OnSelectExited);
    }

    void OnDestroy()
    {
        if (!socket) return;
        socket.selectEntered.RemoveListener(OnSelectEntered);
        socket.selectExited.RemoveListener(OnSelectExited);
    }

    void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (!args.interactableObject.transform.CompareTag("Key"))
        {
            if (sfx && rejectClip) sfx.PlayOneShot(rejectClip);
            if (args.interactableObject != null && socket.interactionManager != null)
{
    socket.interactionManager.SelectExit(socket, args.interactableObject);
}

            return;
        }

        if (sfx && insertClip) sfx.PlayOneShot(insertClip);


        var rb = args.interactableObject.transform.GetComponent<Rigidbody>();
        if (rb)
        {
            rb.isKinematic = true;
        }

        if (lockManager) lockManager.SetLockState(LockType.Key, true);
    }

    void OnSelectExited(SelectExitEventArgs args)
    {
        // To re-lock
        // if (lockManager) lockManager.SetLockState(LockType.Key, false);
    }
}
