using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealerHandler : MonoBehaviour
{
    public static ShopHandler ActiveDialogHandler;

    public GameObject prompt;
  
    public GameObject dialogCanvas;
    public string promptText;
    public string healthIncreaseText;
    public int increaseHealthBy;

    private TextMeshProUGUI text;
    private bool inDialog;
    private bool isInRange;
    private GameObject player;
    private NewPlayerController playerController;



    private TextMeshProUGUI promtTextString;

    private void Awake()
    {
        text = dialogCanvas.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        prompt.SetActive(false);
        dialogCanvas.SetActive(false);
        promtTextString = prompt.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        promtTextString.SetText(promptText);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            playerController = player.GetComponent<NewPlayerController>(); // Updated to NewPlayerController
            isInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
            promtTextString.SetText(promptText);
        }
    }

    private void Update()
    {
        prompt.SetActive(isInRange && !inDialog);

        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (inDialog)
            {
                CloseDialog();
                ResumePlayerMovement();
                inDialog = false;
            }
            else
            {
                prompt.SetActive(false);
                DisplayDialog();
                PausePlayerMovement();
                inDialog = true;
            }
        }

        if (inDialog && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            player.GetComponent<NewPlayerController>().IncreaseHealth(increaseHealthBy);
            CloseDialog();
            ResumePlayerMovement();
            inDialog = false;
        }
    }

    private void DisplayDialog()
    {
        dialogCanvas.SetActive(true);
        text.SetText(healthIncreaseText);
    }

    public void CloseDialog()
    {
        dialogCanvas.SetActive(false);
    }

    private void PausePlayerMovement()
    {
        if (playerController != null)
        {
            playerController.enabled = false;
        }
    }

    private void ResumePlayerMovement()
    {
        if (playerController != null)
        {
            playerController.enabled = true;
        }
    }


}
