using UnityEngine;
using UnityEngine.InputSystem;

public class Pause : MonoBehaviour
{
    [Header("Input")]
    public InputMenu inputMenu;
    public InputAction inputPause;

    private CarController carController;

    private bool pause = false;

    public delegate void ActionPause();
    public static ActionPause onPause;

    private void Awake()
    {
        inputMenu = new InputMenu();

        inputPause = inputMenu.PauseMenu.Pause;
        inputPause.started += _ => PauseResume();
    }

    private void Start()
    {
        carController = GameObject.FindGameObjectWithTag("Player").GetComponent<CarController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnEnable()
    {
        InputEnable();
    }

    private void OnDisable()
    {
        InputDisable();
    }

    public void InputEnable()
    {
        inputMenu.Enable();
        inputPause.Enable();
    }

    public void InputDisable()
    {
        inputMenu.Disable();
        inputPause.Disable();
    }

    public void PauseResume()
    {
        pause = !pause;

        if (pause == true)
        {
            Time.timeScale = 0;
            AudioListener.pause = true;

            carController.inputPlayerCar.Disable();

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1;
            AudioListener.pause = false;

            carController.inputPlayerCar.Enable();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (onPause != null) onPause();
    }
}
