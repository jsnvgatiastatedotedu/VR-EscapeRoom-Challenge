
using UnityEngine;


public class ValveController : MonoBehaviour
{

    public float CurrentAngle => _currentAngle;


    [Header("Refs")]
    public Transform pivot;
    public Transform valve;
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable interactable;

    [Header("Rotation")]
    public Vector2 angleLimits = new Vector2(0f, 90f);
    public float detentStep = 10f;
    public float solveThreshold = 85f;
    public bool requireTwoHands = true;

    [Header("Door hookup")]
    public LockManager lockManager;
    public LockType reportsAs = LockType.Valve;
    
    readonly System.Collections.Generic.List<UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor> _hands =
        new System.Collections.Generic.List<UnityEngine.XR.Interaction.Toolkit.Interactors.IXRSelectInteractor>();
    float _currentAngle;
    bool _solved;

    void Awake()
    {
        if (!interactable) interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        interactable.selectEntered.AddListener(e => { if (!_hands.Contains(e.interactorObject)) _hands.Add(e.interactorObject); });
        interactable.selectExited.AddListener(e => { _hands.Remove(e.interactorObject); });
    }

    void Update()
    {
        if (requireTwoHands && _hands.Count < 2) return;
        if (_hands.Count == 0 || !pivot || !valve) return;

        var t = (_hands[0] as MonoBehaviour)?.transform;
        if (!t) return;

        Vector3 local = pivot.InverseTransformPoint(t.position);
        local.z = 0f;
        if (local.sqrMagnitude < 0.0001f) return;

        float raw = Mathf.Atan2(local.y, local.x) * Mathf.Rad2Deg;
        raw = Mathf.Repeat(raw, 360f);
        float clamped = Mathf.Clamp(raw, angleLimits.x, angleLimits.y);
        if (detentStep > 0.01f) clamped = Mathf.Round(clamped / detentStep) * detentStep;

        _currentAngle = clamped;
        valve.localRotation = Quaternion.AngleAxis(_currentAngle, Vector3.forward);

        if (!_solved && _currentAngle >= solveThreshold)
        {
            _solved = true;
            if (lockManager) lockManager.SetLockState(reportsAs, true);
        }
    }
}
