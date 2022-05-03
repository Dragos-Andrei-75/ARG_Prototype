using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform carTransform;
    [SerializeField] private Rigidbody carRB;

    [Header("Camera Position")]
    [SerializeField] private Vector3 desiredPosition;
    [SerializeField] private Vector3 cameraFocus;
    [SerializeField] private float cameraFocusY = 1.25f;

    [Header("Camera Position Y")]
    [SerializeField] private float cameraPositionY;
    [SerializeField] private float desiredPositionY = 2.5f;
    [SerializeField] private float dampHeight;
    private float velocityY;

    [Header("Camera Position Z")]
    [SerializeField] private float cameraPositionZ;
    [SerializeField] private float desiredPositionZ = 5.0f;
    [SerializeField] private float distanceMultiplier = 0.05f;
    [SerializeField] private float dampDistance;
    private float velocityZ;

    [Header("Camera Rotation")]
    [SerializeField] private float cameraRotation;
    [SerializeField] private float desiredRotation;
    [SerializeField] private float rotationSnapTime = 0.15f;

    private void Start()
    {
        cameraTransform = gameObject.transform;

        carTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        carRB = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();

        cameraPositionY = desiredPositionY;
        cameraPositionZ = desiredPositionZ;

        cameraRotation = gameObject.transform.rotation.eulerAngles.y;

        desiredPosition = carTransform.localPosition;

        cameraFocus = new Vector3(0, cameraFocusY, 0);
    }

    private void LateUpdate()
    {
        CameraPosition();
    }

    private void CameraPosition()
    {
        float currentVelocity = carTransform.InverseTransformDirection(carRB.velocity).z;

        //Camera Position Y
        dampHeight = (currentVelocity / 50.0f) / Time.fixedDeltaTime;
        cameraPositionY = Mathf.Lerp(cameraPositionY, desiredPositionY, dampHeight * Time.fixedDeltaTime);

        //Camera Position Z
        dampDistance = (currentVelocity / 50.0f) / Time.fixedDeltaTime;
        cameraPositionZ = Mathf.SmoothDampAngle(cameraPositionZ, -(desiredPositionZ + (currentVelocity * distanceMultiplier)), ref velocityZ, dampDistance * Time.fixedDeltaTime);

        //Camera Rotation
        desiredRotation = carTransform.rotation.eulerAngles.y;
        cameraRotation = Mathf.SmoothDampAngle(cameraRotation, desiredRotation, ref velocityY, rotationSnapTime);

        //Desired Position
        desiredPosition = carTransform.position + (Quaternion.Euler(0, cameraRotation, 0) * new Vector3(0, cameraPositionY, cameraPositionZ));

        cameraTransform.position = desiredPosition;
        cameraTransform.LookAt(carTransform.position + cameraFocus);
    }
}
