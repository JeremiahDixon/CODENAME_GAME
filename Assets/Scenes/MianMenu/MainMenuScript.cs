using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] GameObject classSelectMenu;
    [SerializeField] GameObject _mainMenuFirst;
    [SerializeField] GameObject _classMenuFirst;
    [SerializeField] GameObject _settingsMenuFirst;
    [SerializeField] bool isMain;
    [SerializeField] bool isClassSelect;
    [SerializeField] bool isSettings;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isMain = true;
        EventSystem.current.SetSelectedGameObject(_mainMenuFirst);
    }

    void Update()
    {
        if(Gamepad.current != null && Gamepad.current.wasUpdatedThisFrame && EventSystem.current.currentSelectedGameObject == null)
        {
            if(isMain)
            {
                EventSystem.current.SetSelectedGameObject(_mainMenuFirst);
            }else if(isClassSelect)
            {
                EventSystem.current.SetSelectedGameObject(_classMenuFirst);
            }
        }
    }

    public void LoadClassSelectMenu(){
        isMain = false;
        isClassSelect = true;
        EventSystem.current.SetSelectedGameObject(_classMenuFirst);
        foreach (Transform child in transform) {
            child.gameObject.SetActive(false);
        }
        GetComponent<Image>().enabled = false;
        classSelectMenu.SetActive(true);
    }

    public void Exit(){
        Application.Quit();
    }

    public void LoadSettingsMenu()
    {
        //EventSystem.current.SetSelectedGameObject(_settingsMenuFirst);
        Debug.Log("Settings doesnt work yet!");
    }
}
