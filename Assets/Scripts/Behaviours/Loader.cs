using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour {
  public GameObject stateManagerPrefab;
  public string initialScene;

	void Awake () {
    // the loader first initializes the state manager
    if(StateManager.Instance == null)
      Instantiate(stateManagerPrefab);

    // then, the loader changes scenes to the initial scene.
    SceneManager.LoadScene(initialScene);
	}
}
