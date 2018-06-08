using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

/**
 * \brief
 * The value of a stat.
 *
 * Essentially, this is just a float with the ability to be rendered
 * to string for the purpose of the game.
 * (See #NiceName and StatValueFormatter.)
 */
[Serializable]
public class StatValue {
  [SerializeField]
  public float value;

  [SerializeField]
  public StatValueFormatter statFormatter;

  /**
   * \brief
   * Gets an uninitialized StatValue object whose #value is zero and
   * whose #statFormatter is `null`.
   */
  public static StatValue Uninitialized => new StatValue(null);

  /**
   * \brief
   * The internal value of the equipment stat value.
   */
  public float FloatValue {
    get {
      return value;
    }
    private set {
      this.value = value;
    }
  }

  /**
   * \brief
   * Gets the human "nice" name for the current stat value.
   *
   * For example, warmth stat in `[2f, 3f)` might be called "very
   * warm".
   */
  public string NiceName => statFormatter.FormatStatValue(value);

  /**
   * \brief
   * Constructor for a zero stat value.
   */
  public StatValue(StatValueFormatter fmt) {
    this.value = 0f;
    this.statFormatter = fmt;
  }

  /**
   * \brief
   * Constructor for a specified stat value.
   */
  public StatValue(float value, StatValueFormatter fmt) {
    this.value = value;
    this.statFormatter = fmt;
  }

  /**
   * \brief
   * Adds two StatValue objects.
   *
   * This addition produces a new StatValue object whose
   * internal value is the sum of the input values.
   * The input StatValue objects must have equal
   * StatValueFormatters.
   * Otherwise, this method throws `InvalidStatValue`.
   */
  public static StatValue operator+(StatValue x, StatValue y) {
    if(x.statFormatter != y.statFormatter)
      throw new InvalidStatValue("Stat value formatters do not agree.");
    return new StatValue(x.value + y.value, x.statFormatter);
  }
}

/**
 * \brief
 * Raised when an equipment stat value is invalid.
 */
public class InvalidStatValue : SixteenFiftyException {
  public InvalidStatValue() : base() {
  }
  public InvalidStatValue(string message) : base(message) {
  }
}

/**
 * \brief
 * A type of stat.
 *
 * Each stat in the game has a representation as a class implementing
 * IStat, which establishes what its stat value type is and its name.
 *
 * These are general stat types used for both character stats
 * (e.g. health, warmth, etc.) and equipment stats (e.g. protection,
 * warmth).
 *
 * The difference is essentially in how they are computed:
 * - Equipment stats are unchanging for a given piece of equipment;
 * - Character stats are computed based on environmental factors,
 *   character skill levels, and current character equipment.
 */
public class Stat : ScriptableObject {
  /**
   * \brief The name of the stat.
   */
  new public string name;

  /**
   * \brief
   * The zero value of the stat.
   */
  public StatValue zero;
}

public static class StatValueIEnumerableExt {
  public static StatValue Sum(this IEnumerable<StatValue> source) {
    var s = source.ToList();
    if(s.Count == 0)
      throw new EmptyStatValueEnumerableException();
    var r = s[0];
    for(int i = 1; i < s.Count; i++) {
      r += s[i];
    }
    return r;
  }
}

public class EmptyStatValueEnumerableException : SixteenFiftyException {
  public EmptyStatValueEnumerableException() : base() {
  }
  public EmptyStatValueEnumerableException(string message) : base(message) {
  }
}
