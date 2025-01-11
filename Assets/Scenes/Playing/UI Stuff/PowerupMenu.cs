using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Linq;
using System.Collections.Generic;

public class PowerupMenu : MonoBehaviour
{
    GameObject healthManager;
    GameObject option1;
    GameObject option2;
    GameObject option3;
    public GameObject[] allPowerups;
    public List<GameObject> possiblePowerups;
    int randomPowerup1;
    int randomPowerup2;
    int randomPowerup3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthManager = GameObject.Find("HealthCanvas");
        foreach (GameObject powerup in allPowerups)
        {
            if( !powerup.GetComponent<Powerup>().powerupSO.classSpecific )
            {
                possiblePowerups.Add(Instantiate(powerup, transform.position, transform.rotation));
            }else if(powerup.GetComponent<Powerup>().powerupSO.usableClasses.Contains(GameManager.Instance.currentClassSo))
            {
                possiblePowerups.Add(Instantiate(powerup, transform.position, transform.rotation));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PowerUp()
    {
        Time. timeScale = 0;
        Debug.Log("Powerup enabled!");
        healthManager.SetActive(false);
        foreach (Transform child in transform) {
            child.gameObject.SetActive(true);
            foreach (Transform childschild in child.transform){
                child.gameObject.SetActive(true);
            }
        }
        option1 = GameObject.Find("Option1");
        option2 = GameObject.Find("Option2");
        option3 = GameObject.Find("Option3");
        randomPowerup1 = Random.Range(0, possiblePowerups.Count);
        option1.GetComponent<Image>().sprite = possiblePowerups[randomPowerup1].GetComponent<Powerup>().sprite;
        option1.GetComponentInChildren<TextMeshProUGUI>().text = possiblePowerups[randomPowerup1].GetComponent<Powerup>().powerupName + " : " + possiblePowerups[randomPowerup1].GetComponent<Powerup>().description;
        randomPowerup2 = Random.Range(0, possiblePowerups.Count);
        option2.GetComponent<Image>().sprite = possiblePowerups[randomPowerup2].GetComponent<Powerup>().sprite;
        option2.GetComponentInChildren<TextMeshProUGUI>().text = possiblePowerups[randomPowerup2].GetComponent<Powerup>().powerupName + " : " + possiblePowerups[randomPowerup2].GetComponent<Powerup>().description;
        randomPowerup3 = Random.Range(0, possiblePowerups.Count);
        option3.GetComponent<Image>().sprite = possiblePowerups[randomPowerup3].GetComponent<Powerup>().sprite;
        option3.GetComponentInChildren<TextMeshProUGUI>().text = possiblePowerups[randomPowerup3].GetComponent<Powerup>().powerupName + " : " + possiblePowerups[randomPowerup3].GetComponent<Powerup>().description;
    }

    public void PowerupSelected(int powerup)
    {
        if(powerup == 1){
            possiblePowerups[randomPowerup1].GetComponent<Powerup>().PowerupSelected();
        }else if(powerup ==2){
            possiblePowerups[randomPowerup2].GetComponent<Powerup>().PowerupSelected();
        }else if(powerup == 3){
            possiblePowerups[randomPowerup3].GetComponent<Powerup>().PowerupSelected();
        }
        foreach (Transform child in transform) {
            child.gameObject.SetActive(false);
            foreach (Transform childschild in child.transform){
                child.gameObject.SetActive(false);
            }
        }
        healthManager.SetActive(true);
        Time. timeScale = 1;
    }
}
