using System;

using UnityEngine;

namespace SixteenFifty.Behaviours {
  /**
   * \brief
   * Causes the camera to follow the object with this component.
   */
  public class CameraFollows : MonoBehaviour {
    [SerializeField] [HideInInspector]
    Camera camera;
    
    void Awake() {
      camera = Camera.main;
    }

    void Update() {
      var p = transform.position;
      var q = camera.transform.position;
      q.x = p.x;
      q.y = p.y;
      camera.transform.position = q;
    }
  }
}
