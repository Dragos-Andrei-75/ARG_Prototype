using UnityEngine;

public class Drag : MonoBehaviour
{
    [Header("Script References")]
    [SerializeField] private Contact contact;

    [Header("Component References")]
    [SerializeField] private Rigidbody carRB;

    [Header("Layers")]
    [SerializeField] private LayerMask layerTrack;
    [SerializeField] private LayerMask layerGround;

    [Header("Drag")]
    [SerializeField] private float drag;
    [SerializeField] private float dragAir = 0.1f;
    [SerializeField] private float dragTrack = 1.0f;
    [SerializeField] private float dragGround = 2.5f;
    [SerializeField] private float angularDragAir = 7.5f;
    [SerializeField] private float angularDragSurface = 2.5f;
    [SerializeField] private float[] wheelsDrag;

    private void Start()
    {
        contact = gameObject.GetComponent<Contact>();

        carRB = gameObject.GetComponent<Rigidbody>();

        layerTrack = LayerMask.NameToLayer("Track");
        layerGround = LayerMask.NameToLayer("Ground");

        wheelsDrag = new float[gameObject.GetComponentsInChildren<Suspension>().Length];
    }

    private void FixedUpdate()
    {
        CalculateDrag();
    }

    private void CalculateDrag()
    {
        for (int i = 0; i < contact.suspensionScripts.Length; i++)
        {
            if (contact.suspensionScripts[i].surfaceCheck == true)
            {
                if (contact.suspensionScripts[i].surfaceHit.collider.gameObject.layer == layerTrack)
                {
                    wheelsDrag[i] = dragTrack;
                }
                else if (contact.suspensionScripts[i].surfaceHit.collider.gameObject.layer == layerGround)
                {
                    wheelsDrag[i] = dragGround;
                }

                carRB.angularDrag = angularDragSurface;
            }
            else
            {
                wheelsDrag[i] = dragAir;
                carRB.angularDrag = angularDragAir;
            }

            drag += wheelsDrag[i];
        }

        carRB.drag = drag / wheelsDrag.Length;
        drag = 0.0f;
    }
}
