using UnityEngine;

public class keySys : MonoBehaviour
{
    public bool grabbedKey = false;
    public GameObject key1;
    public GameObject key2;
    public GameObject key3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("b");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {

         if (other.CompareTag("Player"))
        {
            grabbedKey = true;
            
            Destroy(key1);
            Destroy(key2);
            Destroy(key3);
        }



}
}