using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// YOU must implement the Slingshot

public class Slingshot : MonoBehaviour {
    static private Slingshot S;

    [Header("Set in Inspector")]

    // Place class variables here
    public GameObject prefabProjectile;
    public float velocityMult = 8f;

    [Header("Set Dynamically")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;
    private Rigidbody projectileRigidbody;

    static public Vector3 LAUNCH_POS
    {
        get
        {
            if (S == null) return Vector3.zero;
            return S.launchPos;
        }
    }


    void Awake()
    {
        S = this;
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;
    }

    void OnMouseEnter()
    {
        //print("Slingshot: OnMouseEnter()");
        launchPoint.SetActive(true);
    }

    void OnMouseExit()
    {
        //print("Slingshot:OnMouseExit()");
        launchPoint.SetActive(false);
    }

    private void OnMouseDown()
    {
        //player has pressed the  mouse button
        aimingMode = true;
        //instantiate a projectile
        projectile = Instantiate( prefabProjectile ) as GameObject;
        //start it at launchpoint
        projectile.transform.position = launchPos;
        //set it to isKinematic for now
        projectile.GetComponent<Rigidbody>().isKinematic = true;

        //set it to isKinematic for now
        projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.isKinematic = true;
    }

    void Update()
    {
        //if slingshot is not in aimingMode, don't run
        if (!aimingMode)
            return;

        //get the current mouse position in 2d screen coordinates
        Vector3 mousePos2d = Input.mousePosition;
        mousePos2d.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2d);

        //Find the delta from the launchPos to the mousePos3D
        Vector3 mouseDelta = mousePos3D - launchPos;

        //Limit mouseDelta to the radius of the slingshot SphereCollider
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        //move the projectile to this new position
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;
        if (Input.GetMouseButton(0))
        {
            //the mouse has been released
            aimingMode = false;
            projectileRigidbody.isKinematic = false;
            projectileRigidbody.velocity = -mouseDelta * velocityMult;
            FollowCam.POI = projectile;
            projectile = null;
            MissionDemolition.ShotFired();
            ProjectileLine.S.poi = projectile;
        }
    }


}
