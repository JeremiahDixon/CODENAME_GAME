using System.Runtime.InteropServices.ComTypes;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class Powerup : MonoBehaviour
{

    public string powerupName;
    public int powerupLevel = 0;
    public int maxPowerupLevel;
    public Sprite sprite;
    public PowerupSO powerupSO;
    public bool classSpecific;
    public ClassSO[] usableClasses; 
    public string description;
    public void IncreaseLevel()
    {
        powerupLevel += 1;
    }

    public virtual void PowerupSelected()
    {

    }

}
