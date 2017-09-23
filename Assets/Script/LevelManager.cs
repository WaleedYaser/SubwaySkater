using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public static LevelManager Instance { set; get;}

	// Level spawning

	// List of pieces
	public List<Piece> ramps = new List<Piece>();
	public List<Piece> longBlocks = new List<Piece>();
	public List<Piece> jumps = new List<Piece>();
	public List<Piece> slides = new List<Piece>();
	public List<Piece> pieces = new List<Piece>(); // All pieces on the pool

	public Piece GetPiece(PieceType pt, int visualIndex)
	{
		Piece p = pieces.Find(x => x.type == pt && x.visualIndex == visualIndex && !x.gameObject.activeSelf);

		if (p == null) {
			GameObject go = null;

			switch (pt) {
			case PieceType.ramp:
				go = ramps [visualIndex].gameObject;
				break;
			case PieceType.longBlock:
				go = longBlocks [visualIndex].gameObject;
				break;
			case PieceType.jump:
				go = jumps [visualIndex].gameObject;
				break;
			case PieceType.slide:
				go = slides [visualIndex].gameObject;
				break;		
			}
			go = Instantiate (go);
			p = go.GetComponent<Piece> ();
			pieces.Add (p);
		}

		return p;
	}

}
 