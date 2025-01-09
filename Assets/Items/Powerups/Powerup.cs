using System.Runtime.InteropServices.ComTypes;
using Unity.VisualScripting;
using UnityEngine;

public class Powerup : MonoBehaviour
{

    public string powerupName;
    int powerupLevel = 0;
    public Sprite sprite;
    public PowerupSO powerupSO;
    public bool classSpecific;
    public ClassSO[] usableClasses; 

    public void IncreaseLevel()
    {
        powerupLevel += 1;
    }

}
