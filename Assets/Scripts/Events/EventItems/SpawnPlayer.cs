using System;

namespace SixteenFifty.EventItems {
  [Serializable]
  [NoEditorNeeded]
  [SelectableSubtype(friendlyName = "Spawn Player")]
  public class SpawnPlayer : ImmediateScript, IEquatable<SpawnPlayer> {
    public override void Call(EventRunner runner) {
      runner.GridManager.SpawnPlayer();
    }

    public bool Equals(SpawnPlayer that) => true;

    public override bool Equals(IScript that) =>
      IEquatableExt.Equals(this, that);
  }
}
