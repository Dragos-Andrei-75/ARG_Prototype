using UnityEngine;

public class AntiRollBar : MonoBehaviour
{
    [Header("Script References")]
    [SerializeField] private CarController carController;
    [SerializeField] private Suspension[] wheelsOnAxle;

    [Header("Component References")]
    [SerializeField] private Rigidbody carRB;

    [Header("AntiRoll Bar Attributes")]
    [SerializeField] private float lengthDifference;
    [SerializeField] private float antiRollBarStiffness = 5.0f;
    [SerializeField] private float antiRollBarForce;

    private void Start()
    {
        carController = gameObject.transform.root.GetComponent<CarController>();
        wheelsOnAxle = GetComponentsInChildren<Suspension>();

        carRB = gameObject.transform.root.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        AntiRollBarBehabiour();
    }

    private void AntiRollBarBehabiour()
    {
        for (int i = 0; i < wheelsOnAxle.Length; i++)
        {
            if (wheelsOnAxle[i].surfaceCheck == true)
            {
                lengthDifference = (wheelsOnAxle[0].springLength - wheelsOnAxle[1].springLength) / ((wheelsOnAxle[0].springRestLength + wheelsOnAxle[1].springRestLength) / 2);
            }
            else
            {
                lengthDifference = 0;
            }

            lengthDifference = carController.DecimalRound(lengthDifference, 3);
        }

        antiRollBarForce = lengthDifference * antiRollBarStiffness;
        antiRollBarForce = carController.DecimalRound(antiRollBarForce, 3);

        if (wheelsOnAxle[0].surfaceCheck == true)
        {
            carRB.AddForceAtPosition(-antiRollBarForce * wheelsOnAxle[0].transform.up, wheelsOnAxle[0].transform.position);
        }

        if (wheelsOnAxle[1].surfaceCheck == true)
        {
            carRB.AddForceAtPosition(antiRollBarForce * wheelsOnAxle[1].transform.up, wheelsOnAxle[1].transform.position);
        }
    }
}
