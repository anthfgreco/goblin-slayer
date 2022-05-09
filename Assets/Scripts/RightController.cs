using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RightController : MonoBehaviour
{
    private SteamVR_TrackedObject trackedObj;
    private SteamVR_Controller.Device Controller {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }

    [SerializeField] private Text txt;
    [SerializeField] private int bomberRequiredKills;
    public int goblinsKilled = 0;

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

        UpdateText();
    }

    private void UpdateText() {
        string s = "";
        s += "Goblins slain: " + goblinsKilled + "\n";
        s += "\n";
        s += "Slay " + bomberRequiredKills + " and shoot the target for a bomber plane!";

        txt.text = s;
    }
}
