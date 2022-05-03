using UnityEngine;

public class CarLap : MonoBehaviour
{
    public Transform checkPointTransform;

    public int lap;
    public int checkPointIndex;

    private void Start()
    {
        lap = 0;
        checkPointIndex = 0;
    }
}
