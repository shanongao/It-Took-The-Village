using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerTutorialInteraction : MonoBehaviour
{
    public enum State
    {
        Welcome,
        Walking,
        Talking,
        GrabbingWeapons,
        Attacking,
        Defending,
        Completed
    }

    //private bool isInTutorialZone = false;
    [SerializeField] private GameObject tutorialUi;
    [SerializeField] private string startScene;
    private DialogBoxController dialogBoxController;
    private State state;
    

    private string welcomeMessage = "Welcome to the tutorial, let's get started. First we'll " +
        "try to move around, use WASD keys to walk around the scene all 4 directions. Click Enter to start.";

    private string walkingMessage = "Nice, let's talk to some villagers now, approach " +
        "one of the villagers and follow the instructions to speak to them. Click Enter to continue";

    private string talkingMessage = "Great, let's arm ourselves now, find a weapon in the village " +
        "and walk towards it. Once you grab it, click X to open inventory and equip weapon. Click Enter to continue";

    private string weaponsMessage = "Awesome, you can toggle inventory using X. Now let's learn how to attack with" +
        " this weapon. Left mouse click to attack with the weapon. Click Enter to continue";

    private string attackingMessage = "Great, now let's defend. Use Right mouse click to defend. Click Enter to continue";

    private string defendingMessage = "Awesome, this is the end of the tutorial, Click Enter start the game";

    private void Awake()
    {
        dialogBoxController = tutorialUi.GetComponent<DialogBoxController>();
    }

    private void Start()
    {
        state = State.Welcome;
    }

    private void Update()
    {
        if (state == State.Welcome)
        {
            StartTutorial();
        }
        else if (state == State.Walking)
        {
            CheckWalking();
        }
        else if (state == State.Talking)
        {
            CheckTalking();
        }
        else if (state == State.GrabbingWeapons)
        {
            CheckWeapons();
        }
        else if (state == State.Attacking)
        {
            CheckAttacking();
        }
        else if (state == State.Defending)
        {
            CheckDefending();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                SceneManager.LoadScene(startScene);
            }
        }

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            dialogBoxController.HideDialog();
        }
    }

    void StartTutorial()
    {
        dialogBoxController.ShowDialog(welcomeMessage);
        state = State.Walking;
    }

    void CheckWalking()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) ||
            Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) ||
            Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) ||
            Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            dialogBoxController.WaitAndShowDialog(3, walkingMessage);
            state = State.Talking;
        }
    }

    void CheckTalking()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            dialogBoxController.WaitAndShowDialog(2, talkingMessage);
            state = State.GrabbingWeapons;
        }
    }

    void CheckWeapons()
    {
        if (gameObject.GetComponent<NewPlayerController>()._equippedWeapon != -1)
        {
            dialogBoxController.WaitAndShowDialog(0.5f, weaponsMessage);
            state = State.Attacking;
        }
    }

    void CheckAttacking()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dialogBoxController.WaitAndShowDialog(0.5f, attackingMessage);
            state = State.Defending;
        }
    }

    void CheckDefending()
    {
        if (Input.GetMouseButtonDown(1))
        {
            dialogBoxController.WaitAndShowDialog(0.5f, defendingMessage);
            state = State.Completed;
        }
    }
}
