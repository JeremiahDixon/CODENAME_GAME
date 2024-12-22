using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    private GameObject InventoryMenu;
    private bool menuActivated;
    public ItemSlot[] itemSlot;

    public ItemSO[] itemSos;
    void Start() {
        
    }

    void Update() {
        if(Input.GetButtonDown("Inventory") && menuActivated){
            Time.timeScale = 1;
            InventoryMenu.SetActive(false);
            menuActivated = false;
        }
        else if(Input.GetButtonDown("Inventory") && !menuActivated){
            Time.timeScale = 0;
            InventoryMenu.SetActive(true);
            menuActivated = true;
        }

    }


    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription){
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if(!itemSlot[i].getIsFull() && itemSlot[i].getItemName() == itemName || itemSlot[i].getQuantity() == 0){
                int leftOverItems = itemSlot[i].addItem(itemName, quantity, itemSprite, itemDescription);
                if(leftOverItems > 0){
                    leftOverItems = AddItem(itemName, leftOverItems, itemSprite, itemDescription);
                }
                return leftOverItems;
            }
        }
        return quantity;
        //Debug.Log("itemName = " + itemName + " quantity = " + quantity +  " itemSprite = " + itemSprite);
    }

    public bool UseItem(string itemName){
        for (int i = 0; i < itemSos.Length; i++)
        {
            if(itemSos[i].itemName == itemName){
                itemSos[i].UseItem();
                return true;
            }
        }
        return false;
    }

    public void DeselectAllSlots(){
        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i].selectedShader.SetActive(false);
            itemSlot[i].thisItemSelected = false;
        }
    }
}
