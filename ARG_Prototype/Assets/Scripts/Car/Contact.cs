using UnityEngine;

public class Contact : MonoBehaviour
{
    [Header("Script References")]
    public Suspension[] suspensionScripts;
    public Suspension[] wheelsDrive;
    public Suspension[] wheelsTurn;

    [Header("Contact")]
    public bool checkCarBodyContact = false;

    private void Start()
    {
        suspensionScripts = new Suspension[gameObject.GetComponentsInChildren<Suspension>().Length];
        suspensionScripts = gameObject.GetComponentsInChildren<Suspension>();

        CheckWheels();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Map")
        {
            checkCarBodyContact = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        checkCarBodyContact = false;
    }

    private void CheckWheels()
    {
        int i;
        int wheelDriveCount = 0;
        int wheelSteerCount = 0;

        for (i = 0; i < suspensionScripts.Length; i++)
        {
            if (suspensionScripts[i].wheelDrive == true)
            {
                wheelsDrive = new Suspension[wheelsDrive.Length + 1];
            }

            if (suspensionScripts[i].wheelSteer == true)
            {
                wheelsTurn = new Suspension[wheelsTurn.Length + 1];
            }
        }

        for (i = 0; i < suspensionScripts.Length; i++)
        {
            if (suspensionScripts[i].wheelDrive == true)
            {
                wheelsDrive[wheelDriveCount] = suspensionScripts[i];
                wheelDriveCount++;
            }

            if (suspensionScripts[i].wheelSteer == true)
            {
                wheelsTurn[wheelSteerCount] = suspensionScripts[i];
                wheelSteerCount++;
            }
        }
    }

    public bool CheckAccelerate()
    {
        for (int i = 0; i < wheelsDrive.Length; i++)
        {
            if (wheelsDrive[i].surfaceCheck == true)
            {
                return true;
            }
        }

        return false;
    }

    public bool CheckSteer()
    {
        for (int i = 0; i < wheelsTurn.Length; i++)
        {
            if (wheelsTurn[i].surfaceCheck == true)
            {
                return true;
            }
        }

        return false;
    }
}
