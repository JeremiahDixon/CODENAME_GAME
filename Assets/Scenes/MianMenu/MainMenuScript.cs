using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] GameObject classSelectMenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void LoadClassSelectMenu(){
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
        Debug.Log("Settings doesnt work yet!");
    }
}
