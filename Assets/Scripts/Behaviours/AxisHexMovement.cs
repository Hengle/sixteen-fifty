using System.Linq;

using UnityEngine;

namespace SixteenFifty.Behaviours {
  [RequireComponent(typeof(MapEntity))]
  public class AxisHexMovement : MonoBehaviour {
    public float moveSpeed = 1;
    public float progressSpeed = 1;

    [Range(0f, 1f)]
    public float progress = 0;
  
    bool Active => destination == null && map != null && !manager.eventManager.IsUI;

    [SerializeField] [HideInInspector]
    MapEntity mapEntity;

    [SerializeField] [HideInInspector]
    HexGridManager manager;

    [SerializeField] [HideInInspector]
    HexGrid map;

    [SerializeField] [HideInInspector]
    HexCell destination;

    [SerializeField] [HideInInspector]
    HexDirection lastDirection;

    void Awake() {
      mapEntity = GetComponent<MapEntity>();
      Debug.Assert(
        null != mapEntity,
        "AxisMovement is attached with a MapEntity.");
      
      manager = GetComponentInParent<HexGridManager>();
      Debug.Assert(
        null != manager,
        "AxisMovement is under a HexGridManager.");
    }

    void OnEnable() {
      manager.MapReady += OnMapReady;
      mapEntity.EndMove += OnEndMove;
    }

    void OnDisable() {
      manager.MapReady -= OnMapReady;
      mapEntity.EndMove -= OnEndMove;
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

    void Update() {
      if(!Active)
        return;
      
      var v = InputUtility.PrimaryAxis;
      if(v.sqrMagnitude < 0.01) {
        // stopped holding the stick
        progress = 0;
        return;
      }

      // you might wonder why we don't make AxisHexMovement implement
      // INotifyDirectionChange and have it raise the relevant event
      // here.
      // The reason is simple: MapEntity already does that for us.
      // We use MapEntity.MoveFollowingPath (for single-cell paths,
      // which is somewhat of a degenerate case) and this method
      // automatically detects direction changes while moving and
      // fires DirectionChanged.
      var theta = Mathf.Atan2(v.y, v.x);
      if(theta < 0)
        theta += 2 * Mathf.PI;
      var d = TileMap.HexMetrics.DirectionFromAngle(theta);
      if(d != lastDirection) {
        progress = 0;
        lastDirection = d;
        return;
      }

      // progress increased past 100%
      if(ProgressUp()) {
        // - figure out what the destination is.
        // - if it's a nonempty cell, then we try to interact with it.
        // - otherwise, move there

        var destinationCoords =
          mapEntity.CurrentCell.coordinates[lastDirection];
        destination = map[destinationCoords];
        if(destination != null) {
          if(destination.IsNonEmpty) {
            var interactions = destination.Interactions.ToArray();
            if(interactions.Length > 0)
              manager.PresentInteractionsMenu(interactions);
            destination = null;
          }
          else
            mapEntity.MoveFollowingPath(new [] { destination });
        }
      }
    }
  }
}
