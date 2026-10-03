using UnityEngine;

public class cage : MonoBehaviour
{
    public GameObject key;
    public GameObject bar1;
    public GameObject bar2;
    public GameObject bar3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        


    }
     void OnTriggerEnter(Collider other)
    {
        keySys keyCheck = key.GetComponent<keySys>();

         if (other.CompareTag("Player"))
        {
            if(keyCheck.grabbedKey){
            
            Destroy(bar1);
            Destroy(bar2);
            Destroy(bar3);
            }
        }



}
}
