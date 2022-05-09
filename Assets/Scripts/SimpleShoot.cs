using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimpleShoot : MonoBehaviour
{
    public RightController RightControllerScript;
    AudioSource audioData;
    [Header("Prefab References")]
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;

    [Header("Location References")]
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;

    [Header("Settings")]
    [Tooltip("Specify time to destroy object")] [SerializeField] private float destroyTimer;
    [Tooltip("Bullet Speed")] [SerializeField] private float shotPower;
    [Tooltip("Casing Ejection Speed")] [SerializeField] private float ejectPower;


    void Start()
    {
        if (barrelLocation == null)
            barrelLocation = transform;

        if (gunAnimator == null)
            gunAnimator = GetComponentInChildren<Animator>();

        audioData = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Input
        if (RightControllerScript.pressedDown) {
            // This trigger plays the animation, shoots the bullet, and creates the casing!!!
            // This took hours of debugging, I thought this was only an animation
            gunAnimator.SetTrigger("Fire");
            RightControllerScript.pressedDown = false;
        }
    }

    // Creates bullet behavior
    public void Shoot()
    {
        audioData.Play(0);
        
        // Create muzzle flash effect
        GameObject tempFlash;
        tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

        // Destroy the muzzle flash effect
        Destroy(tempFlash, destroyTimer);
    
        // Create a bullet and add force on it in direction of the barrel
        Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation).GetComponent<Rigidbody>().AddForce(barrelLocation.forward * shotPower);
    }

    // Creates casing behavior
    public void CasingRelease()
    {
        //Create the casing
        GameObject tempCasing;
        tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
        
        //Add force on casing to push it out
        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
        
        //Add torque to make casing spin in random direction
        tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

        //Destroy casing after X seconds
        Destroy(tempCasing, destroyTimer);
    }

}
