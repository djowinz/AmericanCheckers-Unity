using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public Material selectedMaterial;
	public Material unselectedMaterial;
	public Material availableTileMaterial;
	public GameObject[] whitePieces;
	public GameObject[] blackPieces;
	public GameObject[] tiles;
	public bool playerTurn = true;
	public GameObject activePiece;

	private List<string> gameBoardPositions = new List<string> ();
	private List<string> availableMoves = new List<string>();
	private string[] columns = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };

	// True start before scene start
	void Awake () {
		whitePieces = GameObject.FindGameObjectsWithTag ("White Pieces");
		blackPieces = GameObject.FindGameObjectsWithTag ("Black Pieces");
		tiles = GameObject.FindGameObjectsWithTag ("GameTiles").OrderBy(tile => tile.name).ToArray(); // Ordered tiles for level of consistency

		int pos = 0;
		int column = 0;
		int maxPos = 8;

		foreach (GameObject tile in tiles) {
			if (pos == 8) {
				column++;
				pos = 0;
			}
			TileController tileCont = tile.GetComponent<TileController> ();

			tileCont.tileColumn = columns [column];
			tileCont.tileIndex = pos;
			tileCont.tileName = columns [column] + pos;

			pos++;
		}
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0) && playerTurn) {
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit, 100.0f)) {
				if (hit.transform.tag == "Black Pieces" && activePiece == null) {
					setPlayerPiece (hit.transform.gameObject, true);
				} else if (hit.transform.tag != "Black Pieces" || hit.transform.tag != "White Pieces") {
					if (hit.transform.tag == "GameTiles") {
						TileController tileCont = hit.transform.gameObject.GetComponent<TileController> ();
						if (activePiece != null && !tileCont.isBlackPieceTouching && !tileCont.isWhitePieceTouching) {
							moveGamePiece (hit.transform.gameObject);
						}
					}
				}
			}
		}
	}

	void LateUpdate () {
		if (gameBoardPositions.Count < 1) {
			this.updateGamePositions ();
		}
	}

	void updateGamePositions () {
		tiles = GameObject.FindGameObjectsWithTag ("GameTiles").OrderBy(tile => tile.name).ToArray();
		gameBoardPositions = new List<string> ();
		List<string> columnVales = new List<string> ();
		int pos = 0;
		int column = 0;
		int maxPos = 8;
	
		foreach (GameObject tile in tiles) {
			TileController tileCont = tile.GetComponent<TileController> ();
//			Debug.Log ("White: " + tileCont.isWhitePieceTouching + " Black: " + tileCont.isBlackPieceTouching + " Empty: " + (!tileCont.isWhitePieceTouching && !tileCont.isBlackPieceTouching));
			if (tileCont.isBlackPieceTouching) {
				gameBoardPositions.Add ("1");
				columnVales.Add ("1");
			} else if (tileCont.isWhitePieceTouching) {
				gameBoardPositions.Add ("2");
				columnVales.Add ("2");
			} else if (!tileCont.isWhitePieceTouching && !tileCont.isBlackPieceTouching) {
				gameBoardPositions.Add ("0");
				columnVales.Add ("0");
			}
			if (pos == 7) {
				Debug.Log ("Column: " + columns [column] + " Values: " + string.Join (" ", columnVales.ToArray ()));
				columnVales = new List<string> ();
				column++;
				pos = 0;
			}
			pos++;
		}

//		Debug.Log("Positions = " + string.Join(" ",
//			new List<string>(gameBoardPositions)
//				.ConvertAll(p => p.ToString())
//				.ToArray())
//		);
	}

	void setPlayerPiece(GameObject pieceObject, bool moving) {
		MeshRenderer pieceMesh = pieceObject.GetComponent<MeshRenderer> ();
		if (moving) {
			pieceMesh.material = selectedMaterial;
			activePiece = pieceObject;
		} else {
			pieceMesh.material = unselectedMaterial;
		}
	}



	void moveGamePiece(GameObject hitElement) {
		Vector3 selectedPosition = hitElement.transform.position;
		if (isMoveValid (selectedPosition, hitElement)) {
			Vector3 activePiecePosition = activePiece.transform.position;
			activePiece.transform.position = new Vector3(selectedPosition.x, activePiecePosition.y, selectedPosition.z);
			setPlayerPiece (activePiece, false);
			activePiece = null;
			this.updateGamePositions ();
			this.performAIMove ();
		}
	}

	bool isMoveValid(Vector3 desiredPosition, GameObject hitElement) {
		if (hitElement.transform.tag != "Black Pieces" && hitElement.transform.tag != "White Pieces") {
			return true;
		} else {
			return false;
		}
	}

	void performAIMove() {

	}
}
