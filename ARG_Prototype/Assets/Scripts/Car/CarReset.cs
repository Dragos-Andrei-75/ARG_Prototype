using UnityEngine;
using System.Collections;

public class CarReset : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private Transform carTransform;
    [SerializeField] private Rigidbody carRB;

    [Header("Script References")]
    [SerializeField] private CarController carController;
    [SerializeField] private Contact contact;

    [Header("Reset Car")]
    [SerializeField] private bool reset = false;
    [SerializeField] private float resetTime = 5.0f;

    private void Start()
    {
        carTransform = gameObject.GetComponent<Transform>();
        carRB = gameObject.GetComponent<Rigidbody>();

        carController = gameObject.GetComponent<CarController>();
        contact = gameObject.GetComponent<Contact>();
    }

    private void Update()
    {
        CallResetCar();
    }

    private void FixedUpdate()
    {
        ResetCarAccident();
    }

    private void CallResetCar()
    {
        if (contact.checkCarBodyContact == true && contact.CheckAccelerate() == false && contact.CheckSteer() == false)
        {
            reset = true;

            if (carController.reset == true)
            {
                resetTime = 0.0f;
            }
        }
        else
        {
            reset = false;
        }
    }

    private void ResetCarAccident()
    {
        if (reset == true)
        {
            resetTime -= Time.fixedDeltaTime;

            if (resetTime <= 0.0f)
            {
                carTransform.position = new Vector3(carTransform.position.x, carTransform.position.y + 1.25f, carTransform.position.z - 0.25f);

                if (contact.CheckSteer() == false || contact.CheckAccelerate() == false)
                {
                    carTransform.localEulerAngles = new Vector3(0, carTransform.localEulerAngles.y, 0);
                }

                carRB.velocity = Vector3.zero;
                carRB.angularVelocity = Vector3.zero;

                resetTime = 5.0f;
            }
        }
    }

    public IEnumerator ResetCarCheckPoint(Vector3 checkPointPosition, Vector3 checkPointRotation)
    {
        if (checkPointPosition != null)
        {
            yield return new WaitForSeconds(2.5f);

            carTransform.position = checkPointPosition;
            carTransform.eulerAngles = checkPointRotation;

            carRB.velocity = Vector3.zero;
            carRB.angularVelocity = Vector3.zero;
        }

        yield break;
    }
}
