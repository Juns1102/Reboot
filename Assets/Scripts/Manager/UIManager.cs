using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {
    #region Singleton
    private static UIManager instance;

    public static UIManager Instance{
        get{
            if(null == instance){
                return null;
            }
            return instance;
        }
    }

    private void Awake() {
        if(instance == null){
            instance = this;
            if(transform.parent != null && transform.root != null){
                DontDestroyOnLoad(this.transform.root.gameObject);
            }
            else{
                DontDestroyOnLoad(this.gameObject);
            }
        }
        else{
            Destroy(this.gameObject);
        }
    }
    #endregion

    [SerializeField]
    float fadeTime;

    private bool activeInventory;
    public bool activeMap;
    private Slot[] slots;
    public int selectSlot;
    public GameObject inventoryUI;
    public GameObject informationUI;
    public GameObject inventorySlots;
    public GameObject selectPlanetPanel;
    public GameObject fade;
    public TextMeshProUGUI HUDcapacity;
    public TextMeshProUGUI value;
    public TextMeshProUGUI capacity;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemValue;
    public TextMeshProUGUI itemWeight;
    public TextMeshProUGUI planetName;
    public TextMeshProUGUI maxValue;
    public TextMeshProUGUI moveCount;
    public Image itemImage;
    public Image[] hearts;
    public Image skill1Cool;
    public Image skill2Cool;
    public Image skill2Panel;
    public Button map1Btn;
    public Button map2Btn;
    public Button map3Btn;

    private void Start(){
        selectSlot = -1;
        inventoryUI.GetComponent<CanvasGroup>().alpha = 0;
        informationUI.GetComponent<CanvasGroup>().alpha = 0;
        slots = inventorySlots.GetComponentsInChildren<Slot>();
        activeInventory = false;
        activeMap = false;
        selectPlanetPanel.SetActive(activeMap);
        inventoryUI.SetActive(activeInventory);
        informationUI.SetActive(activeInventory);
    }

    public void SetMapBtn(){
        if(GameManager.Instance.map1Value!=0){
            map1Btn.interactable = false;
            map1Btn.GetComponentInChildren<TextMeshProUGUI>().text = "Value: " + GameManager.Instance.map1Value;
        }
        if(GameManager.Instance.map2Value!=0){
            map2Btn.interactable = false;
            map2Btn.GetComponentInChildren<TextMeshProUGUI>().text = "Value: " + GameManager.Instance.map2Value.ToString();
        }
        if(GameManager.Instance.map3Value!=0){
            map3Btn.interactable = false;
            map3Btn.GetComponentInChildren<TextMeshProUGUI>().text = "Value: " + GameManager.Instance.map3Value.ToString();
        }
    }

    public void SetMoveCount(){
        moveCount.text = GameManager.Instance.moveCount.ToString();
    }

    public void FadeIn(){
        fade.GetComponent<CanvasGroup>().DOFade(0, 1f).SetEase(Ease.Linear).OnComplete(() => 
        {fade.SetActive(false); GameManager.Instance.SetMaxValueInfo();});
    }

    public void FadeOut(){
        fade.SetActive(true);
        fade.GetComponent<CanvasGroup>().DOFade(1, 1f).SetEase(Ease.Linear).OnComplete(() => 
        {if(activeInventory)Inventory(); GameManager.Instance.moveCount = GameManager.Instance.maxMoveCount;
        SetMoveCount(); GameManager.Instance.fieldItems.Clear(); 
        selectPlanetPanel.SetActive(false); activeMap = false; 
        GameManager.Instance.teleport(); FadeIn();});
    }

    public void EquipTp(){
        skill2Panel.color = new Color(255/255, 255/255, (float)206/255, 255/255);
    }
    public void UseTp(){
        skill2Panel.color = new Color(255/255, 255/255, 255/255, (float)100/255);
    }

    public void SetCoolTime(){
        skill1Cool.fillAmount = GameManager.Instance.skill1CoolTime / GameManager.Instance.skill1MaxCoolTime;
        skill2Cool.fillAmount = GameManager.Instance.skill2CoolTime / GameManager.Instance.skill2MaxCoolTime;
    }

    public void Map(){
        if(activeMap){
            selectPlanetPanel.GetComponent<CanvasGroup>().DOFade(0, fadeTime).SetEase(Ease.Linear).OnComplete(() => SetMap());
        }
        else{
            SetMap();
            selectPlanetPanel.GetComponent<CanvasGroup>().DOFade(1, fadeTime).SetEase(Ease.Linear);
        }
    }

    public void SetMap(){
        activeMap = !activeMap;
        selectPlanetPanel.SetActive(activeMap);
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
        informationUI.SetActive(activeInventory);
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
