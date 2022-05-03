using UnityEngine;
using System.Collections;

public class Charge : MonoBehaviour
{
    [Header("GameObject References")]
    [SerializeField] private GameObject charge;

    [Header("Charge Attributes")]
    [SerializeField] private float power = 1.0f;
    [SerializeField] private float respawnTime = 2.5f;

    private Vector3 positionA;
    private Vector3 positionB;
    private bool movesToA = true;
    private bool movesToB = false;
    private float rotationSpeed = 50.0f;

    private void Start()
    {
        charge = gameObject;

        positionA = new Vector3(charge.transform.position.x, charge.transform.position.y - 0.25f, charge.transform.position.z);
        positionB = new Vector3(charge.transform.position.x, charge.transform.position.y + 0.25f, charge.transform.position.z);
    }

    private void FixedUpdate()
    {
        MoveCharge();
    }

    private IEnumerator OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Battery>() == true)
        {
            Battery battery = other.GetComponent<Battery>();

            if (battery.chargeLevel < battery.chargeLimit)
            {
                other.transform.root.GetComponent<CarController>().topSpeedForward += power;
                battery.chargeLevel++;
            }

            charge.GetComponent<MeshRenderer>().enabled = false;
            charge.GetComponent<SphereCollider>().enabled = false;
        }

        yield return new WaitForSeconds(respawnTime);

        charge.GetComponent<MeshRenderer>().enabled = true;
        charge.GetComponent<SphereCollider>().enabled = true;
    }

    private void MoveCharge()
    {
        float moveSpeed = 0.0025f;

        if (movesToA == true)
        {
            if (charge.transform.position != positionA)
            {
                charge.transform.position = Vector3.MoveTowards(charge.transform.position, positionA, moveSpeed);
            }
            else
            {
                movesToA = false;
                movesToB = true;
            }
        }
        else if (movesToB == true)
        {
            if (charge.transform.position != positionB)
            {
                charge.transform.position = Vector3.MoveTowards(charge.transform.position, positionB, moveSpeed);
            }
            else
            {
                movesToA = true;
                movesToB = false;
            }
        }

        charge.transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.fixedDeltaTime));
    }
}
