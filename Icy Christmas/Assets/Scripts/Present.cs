using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Present : MonoBehaviour {

	public GameObject gift;
	public GameObject ribbon;

	public Material[] colors;
	public Material[] paperWraps;

	public Material ribbonColor;
	public Material giftPaperWrap;


	public GameObject cheminee;

	void Start () 
	{
		ribbonColor = colors [Random.Range (0, colors.Length - 1)];
		giftPaperWrap = paperWraps [Random.Range (0, paperWraps.Length - 1)];
		gift.GetComponent<Renderer> ().material = giftPaperWrap;
		ribbon.GetComponent<Renderer> ().material = ribbonColor;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
