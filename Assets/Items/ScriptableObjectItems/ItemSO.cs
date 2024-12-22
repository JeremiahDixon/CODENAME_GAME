using NUnit.Framework.Constraints;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "Scriptable Objects/ItemSO")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public StatToChange statToChange = new StatToChange();
    public int amountToChangeStat;

    public enum StatToChange{
        none,
        health,
        attack,
        stamina
    };

    public void UseItem(){
        switch(statToChange){
            case StatToChange.none:
                break;
            case StatToChange.health:
                GameObject.Find("Player").GetComponent<Player>().Heal(amountToChangeStat);
                break;
            case StatToChange.attack:
                break;
            case StatToChange.stamina:
                break;
            default:
                break;
        }
    }

}
