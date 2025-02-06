using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PowerupMenu : MonoBehaviour
{
    GameObject healthManager;
    GameObject option1;
    GameObject option2;
    GameObject option3;
    public GameObject[] allPowerups;
    public List<GameObject> possiblePowerups;
    List<GameObject> randomPowerups;
    [SerializeField] GameObject _powerupMenuFirst;
    [SerializeField] private PlaySystemManager playSystemManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (playSystemManager != null)
        {
            playSystemManager.OnLevelUp += PowerUp;
        }
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

    private void OnDestroy()
    {
        if (playSystemManager != null)
        {
            playSystemManager.OnLevelUp -= PowerUp;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PowerUp(int level)
    {
        GameManager.Instance.PoweringUp();
        Time.timeScale = 0;
        Debug.Log("Powerup enabled!");
        healthManager.SetActive(false);

        foreach (Transform child in transform) {
            child.gameObject.SetActive(true);
            foreach (Transform childschild in child.transform){
                childschild.gameObject.SetActive(true);
            }
        }
        
        option1 = GameObject.Find("Option1");
        option2 = GameObject.Find("Option2");
        option3 = GameObject.Find("Option3");

        if(randomPowerups != null)
        {
            randomPowerups.Clear();
        }
        randomPowerups = GetRandomPowerups(3);

        if(randomPowerups.Count >= 1)
        {
            option1.GetComponent<Image>().sprite = randomPowerups[0].GetComponent<Powerup>().sprite;
            option1.GetComponentInChildren<TextMeshProUGUI>().text = randomPowerups[0].GetComponent<Powerup>().powerupName + " : " + randomPowerups[0].GetComponent<Powerup>().description + 
                " : " + randomPowerups[0].GetComponent<Powerup>().powerupLevel + " out of " + randomPowerups[0].GetComponent<Powerup>().maxPowerupLevel;
        }

        if(randomPowerups.Count >= 2)
        {
            option2.GetComponent<Image>().sprite = randomPowerups[1].GetComponent<Powerup>().sprite;
            option2.GetComponentInChildren<TextMeshProUGUI>().text = randomPowerups[1].GetComponent<Powerup>().powerupName + " : " + randomPowerups[1].GetComponent<Powerup>().description + 
                " : " + randomPowerups[1].GetComponent<Powerup>().powerupLevel + " out of " + randomPowerups[1].GetComponent<Powerup>().maxPowerupLevel;

        }else if(option2.activeInHierarchy)
        {
            option2.SetActive(false);
        }

        if(randomPowerups.Count >= 3)
        {
            option3.GetComponent<Image>().sprite = randomPowerups[2].GetComponent<Powerup>().sprite;
            option3.GetComponentInChildren<TextMeshProUGUI>().text = randomPowerups[2].GetComponent<Powerup>().powerupName + " : " + randomPowerups[2].GetComponent<Powerup>().description + 
                " : " + randomPowerups[2].GetComponent<Powerup>().powerupLevel + " out of " + randomPowerups[2].GetComponent<Powerup>().maxPowerupLevel;
        }else if(option3.activeInHierarchy)
        {
            option3.SetActive(false);
        }

        if(Gamepad.current != null)
        {
            EventSystem.current.SetSelectedGameObject(_powerupMenuFirst);
        }
    }

    List<GameObject> GetRandomPowerups(int count)
    {
        // Ensure we don't request more items than available
        if (possiblePowerups.Count < count)
        {
            Debug.LogWarning("Requested more items than available in the list.");
            count = possiblePowerups.Count;
        }

        // Create a temporary list to store the indices
        List<int> indices = new List<int>();
        while (indices.Count < count)
        {
            int randomIndex = Random.Range(0, possiblePowerups.Count);
            if (!indices.Contains(randomIndex))
            {
                indices.Add(randomIndex);
            }
        }

        // Use the selected indices to pick the GameObjects
        List<GameObject> selectedItems = new List<GameObject>();
        foreach (int index in indices)
        {
            selectedItems.Add(possiblePowerups[index]);
        }

        return selectedItems;
    }

    public void PowerupSelected(int powerup)
    {
        if(powerup == 1){
            randomPowerups[0].GetComponent<Powerup>().PowerupSelected();
            if(randomPowerups[0].GetComponent<Powerup>().powerupLevel >= randomPowerups[0].GetComponent<Powerup>().maxPowerupLevel)
            {
                possiblePowerups.Remove(randomPowerups[0]);
            }
        }else if(powerup ==2){
            randomPowerups[1].GetComponent<Powerup>().PowerupSelected();
            if(randomPowerups[1].GetComponent<Powerup>().powerupLevel >= randomPowerups[1].GetComponent<Powerup>().maxPowerupLevel)
            {
                possiblePowerups.Remove(randomPowerups[1]);
            }
        }else if(powerup == 3){
            randomPowerups[2].GetComponent<Powerup>().PowerupSelected();
            if(randomPowerups[2].GetComponent<Powerup>().powerupLevel >= randomPowerups[2].GetComponent<Powerup>().maxPowerupLevel)
            {
                possiblePowerups.Remove(randomPowerups[2]);
            }
        }
        foreach (Transform child in transform) {
            child.gameObject.SetActive(false);
            foreach (Transform childschild in child.transform){
                childschild.gameObject.SetActive(false);
            }
        }
        healthManager.SetActive(true);
        Time.timeScale = 1;
        EventSystem.current.SetSelectedGameObject(null);
        GameManager.Instance.DonePoweringUp();
    }
}
