using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Commands {
  public class FadeInSpeaker : Command<Speaker> {

    public FadeInSpeaker() {
    }
    
    public override IEnumerator GetCoroutine() {
      throw new System.NotImplementedException("FadeInSpeaker.GetCoroutine()");
    }
  }
}
