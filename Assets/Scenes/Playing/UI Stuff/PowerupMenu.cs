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
            if( !powerup.GetComponent<Powerup>().powerupSO.classSpecific || powerup.GetComponent<Powerup>().powerupSO.usableClasses.Contains(GameManager.Instance.currentClassSo))
            {
                possiblePowerups.Add(powerup);
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
        option1.GetComponent<Image>().sprite = possiblePowerups[randomPowerup1].GetComponent<Powerup>().powerupSO.sprite;
        option1.GetComponentInChildren<TextMeshProUGUI>().text = possiblePowerups[randomPowerup1].GetComponent<Powerup>().powerupSO.powerupName + " : " + possiblePowerups[randomPowerup1].GetComponent<Powerup>().powerupSO.description;
        randomPowerup2 = Random.Range(0, possiblePowerups.Count);
        option2.GetComponent<Image>().sprite = possiblePowerups[randomPowerup2].GetComponent<Powerup>().powerupSO.sprite;
        option2.GetComponentInChildren<TextMeshProUGUI>().text = possiblePowerups[randomPowerup2].GetComponent<Powerup>().powerupSO.powerupName + " : " + possiblePowerups[randomPowerup1].GetComponent<Powerup>().powerupSO.description;
        randomPowerup3 = Random.Range(0, possiblePowerups.Count);
        option3.GetComponent<Image>().sprite = possiblePowerups[randomPowerup3].GetComponent<Powerup>().powerupSO.sprite;
        option3.GetComponentInChildren<TextMeshProUGUI>().text = possiblePowerups[randomPowerup3].GetComponent<Powerup>().powerupSO.powerupName + " : " + possiblePowerups[randomPowerup1].GetComponent<Powerup>().powerupSO.description;
    }

    public void PowerupSelected(int powerup)
    {
        if(powerup == 1){
            Instantiate(possiblePowerups[randomPowerup1], transform.position, transform.rotation);
        }else if(powerup ==2){
            Instantiate(possiblePowerups[randomPowerup2], transform.position, transform.rotation);
        }else if(powerup == 3){
            Instantiate(possiblePowerups[randomPowerup3], transform.position, transform.rotation);
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
