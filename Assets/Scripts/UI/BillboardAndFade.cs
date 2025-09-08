using UnityEngine;
using TMPro;

public class BillboardAndFade : MonoBehaviour
{
    public Transform targetCamera;
    public float maxYaw = 30f;
    public float showDistance = 1.6f;
    public float fadeAfterSeconds = 4f;

    TMP_Text _tmp;
    float _timer = -1f;
    Color _base;
    bool _hidden;

    void Awake()
    {
        _tmp = GetComponent<TMP_Text>();
        if (_tmp) _base = _tmp.color;
    }

    void Update()
    {
        if (!_tmp || _hidden || !targetCamera) return;

        Vector3 toCam = (targetCamera.position - transform.position);
        toCam.y = 0f;
        if (toCam.sqrMagnitude > 0.001f)
        {
            var desired = Quaternion.LookRotation(toCam.normalized, Vector3.up);

            float yawDelta = Mathf.DeltaAngle(transform.eulerAngles.y, desired.eulerAngles.y);
            yawDelta = Mathf.Clamp(yawDelta, -maxYaw, maxYaw);
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + yawDelta, 0);
        }


        float d = Vector3.Distance(targetCamera.position, transform.position);
        bool close = d <= showDistance;

        if (close && _timer < 0f)
            _timer = fadeAfterSeconds;

        if (_timer >= 0f)
        {
            _timer -= Time.deltaTime;
            float a = Mathf.InverseLerp(0f, fadeAfterSeconds, _timer); // 1→0
            _tmp.color = new Color(_base.r, _base.g, _base.b, a);
            if (_timer <= 0f)
            {
                _hidden = true;
                _tmp.enabled = false;
            }
        }
    }
}
