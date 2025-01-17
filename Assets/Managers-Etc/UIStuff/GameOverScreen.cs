using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameOverScreen : MonoBehaviour
{

    [SerializeField] GameObject _gameOverMenuFirst;
    void Awake(){
    }

    void Update()
    {

    }
    public void GameOver()
    {
        GetComponent<Image>().enabled = true;
        foreach (Transform child in transform) {
            child.gameObject.SetActive(true);
        }
        if(Gamepad.current != null)
        {
            EventSystem.current.SetSelectedGameObject(_gameOverMenuFirst);
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
