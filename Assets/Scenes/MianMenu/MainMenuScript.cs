using UnityEditor.MPE;
using UnityEngine;

public class MainMenuScript : MonoBehaviour
{
    bool classSelected = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void LoadPlayScene(){
        if(classSelected){
            GameManager.Instance.LoadAScene("VS");
        }else{
            Debug.Log("You gotta select a class man");
        }
    }

    public void SelectClass(string className){
        GameManager.Instance.setPlayerClassSo(className);
        classSelected = true;
    }
}
