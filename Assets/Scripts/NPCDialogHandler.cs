using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCDialogHandler : MonoBehaviour
{
    public static NPCDialogHandler ActiveDialogHandler;

    public GameObject prompt;
    public GameObject dialogCanvas;
    public string promptText;
    [SerializeField] private string[] dialogueText;
    private int dialogIndex = 0;

    private TextMeshProUGUI text;
    private bool inDialog;
    private bool isInRange;
    private GameObject player;
    private NewPlayerController playerController;

    private void Awake()
    {
        text = dialogCanvas.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        prompt.SetActive(false);
        dialogCanvas.SetActive(false);
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
            dialogIndex = 0;
            prompt.SetActive(false);
            CloseDialog();
            ResumePlayerMovement();
        }
    }

    private void Update()
    {
        if (isInRange && !inDialog)
        {
            prompt.SetActive(true);
            TextMeshProUGUI tmp = prompt.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            tmp.SetText(promptText);
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (ActiveDialogHandler != null)
                {
                    ActiveDialogHandler.CloseDialog();
                }

                ActiveDialogHandler = this;
                inDialog = true;
                prompt.SetActive(false);
                DisplayDialog();
                PausePlayerMovement();
            }
        }

        if (inDialog && Input.GetKeyDown(KeyCode.E))
        {
            if (dialogIndex >= dialogueText.Length)
            {
                dialogIndex = 0;
                CloseDialog();
                ResumePlayerMovement();
                return;
            }
            else
            {
                DisplayDialog();
            }
        }
    }

    private void DisplayDialog()
    {
        dialogCanvas.SetActive(true);
        text.SetText(dialogueText[dialogIndex]);
        dialogIndex++;
    }

    public void CloseDialog()
    {
        dialogCanvas.SetActive(false);
        inDialog = false;
        ActiveDialogHandler = null;
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
