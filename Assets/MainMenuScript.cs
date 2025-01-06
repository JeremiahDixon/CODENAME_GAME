using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void LoadPlayScene(){
        GameManager.Instance.LoadAScene("VS");
    }

    public void SelectClass(string className){
        GameManager.Instance.setPlayerClassSo(className);
    }
}
