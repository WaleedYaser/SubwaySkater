using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceSpawner : MonoBehaviour {

	public PieceType type;
	private Piece currentPiece;

	public void Spawn()
	{
		currentPiece = LevelManager.Instance.GetPiece (type, 0);
		currentPiece.gameObject.SetActive (true);
		currentPiece.transform.SetParent (transform, false);
	}

	public void DeSpawn()
	{
		currentPiece.gameObject.SetActive (false);
	}
}
