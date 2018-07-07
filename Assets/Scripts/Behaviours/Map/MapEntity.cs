using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace SixteenFifty.Behaviours {
  using Commands;
  using TileMap;

  /**
  * \brief
  * A behaviour for things that exist within a HexGrid at specific
  * coordinates and can move around.
  *
  * This class only handles *positions*, and raises appropriate events
  * when the entity enters / leaves a cell, changes orientations while
  * moving, etc.
  *
  * In order to render a MapEntity, define a HexMapEntity asset and
  * connect the MapEntity with the HexMapEntity using the
  * MapOrientation behaviour.
  */
  public class MapEntity : MonoBehaviour {
    /**
    * \brief
    * How fast the player moves, in units per seconds.
    */
    public const float MOVE_SPEED = 6f;

    private HexCell currentCell;
    /**
    * \brief
    * The cell the entity is currently occupying.

    * While the entity is in motion, this value becomes stale until the
    * entity enters at a new cell.
    * Every time the entity passes through a cell, this value gets
    * updated.
    */
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

    /**
    * \brief
    * Raised when the MapEntity leaves a cell on the HexGrid.
    */
    public event Action<MapEntity> LeaveCell;

    /**
    * \brief
    * Raised when the MapEntity enters a cell on the HexGrid.
    */
    public event Action<MapEntity> EnterCell;

    /**
    * \brief
    * Raised when the MapEntity begins to move.
    */
    public event Action<MapEntity> BeginMove;

    /**
    * \brief
    * Raised when the MapEntity ends its move. (i.e. It has arrived at its destination.)
    *
    * Note that when this is fired, #MovementCancelled will be false if
    * the move was cancelled.
    */
    public event Action<MapEntity> EndMove;

    /**
    * \brief
    * Raised during movement whenever there's a change of direction.
    *
    * \sa
    * MapOrientation
    */
    public event Action<MapEntity, HexDirection> ChangeDirection;

    private Coroutine movement;

    /**
    * \brief
    * Is the MapEntity currently in motion?
    */
    public bool IsMoving => movement != null;

    /**
    * \brief
    * Was the players movement cancelled?
    *
    * If the player's movement is cancelled, then the player will stop
    * moving when it arrives at its next intermediate destination,
    * after raising the EnterCell event.
    */
    public bool MovementCancelled {
      get;
      private set;
    }

    /**
    * \brief
    * The grid in which the MapEntity exists.
    *
    * Initialized during #Awake.
    */
    public HexGrid Grid {
      get;
      private set;
    }

    /**
    * \brief
    * Teleports the entity instantly to the given cell.
    *
    * This will correctly adjust #CurrentCell but it does not raise
    * #LeaveCell or #EnterCell.
    */
    public void Warp(HexCell cell) {
      Debug.Assert(
        null != cell,
        "Cell to warp to exists.");
      CurrentCell = cell;
      transform.localPosition = cell.coordinates.Box.Center;
      Debug.LogFormat(
        "Warped {0} to {1}.",
        name,
        cell.coordinates);
    }

    /**
    * \brief
    * Teleports the entity instantly to the cell at the given coordinates.
    */
    public void Warp(HexCoordinates coords) {
      Warp(Grid[coords]);
    }

    /**
    * \brief
    * Cancels the entity's movement.
    *
    * Calling this function is a no-op (but issues a warning) if the
    * entity is not moving.
    *
    * \sa
    * #MovementCancelled
    */
    public void CancelMovement() {
      if(null == movement) {
        Debug.LogWarning("Player movement cancelled, but the player was not moving.");
        return;
      }

      MovementCancelled = true;
    }

    void Awake () {
      Grid = this.GetComponentInParentNotNull<HexGrid>();
    }

    /**
      \brief
      Asynchronously begins moving the entity along the given path.

      - The path's cells should all be sequentially adjacent.
      - The path should not include the cell that the entity is
        currently on.
      - Fires #BeginMove when the move actually starts and fires
        #EndMove when the movement ends.
      - Fires #LeaveCell and #EnterCell as the player traverses
        various cells.
      - Movement can be cancelled from the #BeginMove and #EnterCell
        event handlers.
        (#LeaveCell is raised too late, only _after_ checking
        #MovementCancelled).
      - #EndMove will be raised even when a move is
        cancelled, and that #MovementCancelled will be `true` during the
        call to the handler.
    */
    public void MoveFollowingPath(
      IEnumerable<HexCell> path,
      float moveSpeed = DoMoveFollowingPath.DEFAULT_MOVE_SPEED) {

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
            Debug.Log("movement nulled out");
            MovementCancelled = false;
          })
        .GetCoroutine());

      Debug.Log("movement set!");
    }

    class DoMoveFollowingPath : Command<object> {
      /**
      * \brief
      * The default speed for moving a MapEntity.
      */
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

          // raise an event saying that we're leaving the current cell.
          if(null != self.LeaveCell) {
            self.LeaveCell(self);
          }
          Vector3 target = cell.coordinates.Box.Center;

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
}
