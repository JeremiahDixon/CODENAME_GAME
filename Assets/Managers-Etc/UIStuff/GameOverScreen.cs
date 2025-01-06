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
        GameManager.Instance.ReloadCurrentScene();
    }
}
