using UnityEngine;

public class PlayerTutorialInteraction : MonoBehaviour
{
    private bool isInTutorialZone = false;
    private DialogBoxController dialogBoxController;

    private void Awake()
    {
        // Find and assign the DialogBoxController component
        dialogBoxController = FindObjectOfType<DialogBoxController>();
        if (dialogBoxController == null)
        {
            Debug.LogError("DialogBoxController not found in the scene!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TutorialNPC"))
        {
            isInTutorialZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TutorialNPC"))
        {
            isInTutorialZone = false;
            if (dialogBoxController != null)  // Check if dialogBoxController is not null before using it
            {
                dialogBoxController.HideDialog();  // Hide dialog when player leaves the tutorial zone
            }
        }
    }

    private void Update()
    {
        if (isInTutorialZone && Input.GetKeyDown(KeyCode.E))
        {
            DisplayTutorialInstructions();
        }
    }

    private void DisplayTutorialInstructions()
    {
        if (dialogBoxController != null)  // Check if dialogBoxController is not null before using it
        {
            dialogBoxController.ShowDialog("Find your sword! Press space to kill enemies. Open Chest with E");
        }
    }
}