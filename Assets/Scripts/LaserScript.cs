using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    public LineRenderer laserLineRenderer;
    public RaycastHit hit;
    public bool laserActive;
    public LeftController leftControllerScript;
    public Vector3 laserPointPos;
    private int LayerGround;

    // Start is called before the first frame update
    void Start()
    {
        laserLineRenderer = GetComponent<LineRenderer>();
        laserActive = false;
        laserLineRenderer.enabled = laserActive;
        LayerGround = LayerMask.NameToLayer("Ground");
    }

    // Update is called once per frame
    void Update()
    {

        if (laserActive == false)
        {
            laserLineRenderer.enabled = false;
        }
        else
        {
            laserLineRenderer.enabled = true;
            Ray ray = new Ray(transform.position, transform.forward);
            laserLineRenderer.SetPosition(0, transform.position);
            RaycastHit hit;       

            // If ray hits the ground
            if (Physics.Raycast(ray, out hit, 100) && hit.transform.gameObject.layer == LayerGround)
            {
                leftControllerScript.canTeleport = true;
                laserLineRenderer.SetPosition(1, hit.point);
                laserPointPos = hit.point;
            }
            // If ray hits object that is not the ground
            else if (Physics.Raycast(ray, out hit, 100) && hit.transform.gameObject.layer != LayerGround)
            {
                leftControllerScript.canTeleport = false;
                laserLineRenderer.SetPosition(1, hit.point);
            }
            // If ray does not hit an object
            else
            {
                leftControllerScript.canTeleport = false;
                laserLineRenderer.SetPosition(1, ray.GetPoint(100));
            }
        }
    }
}
