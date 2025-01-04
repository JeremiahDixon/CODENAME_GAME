using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using ca.HenrySoftware.Rage;
using Unity.VisualScripting;
using System.Numerics;
public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    //item data
    private string itemName;
    private int quantity;
    private Sprite itemSprite;
    private bool isFull;
    private String itemDescription;
    [SerializeField]
    private int maxNumber;

    //item slot
    [SerializeField]
    private TMP_Text quantityText;
    [SerializeField]
    private Image itemImage;

    //item description slot
    public Image itemDescriptionImage;
    public TMP_Text itemDescriptionNameText;
    public TMP_Text itemDescriptionText;

    //other
    public GameObject selectedShader;
    public bool thisItemSelected;
    private InventoryManager inventoryManager;

    void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int addItem(string itemName, int quantity, Sprite itemSprite, string itemDescription){
        if(isFull){
            return quantity;
        }

        this.itemName = itemName;
        this.itemSprite = itemSprite;
        this.itemDescription = itemDescription;
        itemImage.sprite = itemSprite;

        this.quantity += quantity;
        if(this.quantity >= maxNumber){
            quantityText.text = maxNumber.ToString();
            quantityText.enabled = true;
            isFull = true;
        
            int extraItems = this.quantity - maxNumber;
            this.quantity = maxNumber;
            return extraItems;
        }

        quantityText.text = this.quantity.ToString();
        quantityText.enabled = true;

        return 0;
        
    }

    public bool getIsFull(){
        return this.isFull;
    }

    public int getQuantity(){
        return this.quantity;
    }

    public string getItemName(){
        return this.itemName;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left){
            OnLeftClick();
        }else if(eventData.button == PointerEventData.InputButton.Right){
            OnRightClick();
        }
    }

    void OnLeftClick(){
        if(thisItemSelected){
            bool used = inventoryManager.UseItem(itemName);
            if(used){
                this.quantity -= 1;
                quantityText.text = this.quantity.ToString();
                if(this.quantity <= 0){
                    EmptySlot();
                }
            }
        }else{
            inventoryManager.DeselectAllSlots();
            selectedShader.SetActive(true);
            thisItemSelected = true;
            itemDescriptionNameText.text = itemName;
            itemDescriptionText.text = itemDescription;
            itemDescriptionImage.sprite = itemSprite;
            if(itemDescriptionImage.sprite != null){
                itemDescriptionImage.enabled = true;
            }else{
                itemDescriptionImage.enabled = false;
            }
        }

    }

    private void EmptySlot()
    {
        quantityText.enabled = false;
        itemImage.sprite = null;
        
        itemDescriptionNameText.text = "";
        itemDescriptionText.text = "";
        itemDescriptionImage.enabled = false;
    }

    void OnRightClick(){
        GameObject itemToDrop = new GameObject(itemName);
        Item droppedItem = itemToDrop.AddComponent<Item>();
        droppedItem.setItemName(itemName);
        droppedItem.setQuantity(1);
        droppedItem.setSprite(itemSprite);
        droppedItem.setItemDescription(itemDescription);

        SpriteRenderer sr = itemToDrop.AddComponent<SpriteRenderer>();
        sr.sprite = itemSprite;
        sr.sortingOrder = 1;
        sr.sortingLayerName = "PickupItem";

        itemToDrop.AddComponent<PolygonCollider2D>();

        itemToDrop.transform.position = GameObject.FindWithTag("Player").transform.position + new UnityEngine.Vector3(1, 0, 0);
        itemToDrop.transform.localScale = new UnityEngine.Vector3(0.5f, 0.5f, 0.5f);
        this.quantity -= 1;
        quantityText.text = this.quantity.ToString();
        if(this.quantity <= 0){
            EmptySlot();
        }
    }
}
