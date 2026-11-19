using UnityEngine;

public class WifeAnimationEvents : MonoBehaviour{
    WifeController _controller;

    private void Start () {
        _controller = GetComponentInParent<WifeController>();
    }

    public void EnableMovement () => _controller.IsActive = true;
}
