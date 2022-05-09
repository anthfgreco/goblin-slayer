using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneScript : MonoBehaviour
{
        private Vector3 center = new Vector3(0.0f, 100.0f, 0.0f);
        [SerializeField] private int degreesPerSecond;
        [SerializeField] private GameObject player;
        public bool attackState;

        [Header("Missile Variables")]
        [SerializeField] private float missileChancePerTick;
        [SerializeField] private float spread;
        [SerializeField] private GameObject missile;
        [SerializeField] private float shootForce;

    // Start is called before the first frame update
    void Start()
    {
        attackState = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Spin the plane around the center at degreesPerSecond degrees/second.
        transform.RotateAround(center, Vector3.up, degreesPerSecond * Time.deltaTime);

        if (attackState) {
            Attack();
        }
    }

    void Attack() {
        if (Random.Range(0.0f, 1.0f) < missileChancePerTick) {
            // Calculate vector from plane to player
            Vector3 directionWithoutSpread = player.transform.position - this.transform.position;

            // Calculate spread
            float x = Random.Range(-spread, spread);
            float z = Random.Range(-spread, spread);

            // Calculate new direction from plane to player with spread
            Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, 0.0f, z);

            // Create missile
            GameObject currentMissile = Instantiate(missile, this.transform.position, Quaternion.identity);

            // Rotate missile to face player direction
            currentMissile.transform.forward = directionWithSpread;

            // Add forces to missile
            currentMissile.GetComponent<Rigidbody>().AddForce(directionWithSpread * shootForce);
        }
    }
}
