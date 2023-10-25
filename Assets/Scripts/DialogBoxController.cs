using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    }
}
