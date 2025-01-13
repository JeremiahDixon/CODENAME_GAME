using UnityEngine;
using UnityEngine.UI;

public class GameWinScreen : MonoBehaviour
{

    void Awake(){
    }
    public void GameWin()
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
