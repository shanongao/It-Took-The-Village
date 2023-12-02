using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Door1 : MonoBehaviour
{
    public GameObject door;
    public GameObject prompt;
    private NewPlayerController _playerController;
    private string promptText;
    
    private bool isInRange = false;

    private void Start()
    {
        prompt.SetActive(false);
        _playerController = GameObject.FindWithTag("Player").GetComponent<NewPlayerController>();
    }

    private void Update()
    {
        if (isInRange)
        {
            prompt.SetActive(true);
            TextMeshProUGUI tmp = prompt.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            tmp.SetText(promptText);

            if (Input.GetKeyDown(KeyCode.E))
            {
                OpenDoor();
            }
        }
    }
    private bool AreAllEnemiesDefeated()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Room1Enemy");
        return enemies.Length == 0;
    }


    private void OpenDoor()
    {
        if (door != null)
        {
            if (AreAllEnemiesDefeated())
            {
                door.SetActive(false);
                if (door.TryGetComponent(out Collider collider))
                {
                    collider.enabled = false;
                }
                prompt.SetActive(false);
            }
            else
            {
                promptText = "The door is locked. Defeat all enemies to unlock.";
                TextMeshProUGUI tmp = prompt.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
                tmp.SetText(promptText);
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = true;
            promptText = "Press E to open";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
            prompt.SetActive(false);
        }
    }
}
