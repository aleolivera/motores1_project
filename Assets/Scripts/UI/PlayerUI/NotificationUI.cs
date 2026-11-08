using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class NotificationUI : MonoBehaviour {
    [Header("Text refs")]
    [SerializeField] private TextMeshProUGUI _txtNotification;
    public static string Text { get; private set; }
    public static bool ChangeText { get; private set; }
    public static Color TextColor { get; private set; }


    private void Awake () {
        Text = "";
        TextColor = Color.white;
        ChangeText = false;
    }

    private void Start () {
        if (_txtNotification == null) {
            Debug.LogError("NotificationUI: TextMesh ref not found");
        }
    }

    private void LateUpdate () {
        if (!ChangeText) return;
        
        _txtNotification.text = Text;
        ChangeText = false;
        _txtNotification.color = TextColor;
    }

    public static void ShowNotification (string message, Color color) {
        Text = message;
        TextColor = color;
        ChangeText = true;
    }
}
