using System;
using System.Linq;
using System.Collections.Generic;

public static class KeyValuePairExt {
  /**
   * \brief
   * Clones the `KeyValuePair<K, V>` but with a different key.
   */
  public static KeyValuePair<K, V> ButWithKey<K, V>(this KeyValuePair<K, V> self, K newKey) {
    return new KeyValuePair<K, V>(newKey, self.Value);
  }

  /**
   * \brief
   * Clones the `KeyValuePair<K, V>` but with a different value.
   */
  public static KeyValuePair<K, V> ButWithValue<K, V>(this KeyValuePair<K, V> self, V newValue) {
    return new KeyValuePair<K, V>(self.Key, newValue);
  }
}
