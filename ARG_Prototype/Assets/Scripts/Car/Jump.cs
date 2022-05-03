using UnityEngine;

public class Jump : MonoBehaviour
{
    [Header("Script References")]
    [SerializeField] private CarController carController;
    [SerializeField] private Contact contact;

    [Header("Component References")]
    [SerializeField] private Transform carTransform;
    [SerializeField] private Rigidbody carRB;

    public float jumpTime;

    private void Start()
    {
        carController = gameObject.GetComponent<CarController>();
        contact = gameObject.GetComponent<Contact>();

        carTransform = gameObject.transform;
        carRB = gameObject.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        PerformJump();
        StabilizeCar();
    }

    private void PerformJump()
    {
        float jumpForce = 0.75f;

        if (jumpTime > 0.0f)
        {
            jumpTime -= Time.fixedDeltaTime;

            if (contact.CheckAccelerate() == true || contact.CheckSteer() == true)
            {
                carRB.AddForceAtPosition(Vector3.up * jumpForce, carTransform.position, ForceMode.Impulse);
            }
        }

        jumpTime = carController.DecimalRound(jumpTime, 2);
    }

    private void StabilizeCar()
    {
        float stabilizeForce;
        float stabilizeForceMax = 0.175f;

        if (contact.checkCarBodyContact == false && (contact.CheckAccelerate() == false || contact.CheckSteer() == false))
        {
            for (int i = 0; i < contact.suspensionScripts.Length; i++)
            {
                stabilizeForce = Mathf.Lerp(0.0f, stabilizeForceMax, Mathf.Abs(carTransform.localEulerAngles.x) / 45.0f);

                if (contact.suspensionScripts[i].suspensionTransform.position.y < carTransform.position.y - 0.075f)
                {
                    carRB.AddForceAtPosition(contact.suspensionScripts[i].suspensionTransform.up * stabilizeForce, contact.suspensionScripts[i].suspensionTransform.position, ForceMode.Force);
                }
                else if (contact.suspensionScripts[i].suspensionTransform.position.y > carTransform.position.y + 0.075f)
                {
                    carRB.AddForceAtPosition(-contact.suspensionScripts[i].suspensionTransform.up * stabilizeForce, contact.suspensionScripts[i].suspensionTransform.position, ForceMode.Force);
                }
            }
        }
    }
}
