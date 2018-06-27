using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SixteenFifty {
  using TileMap;
  
  /**
  * Singleton class used to store runtime-persistent state.
  */
  public class StateManager : MonoBehaviour {
    private static StateManager instance = null;
    public static StateManager Instance => instance;

    public GameObject selfPrefab;

    public EventManager eventManager;
    public PlayerController playerController;
    public HexGridManager hexGridManager;

    private StateManager() {

    }

    void Awake() {
      if(instance == null) {
        instance = this;
      }
      else if(instance != this) {
        // if there is an existing instance, then we kill ourselves.
        Destroy(gameObject);
        Debug.LogWarning("Something tried to create another StateManager.");
        return;
      }

      // Normally, when changing scenes in Unity, all objects in the
      // hierarchy are destroyed. By marking ourself as
      // DontDestroyOnLoad, we can persist the state manager even
      // through scene changes.
      DontDestroyOnLoad(gameObject);

      Debug.Log("Initialized StateManager.");
    }

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {

    }
  }
}
