using System;

using UnityEngine;

namespace SixteenFifty.Behaviours {
  using TileMap;
  using Variables;
  
  [RequireComponent(typeof(MapEntity))]
  public class InitialPosition : MonoBehaviour, IPositioner {
    public HexCoordinatesVariable destination;

    [SerializeField] [HideInInspector]
    private HexGridManager hexGridManager;

    [SerializeField] [HideInInspector]
    private MapEntity mapEntity;

    public event Action Positioned;
    
    public void Awake() {
      hexGridManager = GetComponentInParent<HexGridManager>();
      mapEntity = GetComponent<MapEntity>();

      Debug.Assert(
        null != hexGridManager,
        "InitialPosition exists within a map context.");
      Debug.Assert(
        null != mapEntity,
        "InitialPosition is attached with a MapEntity.");
    }

    public void OnEnable() {
      hexGridManager.MapReady += OnMapReady;
    }

    public void OnDisable() {
      hexGridManager.MapReady -= OnMapReady;
    }

    void OnMapReady(IMap map) {
      if(null != destination) {
        mapEntity.Warp(destination.Value);
        Positioned?.Invoke();
      }
    }
  }
}
