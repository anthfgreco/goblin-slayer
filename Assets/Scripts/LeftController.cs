using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LeftController : MonoBehaviour
{
    private SteamVR_TrackedObject trackedObj;
    public LaserScript myLaserScript;
    public bool canTeleport;
    public GameObject CameraRig;
    public GameObject Camera;
    private SteamVR_Controller.Device Controller {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }
    
    [Header("Debugging!!!")]
    public bool pressedDown;
    
    void Awake() {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    void Start() {
        foreach (string i in UnityEngine.Input.GetJoystickNames()) {
            print(i);
        }
        pressedDown = false;
        canTeleport = false;
    }

	// Update is called once per frame
	void Update () {
        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip)) {
            Debug.Log(gameObject.name + "Grip Press Down");
        }
        if (Controller.GetPress(SteamVR_Controller.ButtonMask.Grip)) {
            Debug.Log(gameObject.name + "Grip Hold");
        }
        if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip)) {
            Debug.Log(gameObject.name + "Grip Release");
        }
        if (Controller.GetAxis() != Vector2.zero) {
            Debug.Log(gameObject.name + Controller.GetAxis());
        }
        if (Controller.GetHairTriggerDown()) {
            Debug.Log(gameObject.name + "Trigger Down");
            pressedDown = true;
        }
        if (Controller.GetHairTrigger()) {
            Debug.Log(gameObject.name + "Trigger Hold");
        }
        if (Controller.GetHairTriggerUp()) {
            Debug.Log(gameObject.name + "Trigger Release");
            pressedDown = false;
        }

        if (pressedDown) {
            myLaserScript.laserActive = true;
        }
        else {
            myLaserScript.laserActive = false;
            if (canTeleport) {
                //set start color
                SteamVR_Fade.View(Color.black, 0f);
            
                Vector3 difference = CameraRig.transform.position - Camera.transform.position;
                CameraRig.transform.position = new Vector3(myLaserScript.laserPointPos.x + difference.x, 0.0f, myLaserScript.laserPointPos.z + difference.z);
                
                canTeleport = false;

                //set and start fade to
                SteamVR_Fade.View(Color.clear, 3.0f);
            }
        }
    }
}