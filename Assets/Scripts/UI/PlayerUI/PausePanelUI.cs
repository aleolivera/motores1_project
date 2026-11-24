using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;
public class PausePanelUI : MonoBehaviour {
    [SerializeField] private GameObject _panel;
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _tmpItemName;
    [SerializeField] private TextMeshProUGUI _tmpItemDescription;

    [SerializeField] private Button _btnNext;
    [SerializeField] private Button _btnPrev;

    private List<Item> _items = new List<Item>();
    public int ItemIndex { get; private set; }

    private void Awake () => ItemIndex = 0;
    private void OnEnable () => GameManager.OnGamePaused += OpenPausePanel;
    private void OnDisable () => GameManager.OnGamePaused -= OpenPausePanel;
    private void Start () {
        if (_panel == null)                 Debug.LogError("PausePanelUI: Panel Not Found.");
        if (_itemImage == null)             Debug.LogError("PausePanelUI: Image Not Found.");
        if (_tmpItemName == null)           Debug.LogError("PausePanelUI: TMP Name Not Found.");
        if (_tmpItemDescription == null)    Debug.LogError("PausePanelUI: TMP description Not Found.");
        if (_btnNext == null)               Debug.LogError("PausePanelUI: Button Next Not Found.");
        if (_btnPrev == null)               Debug.LogError("PausePanelUI: Button Prev Not Found.");

        _panel.SetActive(false);
    }

    public void OpenPausePanel (PlayerInventory inventory) {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        _panel.SetActive(true);

        SetItemList(inventory.KeyItems,inventory.QuestItems);
        
        if(_items.Count < 1) { 
            HideItemDisplay();
            return;
        }
        
        DisplayItem(_items[0]);
        
        bool isActive = ( _items.Count > 1 ) ? true : false;
        _btnNext.gameObject.SetActive(isActive);
        _btnPrev.gameObject.SetActive(isActive);
        
        _itemImage.gameObject.SetActive(true);
    }

    private void HideItemDisplay () {
        _tmpItemName.text           = "No Items.";
        _tmpItemDescription.text    = "Interact (E) with Keys and items to collect.";
        _btnNext.gameObject.SetActive(false);
        _btnPrev.gameObject.SetActive(false);
        _itemImage.gameObject.SetActive(false);
    }

    public void ClosePausePanel () {
        Cursor.lockState    = CursorLockMode.Locked;
        Cursor.visible      = false;

        _panel.SetActive(false);

        GameManager.Instance.UnpauseGame();
    }

    public void BackToMain () => GameManager.Instance.LoadMainMenu();
    public void NextItem () {
        if(++ItemIndex >= _items.Count) ItemIndex = 0;
        DisplayItem(_items[ItemIndex]);

    }
    public void PrevItem () {
        if(--ItemIndex < 0) ItemIndex = _items.Count - 1;
        DisplayItem(_items[ItemIndex]);
    }

    private void SetItemList (Dictionary<int,KeyItem> keys, Dictionary<int, QuestItem> questItems) {
        ItemIndex = 0;
        _items.Clear();
        foreach (var item in keys)          { _items.Add(item.Value); }
        foreach (var item in questItems)    { _items.Add(item.Value); }
    }

    private void SetItemImage (Sprite itemImage) => _itemImage.sprite = itemImage;
    private void SetItemNameText (string name) => _tmpItemName.text = name;
    private void SetItemDescriptionText (string description) => _tmpItemDescription.text = description;
    
    private void DisplayItem (Item item) {
        SetItemImage (item.DisplayImage);
        SetItemDescriptionText (item.Description);
        SetItemNameText (item.Name);
    }
}
