using System.Collections.Generic;

/**
 * \brief
 * Extends IEnumerable<T> with the #Numbering method, which numbers
 * each element of the sequence.
 */
public static class NumberingExt {
  /**
   * \brief
   * Numbers each element of the sequence starting from a given value
   * and increasing by a given increment.
   */
  public static IEnumerable<Numbered<T>> Numbering<T>(
    this IEnumerable<T> sequence,
    int start = 0,
    int by = 1) {

    int i = start;
    foreach(var t in sequence) {
      yield return new Numbered<T>(i, t);
      i += by;
    }
  }
}

public class Numbered<T> {
  public readonly int number;
  public readonly T value;

  public Numbered(int number, T value) {
    this.value = value;
    this.number = number;
  }
}
