using UnityEngine;

public class Suspension : MonoBehaviour
{
    [Header("Script References")]
    [SerializeField] private CarController carController;

    [Header("Component References")]
    [SerializeField] public Transform suspensionTransform;
    [SerializeField] private Rigidbody carRB;

    [Header("Surface Check")]
    public bool surfaceCheck = false;
    public RaycastHit surfaceHit;
    private int carLayer;

    [Header("Spring Attributes")]
    [SerializeField] public float springRestLength = 0.25f;
    [SerializeField] private float springTravelUp = 0.25f;
    [SerializeField] private float springTravelDown = 0.25f;
    [SerializeField] private float springMinLength;
    [SerializeField] public float springMaxLength;
    [SerializeField] private float previousSpringLength;
    [SerializeField] public float springLength;
    [SerializeField] private float springStiffness = 10.0f;
    [SerializeField] private float springForce;
    [SerializeField] private float springVelocity;

    [Header("Damper Attributes")]
    [SerializeField] private float damperStiffness = 1.0f;
    [SerializeField] private float damperForce;

    [Header("Suspension Attributes")]
    [SerializeField] private Vector3 suspensionForce;
    [SerializeField] private float counterForce = 2.5f;

    [Header("Wheel Attributes")]
    public bool wheelDrive;
    public bool wheelSteer;
    public float wheelRadius;
    public Vector3 wheelVelocityLS;
    [SerializeField] private float wheelLateralForce;
    [SerializeField] private float wheelLateralForceMin = 0.25f;
    [SerializeField] private float wheelLateralForceMax = 1.25f;
    [SerializeField] private Vector3 wheelLateralVelocityForce;

    private void Start()
    {
        carController = gameObject.transform.root.GetComponent<CarController>();

        suspensionTransform = gameObject.transform;
        carRB = gameObject.transform.root.GetComponent<Rigidbody>();

        carLayer = gameObject.transform.root.GetChild(0).gameObject.layer;

        springMinLength = springRestLength - springTravelDown;
        springMaxLength = springRestLength + springTravelUp;

        if (suspensionTransform.position.z > suspensionTransform.root.position.z)
        {
            wheelSteer = true;
        }

        if (carController.driveTrain == "FWD")
        {
            wheelDrive = suspensionTransform.position.z > suspensionTransform.root.position.z ? true : false;
        }
        else if (carController.driveTrain == "RWD")
        {
            wheelDrive = suspensionTransform.position.z < suspensionTransform.root.position.z ? true : false;
        }
        else if (carController.driveTrain == "AWD")
        {
            wheelDrive = true;
        }

        wheelRadius = gameObject.transform.GetChild(0).GetComponent<Transform>().lossyScale.y / 2.0f;
    }

    private void Update()
    {
        wheelVelocityLS = suspensionTransform.InverseTransformDirection(carRB.GetPointVelocity(surfaceHit.point));
    }

    private void FixedUpdate()
    {
        CheckSurface();
        SuspensionBehaviour();
        WheelLateralForce();
    }

    private void CheckSurface()
    {
        surfaceCheck = Physics.Raycast(suspensionTransform.position, -suspensionTransform.up, out surfaceHit, springMaxLength + wheelRadius, ~carLayer);
        Debug.DrawRay(suspensionTransform.position, -suspensionTransform.up * surfaceHit.distance, Color.green);
    }

    private void SuspensionBehaviour()
    {
        if (surfaceCheck == true)
        {
            previousSpringLength = springLength;
            springLength = surfaceHit.distance - wheelRadius;
            springLength = Mathf.Clamp(springLength, springMinLength, springMaxLength);

            springVelocity = (previousSpringLength - springLength) / Time.fixedDeltaTime;

            springForce = springStiffness * (1.0f - (springLength / springMaxLength));
            damperForce = damperStiffness * springVelocity;

            suspensionForce = suspensionTransform.up * (springForce + damperForce) + (-suspensionTransform.up * counterForce);

            carRB.AddForceAtPosition(suspensionForce, surfaceHit.point, ForceMode.Force);
        }
    }

    private void WheelLateralForce()
    {
        float wheelLateralForceRatio;

        if (surfaceCheck == true)
        {
            wheelLateralForceRatio = carController.DecimalRound(Mathf.Abs(wheelVelocityLS.z) / (carController.topSpeedForward * 0.5f), 3);

            wheelLateralForce = Mathf.Lerp(wheelLateralForceMax, wheelLateralForceMin, wheelLateralForceRatio);
            wheelLateralVelocityForce = -suspensionTransform.right * wheelVelocityLS.x * wheelLateralForce;

            carRB.AddForceAtPosition(wheelLateralVelocityForce, surfaceHit.point, ForceMode.Force);
        }
    }
}
