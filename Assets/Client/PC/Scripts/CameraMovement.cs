using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    public float speed = 10.0f;
    public float sensitivity = 100.0f;
    public float clampAngle = 70.0f;

    private float rotX;
    private float rotY;

    public Transform cameraTransform;
    public Vector3 dirNormalized;
    public Vector3 finalDir;
    public float minDistance;
    public float maxDistance;
    public float finalDistance;

    public float smoothness;
    // Start is called before the first frame update
    void Start()
    {
        rotX = transform.localRotation.eulerAngles.x;
        rotY = transform.localRotation.eulerAngles.y;

        dirNormalized = cameraTransform.localPosition.normalized;
        finalDistance = cameraTransform.localPosition.magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        rotX += -(Input.GetAxis("Mouse Y")) * sensitivity * Time.deltaTime;
        rotY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
        Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
        transform.rotation = rot;
    }

    private void LateUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position,target.position,speed*Time.deltaTime);

        finalDir = transform.TransformPoint(dirNormalized * maxDistance);

        RaycastHit hit;
        if (Physics.Linecast(transform.position,finalDir,out hit))
        {
            finalDistance=Mathf.Clamp(hit.distance,minDistance,maxDistance);
        } 
        else
        {
            finalDistance = maxDistance;
        }
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, dirNormalized * finalDistance, Time.deltaTime * smoothness);
        transform.LookAt(target);
     }
}
