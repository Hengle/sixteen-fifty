using System;
using System.Collections.Generic;

using UnityEngine;

using SixteenFifty.Serialization;

namespace SixteenFifty {
  using Commands;

  [CreateAssetMenu(menuName = "1650/Scripted Event")]
  public class ScriptedEvent : SerializableScriptableObject {
    new public string name;
    public IScript root;
  }

  public interface IScript {
    Command<object> GetScript(EventRunner runner);
  }

  /**
   * \brief
   * A script that's made up of a predetermined sequence of other scripts.
   */
  public class SequenceScript : IScript {
    public List<IScript> items = new List<IScript>();

    public Command<object> GetScript(EventRunner runner) {
      var cmd = Command<object>.Empty;
      foreach(var s in items) {
        cmd = cmd.Then(_ => s.GetScript(runner));
      }
      return cmd;
    }
  }

  /**
   * \brief
   * An immediate script just executes a function.
   *
   * Use this for event items that don't take place over time.
   */
  [Serializable]
  public abstract class ImmediateScript : IScript {
    public Command<object> GetScript(EventRunner runner) {
      return
        Command<EventRunner>.Pure(() => runner)
        .ThenAction(Call);
    }

    public abstract void Call(EventRunner runner);
  }

  [Serializable]
  [EventAttribute(friendlyName = "Play Dialogue")]
  public class PlayDialogue : IScript {
    public Command<object> GetScript(EventRunner runner) {
      throw new SixteenFiftyException("not implemented");
    }
  }

  [EventAttribute(friendlyName = "Give Item")]
  public class GiveItem : ImmediateScript {
    public Item item;
    public int count;

    public override void Call(EventRunner runner) {
      runner.Player.inventory.AddItem(item, count);
      Debug.Log("Gave item.");
    }
  }
}
