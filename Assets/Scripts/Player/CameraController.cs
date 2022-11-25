using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float smoothTime;    // Time required for the cam to reach the player

    public Vector3 offset = new Vector3(0, 2, -10); // Base vector prevent AxisZ from going to 0
    private Vector3 velocity = Vector3.zero;         // 0 speed for the SmoothDamp

    private void Update()
    {
        Vector3 targetPosition = target.position + offset;

        // It will follow the player position delaying smoothTime for better feel
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
