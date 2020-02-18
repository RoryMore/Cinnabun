using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform focusTransform;
    //public CameraShake cameraShake;

    [Header("Speed")]
    float xSpeed = 60.0f;
    [Range(0.0f, 1.0f)]
    [Tooltip("A fraction at which the rotation and position will assume it's new position")]
    public float lerpSpeed = 0.2f;

    [Header("Distances From Focus Point")]
    [Range(0, 100)]
    [Tooltip("The minimum distance the camera can be to the Focused Transform")]
    public float minDistance = 10.0f;
    [Range(0, 100)]
    [Tooltip("The maximum distance the camera can be from the Focused Transform")]
    public float maxDistance = 100.0f;
    [Range(0, 100)]
    [Tooltip("Initial distance the camera will move to from the Focused Transform")]
    public float targetDistance = 25.0f;

    [Header("Mouse Control")]
    public bool invertMouseXAxis = false;
    [Range(0.5f, 10)]
    public float mouseScrollSens = 2.0f;
    [Range(0.5f, 10)]
    public float mouseXSens = 2.0f;

    float xAngle = 0.0f;

    [SerializeField]
    [Range(35.0f, 85.0f)]
    float yAngle = 65.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Change and set Camera targetDistance from focused point if mouse wheel Input is recieved
        targetDistance = Mathf.Clamp(targetDistance - Input.mouseScrollDelta.y * mouseScrollSens, minDistance, maxDistance);

        // Hide and lock cursor when RMB is pressed
        if (Input.GetMouseButtonDown(2))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
        // Reveal and unlock cursor when RMB is released
        if (Input.GetMouseButtonUp(2))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        float dist = targetDistance;
        Quaternion nextRotation = Quaternion.Euler(yAngle, xAngle, 0);

        // While RMB is held
        if (Input.GetMouseButton(2))
        {
            xAngle += Input.GetAxis("Mouse X") * xSpeed * mouseXSens * Time.unscaledDeltaTime * (invertMouseXAxis ? -1.0f : 1.0f);

            // If there is any object obstructing vision from the Camera to Focused Transform
            if (Physics.Linecast(focusTransform.position, transform.position, out RaycastHit hit))
            {
                // Make an extra check here to see if it is an entity, instead of a building or something
                // Entities aren't "obstructing" to the camera, as buildings and terrain are
                // Example code
                //if (!hit.collider.gameObject.tag.Contains("Entity"))
                //{
                //    dist -= hit.distance;
                //}
            }
        }

        yAngle = ClampAngle(yAngle, 35.0f, 85.0f);
        xAngle = ClampAngle(xAngle, -360.0f, 360.0f);

        Vector3 negDistance = new Vector3(0.0f, 0.0f, -dist);
        Vector3 nextPosition = nextRotation * negDistance + focusTransform.position;

        transform.rotation = Quaternion.Slerp(transform.rotation, nextRotation, lerpSpeed);
        transform.position = Vector3.Slerp(transform.position, nextPosition, lerpSpeed);
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

    public IEnumerator cShake(float duration, float amount)
    {
      
        float elapsed = 0.0f;

        if (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * amount;
            float y = Random.Range(-1f, 1f) * amount;
            Vector3 shakeVector = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z);
           
            transform.position = Vector3.Lerp(transform.position, shakeVector, 1f);
           
            elapsed += Time.deltaTime;

            yield return null;
        }

        //transform.localPosition = transform.position;

    }
}
