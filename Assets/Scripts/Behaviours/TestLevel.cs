using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLevel : MonoBehaviour {

  public GameObject playerPrefab;
  public GameObject outlinePrefab;

  private HexGrid grid;

  private PlayerController player;
  private Outline outline;

	// Use this for initialization
	void Start () {
    grid = GetComponentInChildren<HexGrid>();
    player = PlayerController.Construct(playerPrefab, grid).GetComponent<PlayerController>();
    outline = Outline.Construct(outlinePrefab, grid);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
