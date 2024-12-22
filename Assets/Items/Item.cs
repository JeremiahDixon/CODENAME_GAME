using System.Net.Mime;
using UnityEngine;

public class Item : MonoBehaviour, IItem
{
    [SerializeField]
    private string itemName;
    [SerializeField]
    private int quantity;
    [SerializeField]
    private Sprite sprite;
    [TextArea]
    [SerializeField]
    private string itemDescription;

    private InventoryManager inventoryManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player"){
            int leftOverItems = inventoryManager.AddItem(itemName, quantity, sprite, itemDescription);
            if(leftOverItems <= 0){
                Destroy(gameObject);
            }else{
                quantity = leftOverItems;
            }
        }
    }

    public int getDamage()
    {
        throw new System.NotImplementedException();
    }

    public void setDamage(int damage)
    {
        throw new System.NotImplementedException();
    }

    public string getItemName()
    {
        return this.itemName;
    }

    public void setItemName(string itemName)
    {
        this.itemName = itemName;
    }

    public int getQuantity()
    {
        return this.quantity;
    }

    public void setQuantity(int quantity)
    {
        this.quantity = quantity;
    }

    public Sprite getSprite()
    {
        return this.sprite;
    }

    public void setSprite(Sprite sprite)
    {
        this.sprite = sprite;
    }

    public void setItemDescription(string itemDescription)
    {
        this.itemDescription = itemDescription;
    }

    public string getItemDescription()
    {
        return this.itemDescription;
    }
    
}
