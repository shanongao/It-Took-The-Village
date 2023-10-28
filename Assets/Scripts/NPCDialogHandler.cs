using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPCDialogHandler : MonoBehaviour
{
    public GameObject prompt;
    public GameObject dialogCanvas;
    public string promptText;
    [SerializeField] public string[] dialogueText;
    private int dialogIndex = 0;

    private TextMeshProUGUI text;
    private bool inDialog;
    private bool isInRange;

    private void Awake()
    {
        
    }

    private void Start()
    {
        // dialogCanvas = GameObject.FindWithTag("DialogCanvas");
        // prompt = GameObject.FindWithTag("ButtonPrompt");
        text = dialogCanvas.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        prompt.SetActive(false);
        dialogCanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
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
        }
    }

    private void Update()
    {
        if (inDialog && Input.GetKeyDown(KeyCode.E))
        {
            if (dialogIndex >= dialogueText.Length)
            {
                dialogIndex = 0;
                dialogCanvas.SetActive(false);
                inDialog = false;
                return;
            }
            else
            {
                DisplayDialog();
            }
        }

        if (isInRange && !inDialog)
        {
            prompt.SetActive(true);
            TextMeshProUGUI tmp = prompt.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            tmp.SetText(promptText);
            if (Input.GetKeyDown(KeyCode.E))
            {
                inDialog = true;
                prompt.SetActive(false);
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
}