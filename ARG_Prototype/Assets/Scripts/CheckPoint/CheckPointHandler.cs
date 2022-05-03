using UnityEngine;

public class CheckPointHandler : MonoBehaviour
{
    [Header("Checkpoint Object & Component References")]
    [SerializeField] private GameObject checkPoint;
    [SerializeField] private Transform checkPointTransform;

    [Header("Other Object & Components References")]
    [SerializeField] private GameObject carPlayer;
    [SerializeField] private CarReset carReset;
    [SerializeField] private CarLap carLap;
    [SerializeField] private LapDisplay lapDisplay;

    //Checkpoint Attributes
    [SerializeField] private static int checkPointAmount;
    public static int lapsTotal = 3;
    public int index;

    public delegate void Finish();
    public static event Finish OnFinish;

    private void Start()
    {
        checkPoint = gameObject;
        checkPointTransform = checkPoint.transform;

        carPlayer = GameObject.FindGameObjectWithTag("Player");
        carReset = carPlayer.GetComponent<CarReset>();
        carLap = carPlayer.transform.GetChild(0).GetComponent<CarLap>();

        lapDisplay = GameObject.Find("Laps").GetComponent<LapDisplay>();

        checkPointAmount++;

        index = checkPoint.transform.GetSiblingIndex() + 1;

        if (index == 1) carLap.checkPointTransform = checkPointTransform;

        lapDisplay.DisplayLaps(carLap.lap + 1, lapsTotal);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CarLap>() == true)
        {
            CarLap carLap = other.GetComponent<CarLap>();

            if (carLap.checkPointIndex == index)
            {
                return;
            }
            else if (carLap.checkPointIndex == index - 1)
            {
                carLap.checkPointIndex = index;
                carLap.checkPointTransform = checkPointTransform;
            }
            else if (carLap.checkPointIndex == checkPointAmount && index == 1)
            {
                carLap.checkPointIndex = 1;
                carLap.lap++;

                if (carLap.lap != lapsTotal)
                {
                    lapDisplay.DisplayLaps(carLap.lap + 1, lapsTotal);
                }
                else
                {
                    if (OnFinish != null) OnFinish();
                }
            }
            else
            {
                Vector3 checkPointPosition = new Vector3(carLap.checkPointTransform.position.x, 0.75f, carLap.checkPointTransform.position.z);
                Vector3 checkPointRotation = carLap.checkPointTransform.eulerAngles;

                StartCoroutine(carReset.ResetCarCheckPoint(checkPointPosition, checkPointRotation));
            }
        }
    }
}
