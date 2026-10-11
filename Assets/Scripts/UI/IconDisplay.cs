using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class IconDisplay : MonoBehaviour {
    [Header("Icon display settings")]
    [SerializeField]List<Sprite> _sprites = new List<Sprite>();
    [SerializeField] Image _image; 
    
    void Start () {
        _image = GetComponent<Image>();

        if(_image == null) {
            Debug.LogWarning("IconDisplay: Image component not found.");
        }
    }

    public void ChangeIcon (int index, Color color) {
        if (index < 0 || _sprites.Count < 1 || index >= _sprites.Count) return;

        _image.sprite = _sprites[index];
        _image.color = color;
    }
}
