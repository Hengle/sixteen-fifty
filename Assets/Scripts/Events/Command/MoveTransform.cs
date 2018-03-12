using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commands {
  /**
   * Moves a transform to a given destination using a speed.
   */
  public class MoveTransform : Command<object> {
    // in units per *second*
    public const float DEFAULT_SPEED = 1f;
    
    Vector3 destination;
    Transform target;
    float panSpeed;
  
    public MoveTransform(Transform target, Vector3 destination, float panSpeed = DEFAULT_SPEED) {
      this.destination = destination;
      this.target = target;
      this.panSpeed = panSpeed;
    }
  
    public override IEnumerator GetCoroutine() {
      float remaining;
      do {
        remaining = (destination - target.position).sqrMagnitude;
        target.position = Vector3.MoveTowards(target.position, destination, panSpeed);
        yield return null;
      }
      while(remaining > float.Epsilon);
    }
  }
}
