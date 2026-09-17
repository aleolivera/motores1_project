using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthBar : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Gradient _gradient;
    [SerializeField] private Image _fill;
    private Slider _slider;

    void Start() {
        _slider = GetComponent<Slider>();
        if(_slider == null) {
            Debug.Log("HealthBar: Slider component not found.");
        } else {
            SliderConfig();
        }
    }

    public void SetMaxHealth() {
        _slider.maxValue = 1f;
        _slider.value = 1f;
        _fill.color = _gradient.Evaluate(1f);
    }

    public void SetHealth(int health, int maxHealth) {
        _slider.value = (float)health / maxHealth;
        _fill.color = _gradient.Evaluate(_slider.value);
    }

    private void SliderConfig() {
        _slider.interactable = false;
        _slider.transition = Selectable.Transition.None;

        Navigation cpy = _slider.navigation;
        cpy.mode = Navigation.Mode.None;
        _slider.navigation = cpy;

    }
}
