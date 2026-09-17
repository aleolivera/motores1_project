using UnityEngine;

public class BillBoard : MonoBehaviour {
    [SerializeField] private Transform cam;
    
    void LateUpdate() {
        transform.LookAt(transform.position + cam.forward);
    }
}
