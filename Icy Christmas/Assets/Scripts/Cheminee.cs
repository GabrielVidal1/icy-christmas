using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheminee : MonoBehaviour {

	public bool isFull;

	public GameObject[] presents;

	private LevelManager lm;

	public GameObject beam;

	private int wantedGifts;

	public Present animationPresent;

	private Animator animator;

	private AudioSource audio;

	void Start()
	{
		
		lm = GameObject.FindWithTag ("LevelManager").GetComponent<LevelManager>();

		wantedGifts = 0;

		animator = GetComponent<Animator> ();

		audio = GetComponent<AudioSource> ();
	}

	public void PlayReceivalSound()
	{
		audio.Play ();
	}

	public void ReceiveGift(Present present)
	{


		animator.SetTrigger ("ReceiveGift");


		animationPresent.ribbon.GetComponent<Renderer> ().material = present.ribbonColor;
		animationPresent.gift.GetComponent<Renderer> ().material = present.giftPaperWrap;

		wantedGifts++;
		if (wantedGifts == presents.Length)
			isFull = true;


	}

	public void Test()
	{
		lm.Test ();
		beam.SetActive (false);

	}

}
