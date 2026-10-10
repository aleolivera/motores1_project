using UnityEngine;

public class BillBoard : MonoBehaviour {
    private Transform cam;

    void Start () {
        cam = Camera.main.transform;
    }

    void LateUpdate() {
        transform.LookAt(transform.position + cam.forward);
    }
}
