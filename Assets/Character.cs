using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float SpeedTransition;
    public float SpeedRotation;
    public GameObject Eye;
    private Vector3 VelocityTransition = Vector3.zero;
    private Vector3 VelocityRotation = Vector3.zero;
    private const float VelocityEps = 1e-5f;
    
    // This function is taken from here: https://forum.unity.com/threads/quaternion-smoothdamp.793533/
    public static Quaternion SmoothDampQuaternion(Quaternion current, Quaternion target, ref Vector3 currentVelocity, float smoothTime)
    {
        Vector3 c = current.eulerAngles;
        Vector3 t = target.eulerAngles;
        return Quaternion.Euler(
            Mathf.SmoothDampAngle(c.x, t.x, ref currentVelocity.x, smoothTime),
            Mathf.SmoothDampAngle(c.y, t.y, ref currentVelocity.y, smoothTime),
            Mathf.SmoothDampAngle(c.z, t.z, ref currentVelocity.z, smoothTime)
        );
    }

    void Update()
    {
        Vector3 TargetTransition = Vector3.Normalize(new Vector3 (Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical")));
        TargetTransition = TargetTransition + transform.position;
        transform.position = Vector3.SmoothDamp(transform.position, TargetTransition, ref VelocityTransition, 1.0f / SpeedTransition, Mathf.Infinity, Time.deltaTime);
        if (Vector3.Magnitude(VelocityTransition) > VelocityEps) {
            Quaternion TargetRotation = Quaternion.LookRotation(VelocityTransition, Vector3.up);
            transform.rotation = SmoothDampQuaternion(transform.rotation, TargetRotation, ref VelocityRotation, 1.0f / SpeedRotation);
            Eye.transform.position = transform.position + transform.rotation * Vector3.forward * transform.localScale.z / 2.0f; // Last multiplier is the distance from the eye to the body. The radius of the capsule is scale.z / 2.
        }
    }
}
