using System;
using System.Linq;

using UnityEngine;

namespace SixteenFifty.Behaviours {
  using Serialization;
  
  [RequireComponent(typeof(PlayerHexInput))]
  [RequireComponent(typeof(MapEntity))]
  public class AxisHexMovement : SerializableBehaviour {
    public float moveSpeed = 1;
    public float progressSpeed = 1;

    [Range(0f, 1f)]
    public float progress = 0;
  
    [SerializeField] [HideInInspector]
    HexGridManager manager;

    [SerializeField] [HideInInspector]
    MapEntity mapEntity;

    [SerializeField] [HideInInspector]
    HexGrid map;

    [SerializeField] [HideInInspector]
    HexCell destination;

    [SerializeField] [HideInInspector]
    HexDirection direction;

    [SerializeField] [HideInInspector]
    bool hasDirection;

    [SerializeField] [HideInInspector]
    IHexInput hexInput;

    void Awake() {
      mapEntity = GetComponent<MapEntity>();
      Debug.Assert(
        null != mapEntity,
        "AxisHexMovement is attached with a MapEntity.");

      manager = GetComponentInParent<HexGridManager>();
      Debug.Assert(
        null != manager,
        "AxisHexMovement is under a HexGridManager.");

      hexInput = GetComponent(typeof(IHexInput)) as IHexInput;
      Debug.Assert(
        null != hexInput,
        "AxisHexMovement is attached with a IHexInput.");
    }

    void OnEnable() {
      manager.MapReady += OnMapReady;
      mapEntity.EndMove += OnEndMove;
      hexInput.DirectionChanged += OnDirectionChanged;
      hexInput.SubmitPressed += OnSubmitPressed;
    }

    void OnDisable() {
      manager.MapReady -= OnMapReady;
      mapEntity.EndMove -= OnEndMove;
      hexInput.DirectionChanged -= OnDirectionChanged;
      hexInput.SubmitPressed -= OnSubmitPressed;
    }

    void OnMapReady(IMap map) {
      this.map = map as HexGrid;
    }

    void OnEndMove(MapEntity mapEntity) {
      destination = null;
    }

    bool ProgressUp() {
      progress += progressSpeed * Time.deltaTime;
      if(progress > 1) {
        progress = 0;
        return true;
      }
      return false;
    }

    void OnDirectionChanged(Maybe<HexDirection> md) {
      md.Eliminate(
        () => hasDirection = false,
        d => { direction = d; return hasDirection = true; } );
    }

    void OnSubmitPressed() {
      Debug.Log("hi");
      var interactions = mapEntity.CurrentCell.TileInteractions.ToArray();
      if(interactions.Length > 0)
        manager.PresentInteractionsMenu(interactions);
    }

    void Update() {
      if(!hasDirection) {
        progress = 0;
        return;
      }

      // progress increased past 100%
      if(ProgressUp()) {
        // - figure out what the destination is.
        // - if it's a nonempty cell, then we try to interact with it.
        // - otherwise, move there

        var destinationCoords =
          mapEntity.CurrentCell.coordinates[direction];
        destination = map[destinationCoords];
        if(destination != null) {
          if(destination.IsNonEmpty) {
            var interactions = destination.EntityInteractions.ToArray();
            if(interactions.Length > 0)
              manager.PresentInteractionsMenu(interactions);
            destination = null;
          }
          else
            mapEntity.MoveFollowingPath(
              new [] { destination },
              moveSpeed: moveSpeed);
        }
      }
    }
  }
}
