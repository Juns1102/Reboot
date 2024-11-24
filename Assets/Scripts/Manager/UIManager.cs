using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager> {
    [SerializeField]
    float fadeTime;
    bool activeInventory;
    public GameObject inventoryUI;
    private Slot[] slots;
    public GameObject inventorySlots;
    public TextMeshProUGUI capacity;
    public TextMeshProUGUI itemName;

    public TextMeshProUGUI value;
    public TextMeshProUGUI weight;
    public Image itemImage;

    private void Start(){
        inventoryUI.GetComponent<CanvasGroup>().alpha = 0;
        slots = inventorySlots.GetComponentsInChildren<Slot>();
        activeInventory = false;
        inventoryUI.SetActive(activeInventory);
    }
    
    public void OnInventory(){
        if(activeInventory){
            inventoryUI.GetComponent<CanvasGroup>().DOFade(0, fadeTime).SetEase(Ease.Linear).OnComplete(() => SetInventory());
        }
        else{
            ItemPlace();
            SetInventory();
            inventoryUI.GetComponent<CanvasGroup>().DOFade(1, fadeTime).SetEase(Ease.Linear);
        }
        inventoryUI.SetActive(activeInventory);
    }

    private void SetInventory(){
        activeInventory = !activeInventory;
        inventoryUI.SetActive(activeInventory);
    }

    public void ItemPlace(){
        for(int i=0; i<slots.Length; i++){
                slots[i].UpdateSlot();
        }
    }
}
