using System;

namespace SixteenFifty.EventItems {
  [Serializable]
  [SelectableSubtype(friendlyName = "Spawn Player")]
  public class SpawnPlayer : ImmediateScript {
    public override void Call(EventRunner runner) {
      runner.GridManager.SpawnPlayer();
    }
  }
}