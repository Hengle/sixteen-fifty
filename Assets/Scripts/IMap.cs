using System;

using UnityEngine;

namespace SixteenFifty.Behaviours {
  /**
   * \brief
   * Common behaviour implemented by all map types.
   */
  public interface IMap {
    /**
     * \brief
     * Loads the map into the HexGridManager.
     */
    void Load(HexGridManager manager, BasicMap map);

    /**
     * \brief
     * Gets the map represented by this controller.
     */
    BasicMap Map { get; }

    /**
     * \brief
     * Gets the controller for the player in the map.
     */
    PlayerController Player { get; }

    /**
     * \brief
     * Spawns a player character in the map and returns its PlayerController.
     */
    PlayerController SpawnPlayer();

    /**
     * \brief
     * Fires when the map has finished all initialization.
     *
     * Typically, MapEntity objects use this event to perform their
     * initialization once the map is completely ready.
     */
    event Action<IMap> Ready;
  }
}
