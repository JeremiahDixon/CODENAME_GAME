using UnityEngine;

[CreateAssetMenu(fileName = "PowerupSO", menuName = "Scriptable Objects/PowerupSO")]
public class PowerupSO : ScriptableObject
{
    public string powerupName;
    public Sprite sprite;
    public bool classSpecific;
    public ClassSO[] usableClasses; 

    public void CreateInfo(GameObject gameObject)
    {
        gameObject.GetComponent<Powerup>().powerupName = powerupName;
        gameObject.GetComponent<Powerup>().sprite = sprite;
        gameObject.GetComponent<Powerup>().classSpecific = classSpecific;
        gameObject.GetComponent<Powerup>().usableClasses = usableClasses;
    }

}
