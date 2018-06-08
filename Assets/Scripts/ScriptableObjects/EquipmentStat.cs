using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;

/**
 * \brief
 * Dummy interface for equipment stats.
 */
[CreateAssetMenu(menuName = "1650/Equipment Stat")]
public class EquipmentStat : Stat {
}

[Serializable]
public class EquipmentStatDictionary :
SerializableDictionary<EquipmentStat, StatValue> {
  public EquipmentStatDictionary() : base() {
  }
  public EquipmentStatDictionary(IDictionary<EquipmentStat, StatValue> stats) : base(stats) {
  }
}

/**
 * \brief
 * The stats of a piece of equipment.
 */
[Serializable]
public class EquipmentStats {
  /**
   * The dictionary that associates stat types (Stat objects) to stat
   * values (StatValue objects).
   */
  public EquipmentStatDictionary stats;

  public EquipmentStats() {
    stats = new EquipmentStatDictionary();
  }

  public EquipmentStats(EquipmentStatDictionary stats) {
    this.stats = stats;
  }

  /**
   * \brief
   * Adds two EquipmentStats objects pointwise.
   */
  public static EquipmentStats operator+(EquipmentStats self, EquipmentStats that) =>
    new EquipmentStats(
      new EquipmentStatDictionary(
        // combine the underlying dictionaries as enumerables
        self.stats.Concat(that.stats)
        // when both dictionaries share a key, we'll have duplicate keys,
        // which won't make a valid dictionary anymore.
        // idea: group by the key, and then collapse each group using our
        // Sum() extension method
        .GroupBy(k => k.Key).Select(
          grp =>
          new KeyValuePair<EquipmentStat, StatValue>(
            grp.Key, grp.Select(p => p.Value).Sum()))
        .ToDictionary(x => x.Key, x => x.Value)))
    ;
}

public static class EquipmentStatsIEnumerableExt {
  /**
   * \brief
   * Sums a sequence of EquipmentStats objects using their `operator+`.
   */
  public static EquipmentStats Sum(this IEnumerable<EquipmentStats> source) {
    var r = new EquipmentStats(); 
    foreach(var s in source) {
      r += s;
    }
    return r;
  }
}
