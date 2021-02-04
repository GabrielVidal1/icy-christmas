using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour {

	public float length = 50f;
	public float density = 1f;
	public float volumicResistance = 1f;
	public float gravityFragility = 1f;
	public float flat = .5f;

	public int numberOfSides = 7;

	public float noiseAmplitude = 1f;
	public float noiseSize = 0.07f;

	public float startRadius = 1f;
	public float endRadius = 0.05f;

	public Vector3 origin;
	public Vector3 direction;

	public Material sectionMat;
	public float sectionNoise = .5f;

	public float timeToFullyGrow = 3f;
	public AnimationCurve RadiusOverLenght;
	public AnimationCurve tentacleLengthOverTime;
	public AnimationCurve sectionRadiusOverTime;

	public bool customOrigin;

	private List<Vector3> points;
	private List<Vector3> directions;

	[SerializeField]
	private List<float> endRadiuses;

	private List<float> nRadiuses;

	private MeshPart[] sections;

	private int partNumber;
	[SerializeField]
	private float randomOffset;

	public void Go()
	{
		//origin.y -= startRadius;

		partNumber = Mathf.Max( (int)(density * length), 2 );
		sections = new MeshPart[partNumber];
		nRadiuses = new List<float> ();
		
		randomOffset = Random.value;

		BuildPoints ();
		BuildMeshes ();
		StartCoroutine (Grow (3));
	}

	void Update () {
		
	}

	void BuildPoints()
	{
		points = new List<Vector3> ();
		directions = new List<Vector3> ();
		endRadiuses = new List<float> ();

		float tNoise = length * noiseSize;

		Vector3 pos = origin;
		Vector3 posPrec = pos;
		Vector3 dir = direction.normalized;
		Vector3 dirInit = .1f * dir + .9f * Vector3.up;

		if (customOrigin) {
			points.Add (origin);
			directions.Add (dir);
		} else {
			points.Add (pos - new Vector3 (0f, startRadius, 0f));
			directions.Add (dirInit);
		}


		endRadiuses.Add (startRadius);

		float delta = length / (float)partNumber;



		for (int i = 0; i < partNumber; i++) {

			float coef = (float)i / (float)partNumber;

			float coefNoise = Mathf.Pow (coef, .9f);
			float coefRadius = RadiusOverLenght.Evaluate (coef);

			float radius = (1 - coefRadius) * startRadius + coefRadius * endRadius;
			endRadiuses.Add (radius);

			dir += Noise3D (pos / tNoise, 1000f * randomOffset) * coefNoise;
			dir.y *= 1 - flat;

			dir.Normalize ();

			pos += dir * delta;

			directions.Add ((pos - posPrec).normalized);
			posPrec = pos;
			points.Add (pos);
		}
	}

	void BuildMeshes()
	{


		for (int i = 0; i < partNumber; i++) {

			GameObject m = new GameObject ();

			m.AddComponent<MeshFilter> ();
			m.AddComponent<MeshRenderer> ();

			MeshCollider mC = m.AddComponent<MeshCollider> ();
			MeshPart mesh = m.AddComponent<MeshPart> ();

			m.GetComponent<MeshRenderer> ().material = sectionMat;

			mesh.numberOfSides = numberOfSides;
			mesh.noise = sectionNoise;

			mesh.startRadius = 0.01f;
			mesh.startPosition = points [i];
			mesh.startDirection = directions [i];

			mesh.endRadius = 0.01f;
			mesh.endPosition = points [i+1];
			mesh.endDirection = directions [i+1];

			mesh.tentacle = this;

			mesh.resistance = ( 3.14f * endRadiuses [i] * mesh.length * volumicResistance ) / (gravityFragility);

			mesh.Init ();

			mC.sharedMesh = mesh.GetComponent<MeshFilter> ().mesh;
			mC.convex = true;

			sections [i] = mesh;

			if (i == 0)
				mesh.transform.SetParent (transform);
			else
				mesh.transform.SetParent (sections [i - 1].transform);


			sections [i].gameObject.SetActive (false);


		}
	}

	IEnumerator Grow(int framesPerUpdate)
	{
		float time = 0f;

		float delay = framesPerUpdate * Time.deltaTime;

		while ( time < timeToFullyGrow )
		{
			UpdateRadiusesOverTime (time);

			time += delay;
			yield return new WaitForSeconds (delay);
		}

		UpdateRadiusesOverTime (timeToFullyGrow);

	}


	void UpdateRadiusesOverTime(float timeFromGrowthStart)
	{
		float T = timeFromGrowthStart / timeToFullyGrow;

		if (T == 1) {
			nRadiuses = endRadiuses;
		} else {

			nRadiuses.Clear ();
			for (int i = 0; i < partNumber; i++) {

				float pos = (float)i / (float)partNumber;

				float nRadius = endRadiuses[ i ] * sectionRadiusOverTime.Evaluate( T );

				nRadiuses.Add (nRadius);

			}

		nRadiuses.Add (0.001f);
		}

		for (int i = 0; i < partNumber; i++) {

			MeshPart mesh = sections [i];

			mesh.startRadius = nRadiuses [i] + 0.01f;
			mesh.endRadius = nRadiuses [i+1] + 0.01f;

			mesh.UpdateMesh ();
			UpdateCollider (mesh.gameObject);

			if (mesh.startRadius > 0.1f) {
				mesh.gameObject.SetActive (true);
			}
		}
	}

	void UpdateCollider( GameObject mesh )
	{
		mesh.GetComponent<MeshCollider> ().sharedMesh = mesh.GetComponent<MeshFilter> ().mesh;
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
