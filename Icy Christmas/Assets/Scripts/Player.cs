using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

	public float mass;

	[SerializeField]
	private Transform fwd;
	[SerializeField]
	private Transform cam;

	public bool hasAGift;
	public float reachDistance;

	public GameObject basicTentacle;

	private CharacterController cc;

	public GameObject targetedCheminee;

	public Image snowFlake;

	public float usesLeft;
	public float maxUses;

	public GameObject presentIconFull;

	public Present presentCarried;

	public GameObject soundJingleBell;
	public GameObject iceBreaking;
	public GameObject magicPickupSound;

	public void Die()
	{









	}

	void Start () 
	{
		cc = GetComponent<CharacterController> ();
	}
	






	void Update () 
	{

		if (cc.isGrounded) {

			RaycastHit hit;
			Ray ray = new Ray (transform.position, Vector3.down);

			if (Physics.Raycast (ray, out hit, 3)) {
				if (hit.collider.GetComponent<MeshPart> ()) {

					float force = mass - cc.velocity.y;

					hit.collider.GetComponent<MeshPart> ().TestBreaking (force, iceBreaking);

					//print ("je fait une force de : "+force);
				}
			}
		}

		if (Input.GetMouseButtonDown (0) && usesLeft > 0) {
			LeftClick ();
			usesLeft--;
			snowFlake.fillAmount = usesLeft / maxUses;

		}

		if (Input.GetMouseButton (1))
			RightClick ();


		if (Input.GetKeyDown (KeyCode.E)) {


			Interact ();

		}

	}

	void RightClick()
	{
	}


	void Interact()
	{
		Ray ray = new Ray (cam.position, (fwd.position - cam.position).normalized);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, reachDistance)) {

			if (hit.collider.tag == "Cheminee") {
				if (targetedCheminee == hit.collider.gameObject && hasAGift) {
					Cheminee chem = hit.collider.GetComponent<Cheminee> ();

					chem.ReceiveGift (presentCarried);
					hasAGift = false;
					presentIconFull.SetActive (false);

				}
			}

			if (hit.collider.tag == "Present" && !hasAGift) {



				targetedCheminee = hit.collider.GetComponent<Present> ().cheminee;

				targetedCheminee.GetComponent<Cheminee> ().beam.SetActive (true);

				Instantiate (soundJingleBell, transform, false);
				hasAGift = true;
				presentIconFull.SetActive (true);


				hit.collider.gameObject.SetActive (false);

				presentCarried = hit.collider.gameObject.GetComponent<Present>();

			}
			if (hit.collider.tag == "ShardBonus" && usesLeft < maxUses) {

				Instantiate (magicPickupSound, transform.position, Quaternion.identity);

				usesLeft = Mathf.Min (usesLeft + 1, maxUses);
				snowFlake.fillAmount = usesLeft / maxUses;

				hit.collider.gameObject.SetActive (false);



			}

		}




	}

	void LeftClick()
	{
		Collider[] cols = Physics.OverlapSphere (transform.position, 1.5f);
		float min = 1000f;

		Vector3 closestPoint = Vector3.zero;

		GameObject o = null;

		foreach (Collider col in cols) {

			if (col.gameObject.name != "Trigger" && col.gameObject.tag != "Player") {


				Vector3 p = col.bounds.ClosestPoint (transform.position);
				float d = (p - transform.position).sqrMagnitude;

				if (d < min) {
					min = d;
					closestPoint = p;
					o = col.gameObject;
				}
			}
		}


		if ( closestPoint == Vector3.zero )
			return;

		Vector3 dir = fwd.position - cam.position;

		GameObject obj = Instantiate (basicTentacle, Vector3.zero, Quaternion.identity) as GameObject;

		Tentacle t = obj.GetComponent<Tentacle> ();

		if (o.GetComponent<MeshPart> ()) {
			MeshPart omp = o.GetComponent<MeshPart> ();

			t.origin = omp.startPosition;
			t.customOrigin = true;
			t.startRadius = omp.startRadius * 1.3f;
			t.direction = dir.normalized;

		} else {
			t.origin = transform.position;
			t.direction = dir.normalized;
			
			t.origin = closestPoint;

		}
		t.Go ();


	}
}
