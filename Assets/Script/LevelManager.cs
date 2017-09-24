using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public static LevelManager Instance { set; get;}

	private const bool SHOW_COLLIDERS = true;

	// Level spawning
	private const float DISTANCE_BEFORE_SPWAN = 100f;
	private const int INITIAL_SEGMENT = 10;
	private const int MAX_SEGMENT_ON_SCREEN = 15;
	private Transform cameraContainer;
	private int amountOfActiveSegment;
	private int continiousSegments;
	private int currentSpawnZ;
	private int currentLevel;
	private int y1, y2, y3;

	// List of pieces
	public List<Piece> ramps = new List<Piece>();
	public List<Piece> longBlocks = new List<Piece>();
	public List<Piece> jumps = new List<Piece>();
	public List<Piece> slides = new List<Piece>();
	[HideInInspector]
	public List<Piece> pieces = new List<Piece>(); // All pieces on the pool

	// List of segments
	public List<Segment> availableSegments = new List<Segment>();
	public List<Segment> availableTransition = new List<Segment>();
	[HideInInspector]
	public List<Segment> segments = new List<Segment> ();

	// Gameplay
	private bool isMoving;


	private void Awake()
	{
		cameraContainer = Camera.main.transform;
		currentSpawnZ = 0;
		currentLevel = 0;
	}

	private void Start()
	{
		for (int i = 0; i < INITIAL_SEGMENT; i++) {
			GenerateSegment ();
		}
	}

	private void Update()
	{
		if (currentSpawnZ - cameraContainer.position.z < DISTANCE_BEFORE_SPWAN)
		{
			GenerateSegment ();
		}

		if (amountOfActiveSegment >= MAX_SEGMENT_ON_SCREEN) {
			segments [amountOfActiveSegment - 1].DeSpawn ();
			amountOfActiveSegment--;
		}
	}

	private void GenerateSegment()
	{
		SpawnSegment ();

		if (Random.Range (0f, 1f) < (continiousSegments * 0.25f)) {
			SpawnTransition ();
			continiousSegments = 0;
		} else {
			continiousSegments++;
		}
	}

	private void SpawnSegment()
	{
		List<Segment> possibleSegments = availableSegments.FindAll (x => x.beginY1 == y1 || x.beginY2 == y2 || x.beginY3 == y3);
		int id = Random.Range (0, possibleSegments.Count);

		Segment s = GetSegment (id, false);
		y1 = s.endY1;
		y2 = s.endY2;
		y3 = s.endY3;

		s.transform.SetParent (transform);
		s.transform.localPosition = Vector3.forward * currentSpawnZ;

		currentSpawnZ += s.lenght;
		amountOfActiveSegment++;
		s.Spawn ();
	}

	private void SpawnTransition()
	{
		List<Segment> possibleTransition = availableTransition.FindAll (x => x.beginY1 == y1 || x.beginY2 == y2 || x.beginY3 == y3);
		int id = Random.Range (0, possibleTransition.Count);

		Segment s = GetSegment (id, true);
		y1 = s.endY1;
		y2 = s.endY2;
		y3 = s.endY3;

		s.transform.SetParent (transform);
		s.transform.localPosition = Vector3.forward * currentSpawnZ;

		currentSpawnZ += s.lenght;
		amountOfActiveSegment++;
		s.Spawn ();
	}

	public Segment GetSegment(int id, bool transition)
	{
		Segment s = null;
		s = segments.Find (x => x.SegID == id && x.transition == transition && !x.gameObject.activeSelf);

		if (s == null) {
			GameObject go = Instantiate (transition ? availableTransition [id].gameObject : availableSegments [id].gameObject) as GameObject;
			s = go.GetComponent<Segment> ();
			s.SegID = id;
			s.transition = transition;

			segments.Insert (0, s);
		} else {
			segments.Remove (s);
			segments.Insert (0, s);
		}

		return s;

	}

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
 