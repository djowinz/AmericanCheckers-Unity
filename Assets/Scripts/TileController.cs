using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour {
	public bool isBlackPieceTouching = false;
	public bool isWhitePieceTouching = false;
	public int tileIndex = 0;
	public string tileColumn = "";
	public string tileName = "";

	void OnCollisionEnter(Collision collision) {
		Collider contactCollider = collision.collider;
//		Debug.Log (contactCollider.gameObject.tag);
		if (contactCollider.gameObject.tag == "Black Pieces") {
			isBlackPieceTouching = true;
		} else if (contactCollider.gameObject.tag == "White Pieces") {
			isWhitePieceTouching = true;
		}
	}

	void OnCollisionExit(Collision collision) {
		isBlackPieceTouching = false;
		isWhitePieceTouching = false;
	}
}
