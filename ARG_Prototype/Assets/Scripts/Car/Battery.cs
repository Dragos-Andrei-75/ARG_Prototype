using UnityEngine;

public class Battery : MonoBehaviour
{
    public int chargeLevel;
    public int chargeLimit;

    private void Start()
    {
        chargeLevel = 0;
        chargeLimit = 10;
    }
}
