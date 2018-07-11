using System;
using System.Collections.Generic;

using UnityEngine;

namespace SixteenFifty.Quests {
  using Variables;
  
  [CreateAssetMenu(menuName = "1650/Quests/Quest")]
  public class Quest : ScriptableObject {
    new public string name;
    // progress is an *index* to the current goal.
    public IntVariable progress;
    public List<Goal> goals;

    public bool IsComplete => progress.Value == goals.Count;

    /**
     * \brief
     * Proxies the Changed event of the progress variable.
     */
    public event Action<Quest> ProgressChanged;

    /**
     * \brief
     * Raised when progress changes to become greater than the goal
     * count.
     */
    public event Action<Quest> Completed;

    void OnEnable() {
      if(progress != null)
        progress.Changed += OnProgressChanged;
    }

    void OnDisable() {
      if(progress != null)
        progress.Changed -= OnProgressChanged;
    }

    void OnProgressChanged(int n) {
      // we hold the progress variable, so we don't actually care
      // about `n`.
      ProgressChanged?.Invoke(this);

      if(IsComplete)
        Completed?.Invoke(this);
    }
  }

  [Serializable]
  public class Goal {
    public string description;
  }
}
