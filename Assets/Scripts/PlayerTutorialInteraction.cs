using UnityEngine;

public class PlayerTutorialInteraction : MonoBehaviour
{
    private bool isInTutorialZone = false;

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
        Debug.Log("Find your sword! Press space to kill enemies. Open Chest with E");
    }
}
