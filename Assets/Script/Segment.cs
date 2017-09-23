using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Segment : MonoBehaviour {

	public int SegID { set; get;}
	public bool transition;

	public int lenght;
	public int beginY1, beginY2, beginY3;
	public int endY1, endY2, endY3;

	public Piece[] pieces;

	private void Awake()
	{
		pieces = gameObject.GetComponentsInChildren<Piece> ();
	}

	public void Spawn()
	{
		gameObject.SetActive (true);
	}

	public void DeSpawn()
	{
		gameObject.SetActive (false);
	}

}
