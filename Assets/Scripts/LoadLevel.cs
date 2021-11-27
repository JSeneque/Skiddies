using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadLevel : MonoBehaviour
{
    [SerializeField] private Image _progressBar;
    // Start is called before the first frame update
    void Start()
    {
        // call the coroutine to load the main menu
        StartCoroutine(LoadLevelAsync());
    }

    IEnumerator LoadLevelAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("RaceLevel1");

        // while the operation has not finished
        while (!operation.isDone)
        {
            // update the progress bar to match the operation
            _progressBar.fillAmount = operation.progress;
            yield return new WaitForEndOfFrame();
        }
    }

}
