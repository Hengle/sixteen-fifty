using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour {
  public GameObject stateManagerPrefab;

	// Use this for initialization
	void Awake () {
    if(StateManager.Instance == null)
      Instantiate(stateManagerPrefab);
	}
}
