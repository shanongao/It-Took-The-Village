using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private GraphicRaycaster raycaster;
    // = gameObject.GetComponent<CanvasGroup>();
    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        raycaster = GetComponent<GraphicRaycaster>();
        raycaster.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyUp (KeyCode.Escape)) {
            if (canvasGroup.interactable) {
                raycaster.enabled = false;
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
                canvasGroup.alpha = 0f;
                Time.timeScale = 1f;
            } else {
                raycaster.enabled = true;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
                canvasGroup.alpha = 1f;
                Time.timeScale = 0f;
            }
        }
    }
}
