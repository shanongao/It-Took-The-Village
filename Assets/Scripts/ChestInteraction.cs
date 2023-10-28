using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChestInteraction : MonoBehaviour
{
    public GameObject chestClosed;
    public GameObject chestOpen;
    public GameObject prompt;
    public GameObject key;
    public string promptText;
    private bool isInRange = false;
    private bool isOpened = false;

    private void Start()
    {
        prompt.SetActive(false);
        key.SetActive(false);
    }

    void Update()
    {
        if (isInRange && !isOpened)
        {
            prompt.SetActive(true);
            TextMeshProUGUI tmp = prompt.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
            tmp.SetText(promptText);
            if (Input.GetKeyDown(KeyCode.E))
            {
                OpenChest();
            }
        }
    }

    private void OpenChest()
    {
        isOpened = true;
        chestClosed.SetActive(false);
        chestOpen.SetActive(true);
        prompt.SetActive(false);
        key.SetActive(true);
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
            prompt.SetActive(false);
        }
    }
}
