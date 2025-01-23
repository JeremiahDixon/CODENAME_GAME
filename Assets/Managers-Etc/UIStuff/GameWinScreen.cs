using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class GameWinScreen : MonoBehaviour
{
    [SerializeField] GameObject _gameWinMenuFirst;
    const string MAIN_MENU_SCENE_NAME = "Main Menu";
    void Awake(){
    }
    void Update()
    {

    }
    public void GameWin()
    {
        GetComponent<Image>().enabled = true;
        foreach (Transform child in transform) {
            child.gameObject.SetActive(true);
        }
        if(Gamepad.current != null)
        {
            EventSystem.current.SetSelectedGameObject(_gameWinMenuFirst);
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
        GameManager.Instance.LoadAScene(MAIN_MENU_SCENE_NAME);
    }
}
