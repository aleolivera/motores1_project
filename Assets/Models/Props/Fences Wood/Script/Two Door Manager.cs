using System.Collections;
using UnityEngine;

public class TwoDoorManager : DoorManager {
    [Header("Control To Open & Close Door Based On The Speed Given")]
    [SerializeField] public bool OpenDoor;
    [SerializeField] public bool CloseDoor;
    [Range(0, 100)] public float DoorSpeed;

    [Header("Objects Used In Code")]
    [SerializeField] GameObject LeftDoor;
    [SerializeField] GameObject RightDoor;

    // Update is called once per frame
    void Update () {
        if (OpenDoor) {
            StartCoroutine(OnlyOpenLeftDoor());
            StartCoroutine(OnlyOpenRightDoor());
            OpenDoor = false;
        } else if (CloseDoor) {
            StartCoroutine(OnlyCloseLeftDoor());
            StartCoroutine(OnlyCloseRightDoor());
            CloseDoor = false;
        }
    }


    float LeftDoorRotationValue = 0;
    float RightDoorRotationValue = 0;

    IEnumerator OnlyOpenLeftDoor () {
        while (LeftDoorRotationValue < 90) {
            LeftDoorRotationValue = LeftDoorRotationValue + ( DoorSpeed * Time.deltaTime );
            LeftDoor.transform.localEulerAngles = new Vector3(0, LeftDoorRotationValue, 0);
            yield return null;
        }
        LeftDoorRotationValue = 90;
        LeftDoor.transform.localEulerAngles = new Vector3(0, 90, 0);
    }


    IEnumerator OnlyOpenRightDoor () {
        while (RightDoorRotationValue > -90) {
            RightDoorRotationValue = RightDoorRotationValue - ( DoorSpeed * Time.deltaTime );
            RightDoor.transform.localEulerAngles = new Vector3(0, RightDoorRotationValue, 0);
            yield return null;
        }
        RightDoorRotationValue = -90;
        RightDoor.transform.localEulerAngles = new Vector3(0, -90, 0);
    }

    IEnumerator OnlyCloseLeftDoor () {
        while (LeftDoorRotationValue > 0) {
            LeftDoorRotationValue = LeftDoorRotationValue - ( DoorSpeed * Time.deltaTime );
            LeftDoor.transform.localEulerAngles = new Vector3(0, LeftDoorRotationValue, 0);
            yield return null;
        }
        LeftDoorRotationValue = 0;
        LeftDoor.transform.localEulerAngles = new Vector3(0, 0, 0);
    }

    IEnumerator OnlyCloseRightDoor () {
        while (RightDoorRotationValue < 0) {
            RightDoorRotationValue = RightDoorRotationValue + ( DoorSpeed * Time.deltaTime );
            RightDoor.transform.localEulerAngles = new Vector3(0, RightDoorRotationValue, 0);
            yield return null;
        }
        RightDoorRotationValue = 0;
        RightDoor.transform.localEulerAngles = new Vector3(0, 0, 0);
    }

    public override void Open () {
        StartCoroutine(OnlyOpenLeftDoor());
        StartCoroutine(OnlyOpenRightDoor());
        OpenDoor = false;
    }
}
