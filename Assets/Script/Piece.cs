using UnityEngine;

public enum PieceType
{
	none = -1,
	ramp = 0,
	longBlock = 1,
	jump = 2,
	slide = 3,
}

public class Piece : MonoBehaviour {
	public PieceType type;
	public int visualIndex;
}
