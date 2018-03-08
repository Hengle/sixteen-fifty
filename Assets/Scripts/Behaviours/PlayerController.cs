using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
  /**
   * How fast the player moves, in units per seconds.
   */
  public const float MOVE_SPEED = 16f;

  // set in inspector
  new public PlayerRenderer renderer;

  // set by Construct
  public HexGrid grid;

  /**
   * Raised every time the player enters a cell.
   * When the player is moving, it will leave and enter cells in quick
   * succession.
   */
  public event Action<HexCell> LeaveCell;

  /**
   * Raised every time the player leaves a cell.
   * When the player is moving, it will leave and enter cells in quick
   * succession.
   */
  public event Action<HexCell> EnterCell;

  /**
   * The cell the player is currently occupying.
   * Note: while the player is in motion, this value becomes stale
   * until the player enters at a new cell.
   * Every time the player passes through a cell, this value gets
   * updated.
   */
  public HexCell CurrentCell {
    get;
    private set;
  }

  /**
   * Holds the coroutine responsible for moving the player if the
   * player is in movement.
   */
  private Coroutine movement;

  /**
   * If the player's movement is cancelled, then the player will stop
   * moving when it arrives at its next intermediate destination,
   * after raising the EnterCell event.
   */
  private bool movementCancelled;

  /**
   * Cancels the player's movement.
   * Calling this function is a no-op if the player is not moving.
   */
  public void CancelMovement() {
    if(null == movement) {
      Debug.LogWarning("Player movement cancelled, but the player was not moving.");
      return;
    }

    movementCancelled = true;
  }

  public static PlayerController Construct(GameObject prefab, HexGrid grid) {
    var self = prefab.GetComponent<PlayerController>();
    self.grid = grid;
    var instance = Instantiate(prefab).GetComponent<PlayerController>();
    self.grid = null;

    instance.transform.parent = grid.transform;

    instance.Warp(grid[HexCoordinates.Zero]);

    return instance;
  }

  /**
   * Teleports the player instantly to the given cell.
   */
  public void Warp(HexCell cell) {
    if(null != cell) {
      CurrentCell = cell;
    }
    else {
      Debug.LogError("refusing to warp to null!");
    }

    transform.localPosition = cell.coordinates.ToPosition();
  }

  void OnEnable() {
    EnableClickToMove();
  }

  void EnableClickToMove() {
    grid.CellDown += OnCellDown;
  }

  void OnDisable() {
    DisableClickToMove();
  }

  void DisableClickToMove() {
    grid.CellDown -= OnCellDown;
  }

  /**
   * Event handler for cell clicks.
   */
  void OnCellDown(HexCell cell) {
    if(cell == CurrentCell) {
      Debug.Log("clicked on the player's cell! Nothing to do!");
    }
    else {
      if(movement != null) {
        Debug.LogError("tried to start moving, but we're already moving.");
        return;
      }
      var path = grid.FindPath(CurrentCell.coordinates, cell.coordinates);
      if(null == path) {
        Debug.Log("No path can be found.");
        return;
      }
      movement = StartCoroutine(MoveFollowingPath(path));
    }
  }

  /**
   * A coroutine that moves the player through a list of cells.
   * The cells should each be sequentially adjacent.
   */
  IEnumerator MoveFollowingPath(IEnumerable<HexCell> path) {
    Debug.Assert(null != path);
    DisableClickToMove();
    movementCancelled = false;

    var speed = MOVE_SPEED * Time.deltaTime;

    try {
      foreach(var cell in path) {
        if(movementCancelled) {
          Debug.Log("Movement cancelled!");
          yield break;
        }

        Debug.Log("Moving to next position: " + cell.coordinates.ToString());

        // raise an event saying that we're leaving the current cell.
        if(null != LeaveCell) {
            LeaveCell(CurrentCell);
        }
        Vector3 target = cell.coordinates.ToPosition();
        float remaining = 0;
        do {
          remaining = (target - transform.position).sqrMagnitude;
          transform.position = Vector3.MoveTowards(transform.position, target, speed);
          yield return null;
        }
        while(remaining > float.Epsilon);

        CurrentCell = cell;
        if(null != EnterCell) {
            EnterCell(CurrentCell);
        }
      }

      Debug.Log("Movement complete!");
    }
    finally {
      EnableClickToMove();
      movementCancelled = false;
      movement = null;
      Debug.Log("Movement cleanup complete.");
    }
  }

	// Use this for initialization
	void Awake() {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
