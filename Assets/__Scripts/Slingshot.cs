using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour {

	static public Slingshot S;

	public GameObject prefabProjectile;
	public float velocityMult = 4f;
	public bool _____________________;
	public GameObject launchPoint;
	public Vector3 launchPos;
	public GameObject projectile;
	public bool aimingMode;

	void Awake () {
		//Set Slingshot singleton S
		S = this;

		Transform launchPointTrans = transform.Find("LaunchPoint");
		launchPoint = launchPointTrans.gameObject;
		launchPoint.SetActive(false);
		launchPos = launchPointTrans.position;
	}

	void OnMouseEnter() {
		launchPoint.SetActive(true);
	}

	void OnMouseExit() {
		launchPoint.SetActive(false);
	}

	void OnMouseDown() {
		aimingMode = true;
		//Instantiate a Projectile
		projectile = Instantiate(prefabProjectile) as GameObject;
		//Start @launchPoint
		projectile.transform.position = launchPos;
		projectile.GetComponent<Rigidbody>().isKinematic = true;
	}

	void Update ()
	{
		if (!aimingMode) return;
		// Get the current mouse position in 2D screen coordinates
		Vector3 mousePos2D = Input.mousePosition;
		// Convert the mouse position to 3D world coordinates
		mousePos2D.z = -Camera.main.transform.position.z;
		Vector3 mousePos3D = Camera.main.ScreenToWorldPoint (mousePos2D);
		// Find the delta from the launchPos to the mousePos3D
		Vector3 mouseDelta = mousePos3D - launchPos;
		// Limit mouseDelta to the radius of the Slingshot SphereCollider
		float maxMagnitude = this.GetComponent<SphereCollider> ().radius;
		if (mouseDelta.magnitude > maxMagnitude) {
			mouseDelta.Normalize ();
			mouseDelta *= maxMagnitude;
		}
		// Move the projectile to this new position
		Vector3 projPos = launchPos + mouseDelta;
		projectile.transform.position = projPos;
		if (Input.GetMouseButtonUp (0)) {
			// The mouse has been released
			aimingMode = false;
			projectile.GetComponent<Rigidbody> ().isKinematic = false;
			projectile.GetComponent<Rigidbody> ().velocity = -mouseDelta * velocityMult;
			FollowCam.S.poi = projectile;
			projectile = null;
			MissionDemolition.ShotFired();
		}
	}
}
