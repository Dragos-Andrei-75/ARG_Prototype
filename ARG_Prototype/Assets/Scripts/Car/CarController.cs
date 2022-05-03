using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    [Header("Script References")]
    [SerializeField] private Contact contact;

    [Header("Component References")]
    [SerializeField] private Transform carTransform;
    [SerializeField] private Rigidbody carRB;

    [Header("Physics")]
    [SerializeField] private float gravityForce = 9.81f;
    public Vector3 carRBVelocityLS;

    [Header("Car Attributes")]
    [Tooltip("Specify the Drivetrain type of this vehicle(FWD, RWD, AWD).")]
    public string driveTrain = "RWD";
    public float topSpeedForward = 50.0f;
    public float topSpeedBackward = 25.0f;

    [Header("User Input")]
    [SerializeField] public  InputPlayerCar inputPlayerCar;
    [SerializeField] private InputAction inputActionAccelerate;
    [SerializeField] private InputAction inputActionSteer;
    [SerializeField] private InputAction inputActionReset;
    [SerializeField] private float inputAccelerate;
    [SerializeField] private float inputSteer;
    public float accelerate;
    public float steer;
    public bool reset;

    private float velocityAccelerate;
    private float velocitySteer;
    private float smoothTimeAccelerate;
    private float smoothTimeSteer = 0.05f;
    private float smoothDampMaxSpeed = 5.0f;

    private void Awake()
    {
        inputPlayerCar = new InputPlayerCar();

        inputActionAccelerate = inputPlayerCar.Car.Accelerate;
        inputActionAccelerate.started += _ => smoothTimeAccelerate = 1.0f;
        inputActionAccelerate.canceled += _ => smoothTimeAccelerate = 0.75f;

        inputActionSteer = inputPlayerCar.Car.Steer;

        inputActionReset = inputPlayerCar.Car.Reset;
        inputActionReset.started += _ => reset = true;
        inputActionReset.canceled += _ => reset = false;
    }

    private void Start()
    {
        contact = gameObject.GetComponent<Contact>();

        carTransform = gameObject.transform;
        carRB = gameObject.GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        inputPlayerCar.Enable();
        inputActionAccelerate.Enable();
        inputActionSteer.Enable();
        inputActionReset.Enable();
    }

    private void OnDisable()
    {
        inputPlayerCar.Disable();
        inputActionAccelerate.Disable();
        inputActionSteer.Disable();
        inputActionReset.Disable();
    }

    private void Update()
    {
        inputAccelerate = inputActionAccelerate.ReadValue<float>();
        inputSteer = inputActionSteer.ReadValue<float>();

        accelerate = Mathf.SmoothDamp(accelerate, inputAccelerate, ref velocityAccelerate, smoothTimeAccelerate, smoothDampMaxSpeed);

        if (inputAccelerate == 0)
        {
            accelerate = DecimalRound(accelerate, 2) == 0.0f ? 0.0f : accelerate;
        }

        steer = Mathf.SmoothDamp(steer, inputSteer, ref velocitySteer, smoothTimeSteer, smoothDampMaxSpeed);

        if (inputSteer == 0)
        {
            steer = DecimalRound(steer, 2) == 0.0f ? 0.0f : steer;
        }

        carRBVelocityLS = carTransform.InverseTransformDirection(carRB.velocity);
    }

    private void FixedUpdate()
    {
        Gravity();
        Accelerate();
    }

    public float DecimalRound(float value, int decimalPoint)
    {
        float decimalPrecision = 10.0f;

        for (int i = 0; i < decimalPoint - 1; i++)
        {
            decimalPrecision *= decimalPrecision;
        }

        if (decimalPoint == 0)
        {
            value = Mathf.Round(value);
        }
        else
        {
            value = Mathf.Round(value * decimalPrecision) / decimalPrecision;
        }

        return value;
    }

    private void Gravity()
    {
        int surfaceContactPoints = 0;

        for (int i = 0; i < contact.suspensionScripts.Length; i++)
        {
            if (contact.suspensionScripts[i].surfaceCheck == true)
            {
                surfaceContactPoints++;
            }
        }

        if (contact.checkCarBodyContact == false && surfaceContactPoints < 4)
        {
            carRB.AddForceAtPosition(Vector3.down * gravityForce, carTransform.position, ForceMode.Force);
        }
    }

    private void Accelerate()
    {
        Vector3 forcePosition;
        float forcePositionOffsetY;
        float forcePositionOffsetMin = 0.0f;
        float forcePositionOffsetMax = 0.25f;

        if (contact.CheckAccelerate() == true)
        {
            if (carRB.drag <= 1.0f)
            {
                forcePositionOffsetY = Mathf.Lerp(forcePositionOffsetMax, forcePositionOffsetMin, carRB.velocity.magnitude / (topSpeedForward * 0.5f));
            }
            else
            {
                forcePositionOffsetY = 0.0f;
            }

            forcePosition = carTransform.position - carTransform.TransformDirection(0.0f, forcePositionOffsetY, 0.0f);

            if (accelerate > 0.0f)
            {
                carRB.AddForceAtPosition(carTransform.forward * topSpeedForward * accelerate, forcePosition, ForceMode.Force);
            }
            else if (accelerate < 0.0f)
            {
                carRB.AddForceAtPosition(carTransform.forward * topSpeedBackward * accelerate, forcePosition, ForceMode.Force);
            }

            if (accelerate == 0.0f && Mathf.Abs(carRBVelocityLS.z) < 0.05f)
            {
                carRB.velocity = new Vector3(0.0f, carRB.velocity.y, 0.0f);
            }

            Grip();
        }
    }

    private void Grip()
    {
        float sidewaysVelocityNormalized;
        float sidewaysVelocityCounter;

        sidewaysVelocityNormalized = carRBVelocityLS.normalized.x;
        sidewaysVelocityCounter = topSpeedForward;

        if (Mathf.Abs(sidewaysVelocityNormalized) >= 0.05f)
        {
            carRB.AddForceAtPosition(-carTransform.right * sidewaysVelocityNormalized * sidewaysVelocityCounter, carTransform.position, ForceMode.Force);
        }
    }
}
