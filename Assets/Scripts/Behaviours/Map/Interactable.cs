using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SixteenFifty.Behaviours {
  /**
   * Things that the player character can interact with in the map.
   */
  public class Interactable : MonoBehaviour {
    public Interaction[] interactions;

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
      map.Interactables.Add(this);
    }
  }
}
