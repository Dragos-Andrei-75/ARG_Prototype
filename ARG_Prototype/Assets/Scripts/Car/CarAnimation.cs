using UnityEngine;

public class CarAnimation : MonoBehaviour
{
    [Header("Script References")]
    [SerializeField] private CarController carController;
    [SerializeField] private Contact contact;

    [Header("Component References")]
    [SerializeField] private Transform[] wheelTransforms;

    [Header("Wheel Attributes")]
    [SerializeField] private float ackermannSteerLeft;
    [SerializeField] private float ackermannSteerRight;
    [SerializeField] private float wheelTurnRadius = 5.0f;
    [SerializeField] private float steerForceMin = 0.375f;
    [SerializeField] private float steerForceMax = 1.0f;
    [SerializeField] private float steerForce;

    [Header("Car Attributes")]
    [SerializeField] private float rearTrack;
    [SerializeField] private float wheelBase;

    private void Start()
    {
        carController = gameObject.GetComponent<CarController>();
        contact = gameObject.GetComponent<Contact>();

        wheelTransforms = new Transform[gameObject.GetComponentsInChildren<Suspension>().Length];

        for (int i = 0; i < wheelTransforms.Length; i++)
        {
            wheelTransforms[i] = contact.suspensionScripts[i].transform.GetChild(0).transform;
        }

        rearTrack = Vector3.Distance(wheelTransforms[wheelTransforms.Length - 2].position, wheelTransforms[wheelTransforms.Length - 1].position);
        wheelBase = Vector3.Distance(wheelTransforms[0].position, wheelTransforms[contact.suspensionScripts.Length - 2].position);
    }

    private void Update()
    {
        WheelPosition();
        WheelTurn();
    }

    private void FixedUpdate()
    {
        WheelRotation();
    }

    private void WheelPosition()
    {
        for (int i = 0; i < wheelTransforms.Length; i++)
        {
            if (contact.suspensionScripts[i].surfaceCheck == true)
            {
                wheelTransforms[i].localPosition = new Vector3(wheelTransforms[i].localPosition.x, -contact.suspensionScripts[i].springLength, wheelTransforms[i].localPosition.z);
            }
            else
            {
                wheelTransforms[i].localPosition = new Vector3(wheelTransforms[i].localPosition.x, -contact.suspensionScripts[i].springMaxLength, wheelTransforms[i].localPosition.z);
            }
        }
    }

    private void WheelTurn()
    {
        float steerForceRatio = carController.DecimalRound(Mathf.Abs(carController.carRBVelocityLS.z) / carController.topSpeedForward, 3);

        steerForce = Mathf.Lerp(steerForceMax, steerForceMin, steerForceRatio);

        if (carController.steer > 0)
        {
            ackermannSteerLeft = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (wheelTurnRadius + (rearTrack / 2))) * carController.steer * steerForce;
            ackermannSteerRight = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (wheelTurnRadius - (rearTrack / 2))) * carController.steer * steerForce;
        }
        else if (carController.steer < 0)
        {
            ackermannSteerLeft = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (wheelTurnRadius - (rearTrack / 2))) * carController.steer * steerForce;
            ackermannSteerRight = Mathf.Rad2Deg * Mathf.Atan(wheelBase / (wheelTurnRadius + (rearTrack / 2))) * carController.steer * steerForce;
        }
        else
        {
            ackermannSteerLeft = 0;
            ackermannSteerRight = 0;
        }

        for (int i = 0; i < contact.wheelsTurn.Length; i++)
        {
            if (i % 2 == 0.0f)
            {
                wheelTransforms[i].parent.localEulerAngles = new Vector3(0, ackermannSteerLeft, 0);
            }
            else
            {
                wheelTransforms[i].parent.localEulerAngles = new Vector3(0, ackermannSteerRight, 0);
            }
        }
    }

    private void WheelRotation()
    {
        for (int i = 0; i < wheelTransforms.Length; i++)
        {
            wheelTransforms[i].Rotate(contact.suspensionScripts[i].wheelVelocityLS.z / contact.suspensionScripts[i].wheelRadius, 0, 0);
        }
    }
}
