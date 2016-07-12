using UnityEngine;
using System.Collections;

public class ClickHandler : MonoBehaviour {
	public int fieldNr;
	public TicTacToe ticTacToe;

	void OnMouseDown() 
	{
		if(ImageTargetTrackableEventHandler.tracking){
			GetComponent<Collider>().enabled = false;

			ticTacToe.MainFunc(fieldNr);
		}
	}
}
