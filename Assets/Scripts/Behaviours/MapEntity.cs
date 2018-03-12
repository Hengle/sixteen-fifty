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
  public const float MOVE_SPEED = 6f;

  /**
   * The cell the entity is currently occupying.
   * Note: while the player is in motion, this value becomes stale
   * until the player enters at a new cell.
   * Every time the player passes through a cell, this value gets
   * updated.
   */
  private HexCell currentCell;
  public HexCell CurrentCell {
    get {
      return currentCell;
    }
    private set {
      if(null != currentCell)
        currentCell.RemoveEntity(this);
      currentCell = value;
      currentCell.AddEntity(this);
    }
  }

  public event Action<MapEntity> LeaveCell;
  public event Action<MapEntity> EnterCell;

  public event Action<MapEntity> BeginMove;
  /**
   * Note that when this is fired, MovementCancelled will be false if
   * the move was cancelled.
   */
  public event Action<MapEntity> EndMove;

  /**
   * Raised during movement whenever there's a change of direction.
   */
  public event Action<MapEntity, HexDirection> ChangeDirection;

  private Coroutine movement;

  public bool IsMoving => movement != null;

  /**
   * If the player's movement is cancelled, then the player will stop
   * moving when it arrives at its next intermediate destination,
   * after raising the EnterCell event.
   */
  public bool MovementCancelled {
    get;
    private set;
  }

  public HexGrid grid;

  /**
   * Teleports the entity instantly to the given cell.
   */
  public void Warp(HexCell cell) {
    Debug.Assert(null != cell);
    CurrentCell = cell;
    transform.localPosition = cell.coordinates.ToPosition();
  }

  public void Warp(HexCoordinates coords) {
    Warp(grid[coords]);
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

    MovementCancelled = true;
  }

  void Awake () {
    grid = this.GetComponentInParentNotNull<HexGrid>();
  }

  /**
   * Asynchronously begins moving the entity along the given path.
   * The path's cells should all be sequentially adjacent.
   * The path should not include the cell that the entity is currently
   * on.
   * Fires BeginMove when the move actually starts and fires EndMove
   * when the movement ends.
   * Fires LeaveCell and EnterCell as the player traverses various
   * cells.
   * It's only possible to cancel a move before any cells have been
   * traversed from BeginMove.
   * LeaveCell is raised too late (after checking MovementCancelled).
   * Note that EndMove will be called even when a move is cancelled,
   * and that MovementCancelled will be `true` during the call to the
   * handler.
   */
  public void MoveFollowingPath(IEnumerable<HexCell> path, float moveSpeed = DoMoveFollowingPath.DEFAULT_MOVE_SPEED) {
    if(null != movement) {
      Debug.LogError(name + " tried to start moving when already moving!");
      return;
    }
    
    movement = StartCoroutine(
      Command<object>.Action(
        () => {
          if(null != BeginMove)
            BeginMove(this);
        })
      .Then(_ => new DoMoveFollowingPath(this, path, moveSpeed))
      .ThenAction(
        _ => {
          if(null != EndMove)
            EndMove(this);
          movement = null;
          MovementCancelled = false;
        })
      .GetCoroutine());
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
        if(self.MovementCancelled) {
          Debug.Log("Movement cancelled!");
          yield break;
        }

        Debug.Log("Moving to next position: " + cell.coordinates.ToString());

        // raise an event saying that we're leaving the current cell.
        if(null != self.LeaveCell) {
          self.LeaveCell(self);
        }
        Vector3 target = cell.coordinates.ToPosition();

        if(null != self.ChangeDirection) {
          var d = self.CurrentCell.coordinates.WhichNeighbour(cell.coordinates);
          Debug.Assert(d.HasValue, "cell emitted by pathfinding is a neighbour of the current cell");
          self.ChangeDirection(self, d.Value);
        }

        var move = new MoveTransform(self.transform, target, moveSpeed);
        foreach(var o in new TrivialEnumerable(move.GetCoroutine())) {
          yield return null;
        }

        self.CurrentCell = cell;
        if(null != self.EnterCell) {
          self.EnterCell(self);
        }
      }
    }
  }
}
