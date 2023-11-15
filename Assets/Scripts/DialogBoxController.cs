using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogBoxController : MonoBehaviour
{
    public TextMeshProUGUI dialogText;
    public GameObject dialogPanel;

    private void Start()
    {
        HideDialog();
    }

    public void ShowDialog(string message)
    {
        dialogText.text = message;
        dialogPanel.SetActive(true);
    }

    public void HideDialog()
    {
        dialogPanel.SetActive(false);
        dialogText.text = "";
    }

    public void WaitAndShowDialog(float waitTime, string message)
    {
        StartCoroutine(WaitAndShowDialogCoroutine(waitTime, message));
    }

    private IEnumerator WaitAndShowDialogCoroutine(float waitTime, string message)
    {
        yield return new WaitForSeconds(waitTime);
        ShowDialog(message);
    }

    public bool isDialogOpen()
    {
        return dialogPanel.activeSelf;
    }
}
