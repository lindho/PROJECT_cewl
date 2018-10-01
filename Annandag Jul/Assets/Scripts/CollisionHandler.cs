using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CollisionHandler : MonoBehaviour
{
    public LayerMask collisionLayer;

    [HideInInspector]
    public bool colliding = false;
    [HideInInspector]
    public Vector3[] adjustedCameraClipPoints;
    [HideInInspector]
    public Vector3[] desiredCameraClipPoints;

    [SerializeField]
    private float collisionCussion = 3.41f;

    Camera cam;

    public void Initialize(Camera cam)
    {
        this.cam = cam;
        adjustedCameraClipPoints = new Vector3[5];
        desiredCameraClipPoints = new Vector3[5];
    }

    public void UpdateCameraClipPoints(Vector3 cameraPosition, Quaternion atRotaion, ref Vector3[] intoArray)
    {
        if (!cam)
            return;

        intoArray = new Vector3[5];

        float z = cam.nearClipPlane;
        float x = Mathf.Tan(cam.fieldOfView / collisionCussion) * z;
        float y = x / cam.aspect;

        intoArray[0] = (atRotaion * new Vector3(-x, y, z)) + cameraPosition;

        intoArray[1] = (atRotaion * new Vector3(x, y, z)) + cameraPosition;

        intoArray[2] = (atRotaion * new Vector3(-x, -y, z)) + cameraPosition;

        intoArray[3] = (atRotaion * new Vector3(x, -y, z)) + cameraPosition;

        intoArray[4] = cameraPosition - cam.transform.forward;

    }

    bool CollisionDetectedAtClipPoints(Vector3[] clipPoints, Vector3 targetPosition)
    {
        for (int i = 0; i < clipPoints.Length; i++)
        {
            Ray ray = new Ray(targetPosition, clipPoints[i] - targetPosition);
            float distance = Vector3.Distance(clipPoints[i], targetPosition);

            if (Physics.Raycast(ray, distance, collisionLayer, QueryTriggerInteraction.Ignore))
            {
                return true;
            }
        }
        return false;
    }


    public float GetAdjustedDistanceWithRayFrom(Vector3 targetPosition)
    {
        float distance = -1;

        for (int i = 0; i < desiredCameraClipPoints.Length; i++)
        {
            Ray ray = new Ray(targetPosition, desiredCameraClipPoints[i] - targetPosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f, collisionLayer, QueryTriggerInteraction.Ignore))
            {
                if (distance == -1)
                    distance = hit.distance;
                else
                {
                    if (hit.distance < distance)
                        distance = hit.distance;
                }
            }
        }

        if (distance == -1)
            return 0;
        else
            return distance;
    }

    public bool CheckColliding(Vector3 targetPosition)
    {
        if (CollisionDetectedAtClipPoints(desiredCameraClipPoints, targetPosition))
        {
            colliding = true;

        }
        else
        {
            colliding = false;
        }
        return colliding;
    }

}

