using UnityEngine;

namespace SixteenFifty {
  public class BasicMap : ScriptableObject {
    /**
     * \brief
     * The prefab used to construct this map.
     */
    public GameObject prefab;
    
    /**
     * \brief
     * Specifications of all the NPCs in the map.
     */
    public BasicNPC[] npcs;
  }
}
