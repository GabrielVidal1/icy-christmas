using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshPart : MonoBehaviour {


	public float length = 2f;

	public int numberOfSides = 8;

	public float startRadius;
	public Vector3 startDirection;
	public Vector3 startPosition;


	public float endRadius;
	public Vector3 endDirection;
	public Vector3 endPosition;



	public float noise;


	private Circle circleStart;
	private Circle circleEnd;


	private List<Vector3> triangles;
	private List<Vector3> points;
	private List<Vector3> uv;

	private Mesh mesh;

	[HideInInspector]
	public Tentacle tentacle;

	public float resistance;
	public float forceApplied;

	private bool broken;

	IEnumerator Disappear()
	{
		yield return new WaitForSeconds (0f);

		while (endRadius > 0.01f) {

			endRadius *= .9f;
			startRadius *= .9f;

			UpdateMesh ();

			yield return new WaitForSeconds (.1f);

		}

		Destroy (gameObject);



	}

	public void Break()
	{
		if (!broken) {

			broken = true;

			gameObject.AddComponent<Rigidbody> ();

			transform.parent = null;

			if (transform.childCount == 1) {
				transform.GetChild (0).GetComponent<MeshPart> ().Break ();
			}

			StartCoroutine (Disappear ());
		}
	}

	public void TestBreaking( float force, GameObject audio)
	{
		if (resistance < force) {
			Instantiate (audio, transform.position, Quaternion.identity);
			Break ();
		}

	}

	public void Init()
	{
		broken = false;

		triangles = new List<Vector3> ();
		points = new List<Vector3> ();
		uv = new List<Vector3> ();

		mesh = new Mesh ();

		circleStart = new Circle (startRadius, startPosition, startDirection, numberOfSides, noise);

		circleEnd = new Circle (endRadius, endPosition, endDirection,  numberOfSides, noise);


		LinkCircles ();

		List<int> list = new List<int> ();
		foreach (Vector3 t in triangles) {
			list.Add ((int)t.x);
			list.Add ((int)t.y);
			list.Add ((int)t.z);
		}


		mesh.vertices = points.ToArray ();
		mesh.triangles = list.ToArray ();

		mesh.RecalculateNormals ();

		GetComponent<MeshFilter> ().mesh = mesh;
	}



	public void UpdateMesh()
	{
		points.Clear ();

		circleStart = new Circle (startRadius, startPosition, startDirection, numberOfSides, noise);

		circleEnd = new Circle (endRadius, endPosition, endDirection,  numberOfSides, noise);

		points.Add (startPosition);
		points.Add (endPosition);


		Vector3 p1;
		Vector3 p2;
		Vector3 p3;
		Vector3 p4;

		for (int i = 1; i < numberOfSides + 1; i++) {

			if (i < numberOfSides) {
				p1 = circleStart.cPoint [i - 1];
				p2 = circleStart.cPoint [i];
				p3 = circleEnd.cPoint [i - 1];
				p4 = circleEnd.cPoint [i];
			} else {
				p2 = circleStart.cPoint [0];
				p1 = circleStart.cPoint [numberOfSides - 1];
				p4 = circleEnd.cPoint [0];
				p3 = circleEnd.cPoint [numberOfSides - 1];
			}
			////////////////////////////////////////

			points.Add (p1);
			points.Add (p2);
			points.Add (p3);
			points.Add (p4);



			////////////////////////////////////////

			points.Add (p1);
			points.Add (p2);
			points.Add (p3);

			points.Add (p3);
			points.Add (p4);
			points.Add (p2);
		}


		mesh.vertices = points.ToArray ();
		mesh.RecalculateNormals ();
		GetComponent<MeshFilter> ().mesh = mesh;
	}
		
	private void LinkCircles()
	{

		Vector3 p1;
		Vector3 p2;
		Vector3 p3;
		Vector3 p4;

		//indice 0 -> startCircleCenter
		//indice 1 -> endCircleCenter

		points.Add (startPosition);
		points.Add (endPosition);


		for (int i = 1; i < numberOfSides + 1; i++) {

			if (i < numberOfSides) {
				p1 = circleStart.cPoint [i - 1];
				p2 = circleStart.cPoint [i];
				p3 = circleEnd.cPoint [i - 1];
				p4 = circleEnd.cPoint [i];
			} else {
				p2 = circleStart.cPoint [0];
				p1 = circleStart.cPoint [numberOfSides - 1];
				p4 = circleEnd.cPoint [0];
				p3 = circleEnd.cPoint [numberOfSides - 1];
			}

			points.Add (p1);
			points.Add (p2);

			triangles.Add (new Vector3 (
				points.LastIndexOf (p1), 
				points.LastIndexOf (p2),
				0f));




			points.Add (p3);
			points.Add (p4);

			triangles.Add (new Vector3 (
				points.LastIndexOf (p4),
				points.LastIndexOf (p3), 
				1f));

			////////////////////////////////////////

			points.Add (p1);
			points.Add (p2);
			points.Add (p3);

			triangles.Add (new Vector3 (
				points.LastIndexOf (p3),
				points.LastIndexOf (p2),
				points.LastIndexOf (p1)
			));
			////////////////////////////////////

			points.Add (p3);
			points.Add (p4);
			points.Add (p2);

			triangles.Add (new Vector3 (
				points.LastIndexOf (p3),
				points.LastIndexOf (p4),
				points.LastIndexOf (p2)
			));


			///////////////////////////////////////

		}


	}
}






