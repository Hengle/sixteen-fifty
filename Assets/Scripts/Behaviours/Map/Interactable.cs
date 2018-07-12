using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

namespace SixteenFifty.Behaviours {
  /**
   * Things that the player character can interact with in the map.
   */
  public class Interactable : MonoBehaviour {
    public Interaction[] interactions;

    /**
     * \brief
     * Specifies what radius the player must be within in order to
     * interact with this object.
     */
    public float interactionRadius = 1;

    /**
     * \brief
     * Decides whether the given position is within range of this
     * Interactable.
     *
     * \returns
     * Whether the position is close enough.
     * Through the out parameter `sqrDistance`, returns the squared
     * distance to this Interactable.
     */
    public bool IsInRangeOf(Vector2 position, out float sqrDistance) =>
      interactionRadius >
      (sqrDistance = (position - transform.position.Downgrade()).sqrMagnitude);

    public event Action<Interactable> Clicked;

    void OnMouseDown() {
      Clicked?.Invoke(this);
    }

    /**
     * \brief
     * Whether this interactable is focused.
     *
     * This is set externally by InteractableFocus.
     */
    public bool focused = false;

    /**
     * \brief
     * The map in which the interactable exists.
     */
    [SerializeField] [HideInInspector]
    IMap map;

    void Awake() {
      map = GetComponentInParent(typeof(IMap)) as IMap;
      Debug.Assert(
        null != map,
        "Interactable is instantiated in a map.");
    }

    void Start() {
      map.AddInteractable(this);
    }
  }
}
