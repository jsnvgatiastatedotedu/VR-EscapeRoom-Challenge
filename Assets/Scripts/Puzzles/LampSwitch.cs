
using UnityEngine;


[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable))]
public class LampSwitch : MonoBehaviour
{
    public Light lamp;
    public float onIntensity = 2.5f;
    public AudioSource sfx;
    public AudioClip click;

    UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable ix;

    void Awake()
    {
        ix = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();
        ix.selectEntered.AddListener(_ => Toggle());
        ix.activated.AddListener(_ => Toggle());

    void OnDestroy()
    {
        if (!ix) return;
        ix.selectEntered.RemoveListener(_ => Toggle());
        ix.activated.RemoveListener(_ => Toggle());
    }

    void Toggle()
    {
        if (!lamp) return;
        lamp.intensity = (lamp.intensity > 0.1f) ? 0f : onIntensity;
        if (sfx && click) sfx.PlayOneShot(click);
    }
}
