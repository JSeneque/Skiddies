using UnityEngine;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _quitButton;

    private void Start()
    {

#if UNITY_STANDALONE_WIN
        _quitButton.SetActive(true);
#endif
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("LoadingScreen");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
