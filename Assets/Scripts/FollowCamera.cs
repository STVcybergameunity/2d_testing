using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f, 0f, -10f);
    private float smoothTime = 0.25f;
    private float edgeMapLeft = 1.5f;

    private float maxTilt = 5f;          // max degrees of tilt
    private float tiltPerSpeed = 0.5f;     // degrees per unit of speed
    private float tiltSmoothTime = 0.4f;

    private Vector3 velocity = Vector3.zero;
    private float tiltVelocity = 0f;
    private Vector3 lastTargetPos;

    [SerializeField] private Transform target;

    void Start()
    {
        lastTargetPos = target.position;
    }

    void LateUpdate()
    {
        CamFollower();
        CamRotation();
    }

    void CamFollower()
    {
        Vector3 targetPosition = target.position + offset;
        if (targetPosition.y < 0)
        {
            targetPosition.y = 0f;
        }
        if (targetPosition.x < edgeMapLeft)
        {
            targetPosition.x = edgeMapLeft;
        }
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    void CamRotation()
    {
        // Target's horizontal speed, measured from how far it moved this frame
        float speedX = (target.position.x - lastTargetPos.x) / Time.deltaTime;
        lastTargetPos = target.position;

        // Moving right tilts negative Z, moving left tilts positive Z
        float desiredZ = Mathf.Clamp(-speedX * tiltPerSpeed, -maxTilt, maxTilt);

        // Returns a int example -3 = 357
        float currentZ = Mathf.DeltaAngle(0f, transform.eulerAngles.z);

        float newZ = Mathf.SmoothDampAngle(currentZ, desiredZ, ref tiltVelocity, tiltSmoothTime);

        // Sets the new rotation
        transform.rotation = Quaternion.Euler(0f, 0f, newZ);
    }
}