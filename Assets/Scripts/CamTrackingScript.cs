using UnityEngine;

public class CamTrackingScript : MonoBehaviour
{
    [SerializeField] private Transform camTrackingTarget;
    [SerializeField] private float camHeightOffset;

    private void Update()
    {
        transform.position = new Vector3(camTrackingTarget.position.x, camTrackingTarget.position.y + camHeightOffset, camTrackingTarget.position.z);
    }
}
