using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLevel : MonoBehaviour {
  public GameObject playerPrefab;

  public HexGrid grid {
    get;
    private set;
  }

	// Use this for initialization
	void Start () {
    grid = GetComponentInChildren<HexGrid>();
	}
}
