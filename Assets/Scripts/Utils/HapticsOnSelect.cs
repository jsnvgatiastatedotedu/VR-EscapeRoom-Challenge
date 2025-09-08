using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HapticsOnSelect : MonoBehaviour
{
    public float amplitude = 0.5f;
    public float duration = 0.15f;

    void Awake()
    {
        var socket = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>();
        if (socket)
            socket.selectEntered.AddListener(OnSelect);
    }

    void OnSelect(SelectEnterEventArgs args)
    {
        if (args.interactorObject is UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor controllerInteractor)
        {
            controllerInteractor.SendHapticImpulse(amplitude, duration);
        }
    }
}
