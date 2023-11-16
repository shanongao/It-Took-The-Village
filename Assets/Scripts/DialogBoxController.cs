using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DialogBoxController : MonoBehaviour
{
    public TextMeshProUGUI dialogText;
    public GameObject dialogPanel;
    private Queue<string> dialogQueue;
    private bool isDisplayingDialog;

    private void Start()
    {
        //dialogQueue = new Queue<string>();
        //HideDialog();
    }

    public void ShowDialog(string message)
    {
        dialogPanel.SetActive(true);
        dialogText.text = message;
        //dialogQueue.Enqueue(message);

        //if (!isDisplayingDialog)
        //{
        //    StartCoroutine(DisplayDialogFromQueue());
        //}
    }

    //private IEnumerator DisplayDialogFromQueue()
    //{
    //    while (dialogQueue.Count > 0)
    //    {
    //        isDisplayingDialog = true;
    //        string messageToShow = dialogQueue.Dequeue();
    //        dialogText.text = messageToShow;
    //        dialogPanel.SetActive(true);

    //        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space)); // Wait for player input to continue
    //        HideDialog();
    //    }

    //    isDisplayingDialog = false;
    //}

    public void HideDialog()
    {
        dialogPanel.SetActive(false);
        dialogText.text = "";
    }

    public bool isDialogOpen()
    {
        return dialogPanel.activeSelf;
    }

    public void WaitAndShowDialog(float waitTime, string message)
    {
        StartCoroutine(WaitAndShowDialogCoroutine(waitTime, message));
        Debug.Log("ttw");
    }

    private IEnumerator WaitAndShowDialogCoroutine(float waitTime, string message)
    {
        yield return new WaitForSeconds(waitTime);
        ShowDialog(message);
    }
}
