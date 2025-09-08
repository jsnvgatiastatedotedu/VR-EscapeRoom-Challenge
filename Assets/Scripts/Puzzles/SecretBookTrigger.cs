using UnityEngine;


public class SecretBookTrigger : MonoBehaviour
{
    [Header("Slide Target")]
    public Transform panel;
    public Vector3 openLocalOffset = new Vector3(-0.7f, 0f, 0f);
    public float slideTime = 0.8f;
    public AnimationCurve ease = AnimationCurve.EaseInOut(0,0,1,1);

    [Header("Trigger Settings")]
    public float pullDistance = 0.15f;
    public AudioSource sfx;
    public AudioClip slideOpen;

    Vector3 _startPos;
    bool _opened = false;
    UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable _grab;

    void Awake()
    {
        _grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        _startPos = transform.localPosition;
    }

    void Update()
    {
        if (_opened) return;
        float dist = Vector3.Distance(transform.localPosition, _startPos);
        if (dist >= pullDistance)
        {
            _opened = true;
            if (sfx && slideOpen) sfx.PlayOneShot(slideOpen);
            StartCoroutine(SlidePanelOpen());
        }
    }

    System.Collections.IEnumerator SlidePanelOpen()
    {
        if (!panel) yield break;
        Vector3 start = panel.localPosition;
        Vector3 end = start + openLocalOffset;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.01f, slideTime);
            float k = ease.Evaluate(Mathf.Clamp01(t));
            panel.localPosition = Vector3.Lerp(start, end, k);
            yield return null;
        }
        panel.localPosition = end;
    }
}
