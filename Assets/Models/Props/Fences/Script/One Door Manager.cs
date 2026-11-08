using System.Collections;
using UnityEngine;

public class OneDoorManager : DoorManager {
    [Header("Control To Open & Close Door Based On The Speed Given")]
    [SerializeField] public bool OpenDoor;
    [SerializeField] public bool CloseDoor;
    [Range(0, 100)] public float DoorSpeed;

    [Header("Objects Used In Code")]
    [SerializeField] GameObject Door;

    float DoorRotationValue = 0;
    // Update is called once per frame
    void Update () {
        /*
        if (OpenDoor) {
            StartCoroutine(OnlyOpenDoor());
        } else if (CloseDoor) {
            StartCoroutine(OnlyCloseDoor());
        }
        */
    }

    IEnumerator OnlyOpenDoor () {
        while (DoorRotationValue < 90) {
            DoorRotationValue = DoorRotationValue + ( DoorSpeed * Time.deltaTime );
            Door.transform.localEulerAngles = new Vector3(0, DoorRotationValue, 0);
            yield return null;
        }
        DoorRotationValue = 90;
        Door.transform.localEulerAngles = new Vector3(0, 90, 0);
        OpenDoor = false;
    }

    IEnumerator OnlyCloseDoor () {
        while (DoorRotationValue > 0) {
            DoorRotationValue = DoorRotationValue - ( DoorSpeed * Time.deltaTime );
            Door.transform.localEulerAngles = new Vector3(0, DoorRotationValue, 0);
            yield return null;
        }
        DoorRotationValue = 0;
        Door.transform.localEulerAngles = new Vector3(0, 0, 0);
        CloseDoor = false;
    }

    public override void Open () => StartCoroutine(OnlyOpenDoor());
}
