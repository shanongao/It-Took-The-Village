using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DoorMaze : MonoBehaviour
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

    private void OpenDoor()
    {
        if (door != null)
        {
            if (_playerController.mazeKey.activeSelf)
            {
                door.SetActive(false);
                if (door.TryGetComponent(out Collider collider))
                {
                    collider.enabled = false;
                }
                _playerController.mazeKey.SetActive(false);
                prompt.SetActive(false);
            }
            else
            {
                promptText = "The door is locked";
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