[System.Serializable]
public class Circle
{
	public float cRadius;
	public Vector3 cPosition;
	public Vector3 cDir;

	//[HideInInspector]
	//public Vector3 cNormal;

	[HideInInspector]
	public Vector3[] cPoint;

	[HideInInspector]
	public int cSides;

	private Transform parent;

	public Vector3[] pointsRayon;

	private float cNoise;

	public Circle( float radius, Vector3 position, Vector3 dir, int sides, float noise = 0f )
	{
		cRadius = radius;
		cPosition = position;
		cDir = dir;
		cSides = sides;
		cNoise = noise;

		CalculatePointsPosition ();
	}
		


	public void CalculatePointsPosition()
	{
		List<Vector3> ListPoints = new List<Vector3> ();

		Vector3 u = Vector3.Cross (cDir, Vector3.up).normalized;
		Vector3 v = Vector3.Cross (u, cDir).normalized;
		float pi2 = 2 * Mathf.PI / cSides;

		Vector3 pos = cPosition ;

		float aNoise = 0.1f * cRadius;
		float tNoise = aNoise + 0.000000001f;

		for (int i = 0; i < cSides; i++) {
			Vector3 point;


			float nRadius = Mathf.PerlinNoise ( pos.x + pi2 * i, ( pos.y + pos.z) * pi2 * i);

			nRadius = (1 - cNoise) + cNoise * nRadius;

			point = cPosition + cRadius * nRadius * (Mathf.Cos (pi2 * i) * u + Mathf.Sin (pi2 * i) * v);

			Vector3 pNoise = Noise3D (point / tNoise, 100f ) * aNoise;

			point += pNoise;

			ListPoints.Add (point);
		}



		cPoint = new Vector3[ListPoints.Count];
		cPoint = ListPoints.ToArray ();


	}

	public Vector3 Noise3D( Vector3 p, float offset )
	{
		Vector3 nP = Vector3.zero;

		nP.x = Mathf.PerlinNoise (p.y, p.z);
		nP.y = Mathf.PerlinNoise (p.z, p.x + offset);
		nP.z = Mathf.PerlinNoise (p.x, p.y + offset);

		nP.x = (nP.x * 2) - 1;
		nP.y = (nP.y * 2) - 1;
		nP.z = (nP.z * 2) - 1;


		return nP;
	}
}
