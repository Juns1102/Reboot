using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField]
    private int slotNum;

    public Image imageIcon;

    public void UpdateSlot(){
        if(slotNum < InventoryManager.Instance.items.Count){
            imageIcon.sprite = InventoryManager.Instance.items[slotNum].sprite;
            imageIcon.gameObject.SetActive(true);
            gameObject.GetComponent<Button>().interactable = true;
        }
        else{
            imageIcon.gameObject.SetActive(false);
            gameObject.GetComponent<Button>().interactable = false;
        }
    }

    public void ClickBtn(){
        UIManager.Instance.itemImage.sprite = InventoryManager.Instance.items[slotNum].sprite;
        UIManager.Instance.itemName.text = "Name: " + InventoryManager.Instance.items[slotNum].name;
        UIManager.Instance.value.text = "Value: " + InventoryManager.Instance.items[slotNum].value;
        UIManager.Instance.weight.text = "Weight: " + InventoryManager.Instance.items[slotNum].weight;
    }
}
