using UnityEngine;

public class PaperReveal : MonoBehaviour
{
    public Light lamp;
    public Transform hotspot;
    public Vector3 hotspotSize = new Vector3(0.25f, 0.05f, 0.25f);
    public Renderer paperRenderer;
    public Material blurMat;
    public Material sharpMat;

    void Reset()
    {
        paperRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (!paperRenderer || !lamp || !hotspot) return;

        bool lampOn = lamp.intensity > 0.1f;
        bool paperInHotspot = BoundsContains(hotspot, hotspotSize, transform.position);

        var targetMat = (lampOn && paperInHotspot) ? sharpMat : blurMat;
        var mats = paperRenderer.sharedMaterials;
        if (mats.Length == 0 || mats[0] != targetMat)
        {
            paperRenderer.sharedMaterial = targetMat;
        }
    }

    bool BoundsContains(Transform center, Vector3 size, Vector3 worldPoint)
    {
        Vector3 local = center.InverseTransformPoint(worldPoint);
        Vector3 half = size * 0.5f;
        return Mathf.Abs(local.x) <= half.x &&
               Mathf.Abs(local.y) <= half.y &&
               Mathf.Abs(local.z) <= half.z;
    }
}
