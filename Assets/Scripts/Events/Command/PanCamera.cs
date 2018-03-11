using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commands {
  /**
   * Pans the camera to the given position.
   */
  public class PanCamera : Command<object> {
    public const float DEFAULT_TIME = 0.35f;
    
    Vector2 position;
  
    public PanCamera(Vector2 position) {
      this.position = position;
    }
  
    public override IEnumerator GetCoroutine() {
      throw new NotImplementedException("PanCamera.GetCoroutine()");
    }
  }
}
