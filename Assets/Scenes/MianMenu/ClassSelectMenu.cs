using UnityEngine;

public class ClassSelectMenu : MonoBehaviour
{   
    bool classSelected = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectClass(string className){
        GameManager.Instance.setPlayerClassSo(className);
        classSelected = true;
    }

    public void LoadPlayScene(){
        if(classSelected){
            GameManager.Instance.LoadAScene("VS");
        }else{
            Debug.Log("You gotta select a class man");
        }
    }
}
