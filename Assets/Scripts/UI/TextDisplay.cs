using System;
using TMPro;
using UnityEngine;

public class TextDisplay : MonoBehaviour {
    [Header("Text Display settings")]
    [SerializeField] private string _titleText;
    [SerializeField] private string _bodyText;
    
    [Header("References")]
    [SerializeField] private TextMeshProUGUI _tmpTitle;
    [SerializeField] private TextMeshProUGUI _tmpBody;

    public string TitleText { 
        get { return _titleText; } 
        set { 
            _titleText = value;
            _tmpTitle.text = _titleText;
        } 
    }

    public string BodyText { 
        get { return _bodyText; } 
        set { 
            _bodyText = value; 
            _tmpBody.text = _bodyText;
        } 
    }

    private void Start () {
        if (_tmpTitle == null)
            Debug.LogWarning("TextDisplay: TMP title not found.");
        if (_tmpBody == null)
            Debug.LogWarning("TextDisplay: TMP body not found.");

        _tmpTitle.text = _titleText;
        _tmpBody.text = _bodyText;
    }

    public void EnableTextDisplay(bool enable) {
        gameObject.SetActive(enable);
    }

    public void SetBlankText () {
        TitleText   = "";
        BodyText    = "";
    }
}
