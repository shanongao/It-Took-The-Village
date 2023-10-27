using UnityEngine;
using UnityEngine.SceneManagement;

public class openScene : MonoBehaviour
{
    [SerializeField]
    private string sceneName;

    public void OpenScene()
    {
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OpenScene();
        }
    }

}
