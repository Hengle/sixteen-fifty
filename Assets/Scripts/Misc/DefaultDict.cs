using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

/**
 * \brief
 * A dictionary whose indexer returns default elements when a key is
 * missing.
 *
 * (This is modelled after Python's `defaultdict` class.)
 * When constructing an object of this type, a `Func<K, V>` is passed
 * in to generate a default value (of type `V`) for a given key (of
 * type `K`).
 *
 * Optionally, the class can "write back" the default into the
 * underlying collection, which can be useful when dealing with
 * mutable data structures.
 * For example, suppose `V` is some mutable reference type and `foo`
 * is `DefaultDictionary<K, V>`. Let `k` have type `K` and suppose
 * `foo` does not contain the key `k`.
 * Then, `foo[k].SetFoo(true)` only really makes sense if `foo` (after
 * generating the default value for `k`) writes that default back into
 * the dictionary. Otherwise, the newly-created object produced by
 * `foo[k]` is lost.
 */
public class DefaultDictionary<K, V> : IDictionary<K, V> {
  Func<K, V> makeDefault;
  bool writeBack;
  IDictionary<K, V> backing;

  /**
   * \brief Basic constructor.
   *
   * Creates a dictionary that will run `makeDefault` when retrying to
   * retrieve values for keys that don't exist.
   * The `writeBack` flag controls whether the generated defaults are
   * written back into the dictionary after being generated.
   */
  public DefaultDictionary(Func<K, V> makeDefault, bool writeBack = true) {
    this.makeDefault = makeDefault;
    this.writeBack = writeBack;
    this.backing = new Dictionary<K, V>();
  }

  /**
   * \brief
   * Constructor for choosing a custom backing `IDictionary<K, V>`.
   */
  public DefaultDictionary(IDictionary<K, V> backing, Func<K, V> makeDefault, bool writeBack = true) {
    this.makeDefault = makeDefault;
    this.writeBack = writeBack;
    this.backing = backing;
  }

  public V this[K k] {
    get {
      if(backing.ContainsKey(k)) {
        return backing[k];
      }
      else {
        var v = makeDefault(k);
        if(writeBack) {
          backing[k] = v;
        }
        return v;
      }
    }

    set {
      backing[k] = value;
    }
  }

  public int Count => backing.Count;
  public bool IsReadOnly => backing.IsReadOnly;

  public ICollection<K> Keys => backing.Keys;
  public ICollection<V> Values => backing.Values;

  public void Add(KeyValuePair<K, V> item) {
    this[item.Key] = item.Value;
  }

  public void Add(K key, V value) {
    this[key] = value;
  }

  public void SetWriteBack(bool writeBack) {
    this.writeBack = writeBack;
  }

  public void Clear() {
    backing.Clear();
  }

  public bool Contains(KeyValuePair<K, V> item) {
    return backing.Contains(item);
  }

  public bool ContainsKey(K key) {
    return backing.ContainsKey(key);
  }

  public void CopyTo(KeyValuePair<K, V>[] array, Int32 start) {
    backing.CopyTo(array, start);
  }

  public IEnumerator<KeyValuePair<K, V>> GetEnumerator() {
    return backing.GetEnumerator();
  }

  IEnumerator IEnumerable.GetEnumerator() {
    return backing.GetEnumerator();
  }

  public bool Remove(KeyValuePair<K, V> item) {
    return backing.Remove(item);
  }

  public bool Remove(K key) {
    return backing.Remove(key);
  }

  public bool TryGetValue(K key, out V value) {
    return backing.TryGetValue(key, out value);
  }
}
