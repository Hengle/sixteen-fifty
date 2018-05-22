using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMenuButton : MonoBehaviour {
  PlayerMenuController playerMenuController;

  void Awake () {
    playerMenuController = GetComponentInParent<PlayerMenuController>();
    Debug.Assert(null != playerMenuController, "PlayerMenuButton exists within a PlayerMenuController.");
  }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
