using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour {

    public Transform lookTarget;
    public Vector3 offset = new Vector3(0f, -2f, -1f);

    private void Start()
    {
        transform.position = lookTarget.position + offset;
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = lookTarget.position + offset;
        desiredPosition.x = 0f;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime);
    }
}
