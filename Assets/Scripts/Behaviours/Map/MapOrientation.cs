using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace SixteenFifty.Behaviours {
  using Serialization;
  using TileMap;
  
  /**
  * \brief
  * Renders a HexMapEntity in the correct orientation.
  * This component connects a HexMapEntity to a MapEntity.
  */
  [RequireComponent(typeof(SpriteRenderer))]
  public class MapOrientation : SerializableBehaviour {
    private HexDirection orientation;

    public HexMapEntity hexMapEntity;

    /**
     * \brief
     * Gets or sets #hexMapEntity.
     *
     * Using this setter will update the sprite being rendered in the
     * correct orientation.
     */
    public HexMapEntity HexMapEntity {
      get {
        return hexMapEntity;
      }
      set {
        hexMapEntity = value;
        // cause an orientation update when the underlying map entity
        // changes.
        UpdateSprite();
      }
    }

    /**
    * \brief
    * The current orientation of the MapEntity.
    *
    * Setting this property changes which Sprite the SpriteRenderer
    * will draw.
    */
    public HexDirection Orientation {
      get { return orientation; }
      set {
        orientation = value;
        if(null != renderer)
          renderer.sprite = hexMapEntity[orientation];
      }
    }

    /**
    * \brief
    * The renderer to use to draw the HexMapEntity's Sprites.
    *
    * Initialized in ::Awake.
    */
    [SerializeField] [HideInInspector]
    new private SpriteRenderer renderer;

    [SerializeField]
    INotifyDirectionChange entity;

    public void UpdateSprite() {
      Orientation = Orientation;
    }

    void Awake() {
      renderer = GetComponent<SpriteRenderer>();
      Debug.Assert(
        null != renderer,
        "MapOrientation is attached with a SpriteRenderer.");

      entity =
        GetComponent(typeof(INotifyDirectionChange))
        as INotifyDirectionChange;
      Debug.Assert(
        null != entity,
        "MapOrientation is attached to an INotifyDirectionChange component.");
    }

    void Start() {
      if(null != hexMapEntity)
        UpdateSprite();
    }

    void OnEnable() {
      if(null != entity)
        entity.DirectionChanged += OnDirectionChanged;
    }

    void OnDisable() {
      if(null != entity)
        entity.DirectionChanged -= OnDirectionChanged;
    }

    /**
    * \brief
    * Handles ::mapEntity::ChangeDirection.
    */
    void OnDirectionChanged(HexDirection d) {
      Orientation = d;
    }
  }
}
