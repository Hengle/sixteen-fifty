using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Commands;

/**
 * A mapentity component should be attached to any objects that exist
 * on top of the map, at particular coordinates.
 */
public class MapEntity : MonoBehaviour {
  /**
   * How fast the player moves, in units per seconds.
   */
  public const float MOVE_SPEED = 8f;


  /**
   * The cell the entity is currently occupying.
   * Note: while the player is in motion, this value becomes stale
   * until the player enters at a new cell.
   * Every time the player passes through a cell, this value gets
   * updated.
   */
  public HexCell CurrentCell {
    get;
    private set;
  }

  public event Action<HexCell> LeaveCell;
  public event Action<HexCell> EnterCell;

  /**
   * Holds the coroutine responsible for moving the entity if it is in
   * movement.
   */
  private Coroutine movement;

  public bool IsMoving => movement != null;

  /**
   * If the player's movement is cancelled, then the player will stop
   * moving when it arrives at its next intermediate destination,
   * after raising the EnterCell event.
   */
  private bool movementCancelled;

  /**
   * Teleports the entity instantly to the given cell.
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

  /**
   * Cancels the entity's movement.
   * Calling this function is a no-op (but issues a warning) if the
   * entity is not moving.
   */
  public void CancelMovement() {
    if(null == movement) {
      Debug.LogWarning("Player movement cancelled, but the player was not moving.");
      return;
    }

    movementCancelled = true;
  }

  void Awake () {
  }

  public Command<object> MoveFollowingPath(IEnumerable<HexCell> path, float moveSpeed = DoMoveFollowingPath.DEFAULT_MOVE_SPEED) {
    return new DoMoveFollowingPath(this, path, moveSpeed);
  }

  class DoMoveFollowingPath : Command<object> {
    public const float DEFAULT_MOVE_SPEED = 0.4f;
    IEnumerable<HexCell> path;
    float moveSpeed;
    MapEntity self;
    
    public DoMoveFollowingPath(
      MapEntity self,
      IEnumerable<HexCell> path,
      float moveSpeed = DEFAULT_MOVE_SPEED) {

      this.self = self;
      this.path = path;
      this.moveSpeed = moveSpeed;
    }

    public override IEnumerator GetCoroutine() {
      foreach(var cell in path) {
        if(self.movementCancelled) {
          Debug.Log("Movement cancelled!");
          yield break;
        }

        Debug.Log("Moving to next position: " + cell.coordinates.ToString());

        // raise an event saying that we're leaving the current cell.
        if(null != self.LeaveCell) {
            self.LeaveCell(self.CurrentCell);
        }
        Vector3 target = cell.coordinates.ToPosition();
        float remaining = 0;
        do {
          remaining = (target - self.transform.position).sqrMagnitude;
          self.transform.position = Vector3.MoveTowards(self.transform.position, target, moveSpeed);
          yield return null;
        }
        while(remaining > float.Epsilon);

        self.CurrentCell = cell;
        if(null != self.EnterCell) {
            self.EnterCell(cell);
        }
      }
    }
  }
}
