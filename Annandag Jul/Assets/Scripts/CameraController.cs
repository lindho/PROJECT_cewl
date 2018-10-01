using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    PlayerController player;

    private Transform target;
    public float mouseSensitivity = 10.0f;
    public float dstFromTarget = 3.0f;
    public float rotationSmoothTime = .12f;

    private Vector3 rotationSmoothVelocity;
    private Vector3 currentRotation;
    private Vector3 destination;

    public Vector2 pitchMinMax = new Vector2(-40, 85);

    public bool lockCursor;

    private float yaw;
    private float pitch;

    //Collision
    CollisionHandler collision;
    public bool smoothFollow = true;
    public float smooth = 0.05f;
    public float adjustmentDistance = 3;
    Vector3 adjustedDestination = Vector3.zero;
    Vector3 camVel = Vector3.zero;

	private void Start ()
    {
        lockCursor = true;

        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        player = GetComponent<PlayerController>();

        collision = GetComponent<CollisionHandler>();
        collision.Initialize(Camera.main);
        collision.UpdateCameraClipPoints(transform.position, transform.rotation, ref collision.adjustedCameraClipPoints);
        collision.UpdateCameraClipPoints(destination, transform.rotation, ref collision.desiredCameraClipPoints);

        SetCameraTarget(GameObject.Find("PlayerTarget").transform);
	}

	private void LateUpdate ()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= Input.GetAxis("Mouse Y")* mouseSensitivity;
        pitch = ClampAngle(pitch, ref pitchMinMax);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;
        
        transform.position = target.position - transform.forward * dstFromTarget;
    }

    void MoveToTarget()
    {
        ;
    }

    public void SetCameraTarget(Transform t)
    {
        this.target = t;
    }

    private static float ClampAngle(float angle, ref Vector2 pitchMinMax)
    {
        if (angle < -360.0f)
            angle += 360.0f;
        if (angle > 360.0f)
            angle -= 360.0f;
        return Mathf.Clamp(angle, pitchMinMax.x, pitchMinMax.y);
    }
}
