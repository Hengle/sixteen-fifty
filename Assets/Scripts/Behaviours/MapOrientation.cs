using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A behaviour for orientable sprites.
 * Automatically hooks up to a MapEntity component, if present, and
 * watches the ChangeDirection event.
 */
[RequireComponent(typeof(SpriteRenderer))]
public class MapOrientation : MonoBehaviour {
  private HexDirection orientation;

  public HexMapEntity hexMapEntity;

  public HexDirection Orientation {
    get { return orientation; }
    set {
      orientation = value;
      if(null != renderer)
        renderer.sprite = hexMapEntity[orientation];
    }
  }

  [SerializeField]
  private SpriteRenderer renderer;

  [SerializeField]
  private bool registered = false;

  [SerializeField]
  private MapEntity mapEntity;

  void Awake() {
    renderer = this.GetComponentNotNull<SpriteRenderer>();
    // to set the sprite
    Orientation = Orientation;
  }

  void OnEnable() {
    mapEntity = GetComponent<MapEntity>();
    if(null != mapEntity) {
      mapEntity.ChangeDirection += OnChangeDirection;
    }
  }

  void OnDisable() {
    if(null != mapEntity)
      return;
    mapEntity.ChangeDirection -= OnChangeDirection;
  }

  void OnChangeDirection(MapEntity me, HexDirection d) {
    Orientation = d;
  }
}
