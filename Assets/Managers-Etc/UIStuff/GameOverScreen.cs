using UnityEngine;
using UnityEngine.UI;

public class GameOverScreen : MonoBehaviour
{

    void Awake(){
    }
    public void GameOver()
    {
        GetComponent<Image>().enabled = true;
        foreach (Transform child in transform) {
            child.gameObject.SetActive(true);
        }
    }

    public void Restart(){
        Time.timeScale = 1;
        GameManager.Instance.ReloadCurrentScene();
    }

    public void Exit(){
        Time.timeScale = 1;
        Application.Quit();
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        GameManager.Instance.LoadAScene("MainMenu");
    }
}
