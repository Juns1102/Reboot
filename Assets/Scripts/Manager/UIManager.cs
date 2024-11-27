using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager> {
    [SerializeField]
    float fadeTime;
    private bool activeInventory;
    private Slot[] slots;
    public int selectSlot;
    public GameObject inventoryUI;
    public GameObject informationUI;
    public GameObject inventorySlots;
    public TextMeshProUGUI HUDcapacity;
    public TextMeshProUGUI value;
    public TextMeshProUGUI capacity;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemValue;
    public TextMeshProUGUI itemWeight;
    public TextMeshProUGUI planetName;
    public TextMeshProUGUI maxValue;
    public Image itemImage;
    public Image[] hearts;

    private void Start(){
        selectSlot = -1;
        inventoryUI.GetComponent<CanvasGroup>().alpha = 0;
        informationUI.GetComponent<CanvasGroup>().alpha = 0;
        slots = inventorySlots.GetComponentsInChildren<Slot>();
        activeInventory = false;
        inventoryUI.SetActive(activeInventory);
    }
    
    public void Inventory(){
        if(activeInventory){
            inventoryUI.GetComponent<CanvasGroup>().DOFade(0, fadeTime).SetEase(Ease.Linear).OnComplete(() => SetInventory());
            informationUI.GetComponent<CanvasGroup>().DOFade(0, fadeTime).SetEase(Ease.Linear);
        }
        else{
            ItemPlace();
            SetInventory();
            SetPlanetInfo();
            inventoryUI.GetComponent<CanvasGroup>().DOFade(1, fadeTime).SetEase(Ease.Linear);
            informationUI.GetComponent<CanvasGroup>().DOFade(1, fadeTime).SetEase(Ease.Linear);
        }
    }

    private void SetPlanetInfo(){
        planetName.text = "Planet: " + GameManager.Instance.planetName;
        maxValue.text = "MaxValue: " + GameManager.Instance.maxValue;
    }

    private void SetInventory(){
        activeInventory = !activeInventory;
        SetHeadInfo();
        inventoryUI.SetActive(activeInventory);
    }

    public void ItemPlace(){
        for(int i=0; i<slots.Length; i++){
                slots[i].UpdateSlot();
        }
    }

    public void SetHeadInfo() {
        InventoryManager.Instance.GetHeadInfo(value, capacity);
        InventoryManager.Instance.GetHeadInfo(value, HUDcapacity);
    }

    public void DropItem() {
        InventoryManager.Instance.RemoveItem(selectSlot);
        SetHeadInfo();
        ItemPlace();
        BottumInfoReset();
    }

    public void BottumInfoReset() {
        itemImage.gameObject.SetActive(false);
        itemName.text = "Name: ";
        itemValue.text = "Value: ";
        itemWeight.text = "Weight: ";
        selectSlot = -1;
    }

    public void HeartsSet(){
        hearts[0].gameObject.SetActive(GameManager.Instance.playerHearts >= 1);
        hearts[1].gameObject.SetActive(GameManager.Instance.playerHearts >= 2);
        hearts[2].gameObject.SetActive(GameManager.Instance.playerHearts >= 3);
    }
}
