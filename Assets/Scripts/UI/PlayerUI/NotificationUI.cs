using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NotificationUI : MonoBehaviour {
    [Header("Notification Settings")]
    [SerializeField] private TextMeshProUGUI _tmpNotification;
    [SerializeField] private float _textChangeCooldown = 10f;
    [SerializeField] private List<string> _hintTextList = new List<string>();
    
    [Header("Debug")]
    [SerializeField] private float _counter = 0f;
    [SerializeField] private int _hintTextIndex = 0;

    public static string Text { get; private set; }
    public static bool ChangeText { get; private set; }
    public static Color TextColor { get; private set; }


    private void Awake () {
        Text = "";
        TextColor = Color.white;
        ChangeText = false;
    }

    private void Start () {
        if (_tmpNotification == null) {
            Debug.LogError("NotificationUI: TextMesh ref not found");
        }
        ShowHintListText();
    }

    private void Update () {
        if(_counter < _textChangeCooldown) {
            _counter += Time.deltaTime;
        } else {
            ShowHintListText();
        }
    }


    private void LateUpdate () {
        if (!ChangeText) return;

        _tmpNotification.text = Text;
        _tmpNotification.color = TextColor;
        
        ChangeText = false;
        _counter = 0f;
    }

    public static void ShowNotification (string message, Color color) {
        Text = message;
        TextColor = color;
        ChangeText = true;
    }

    public void ShowHintListText() {
        ShowNotification(_hintTextList[_hintTextIndex++], Color.white);

        if(_hintTextIndex >= _hintTextList.Count)   _hintTextIndex = 0;
    }


}
