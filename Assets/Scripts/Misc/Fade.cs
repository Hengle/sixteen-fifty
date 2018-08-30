using System;

public enum FadeDirection { IN, OUT }
  
public static class FadeDirectionExt {
  /**
   * \brief
   * Eliminates a FadeDirection into another type by supplying
   * functions to compute a result for each case.
   */
  public static T Eliminate<T>(this FadeDirection self, Func<T> ifIn, Func<T> ifOut) {
    switch(self) {
    case FadeDirection.IN:
      return ifIn();
    case FadeDirection.OUT:
      return ifOut();
    }
    throw new AnalysisExhaustedException("FadeDirection");
  }

  /**
   * \brief
   * Gets the opposite of this direction.
   */
  public static FadeDirection Inverse(this FadeDirection self) =>
    self.Eliminate(
      () => FadeDirection.OUT,
      () => FadeDirection.IN);

  /**
   * \brief
   * Gets a tuple containing the initial value for fading and a
   * multiplier for the direction to fade.
   */
  public static Tuple<float, int> GetInitialValueAndMultiplier(this FadeDirection self) =>
    self.Eliminate(
      () => Tuple.Create(0f, 1),
      () => Tuple.Create(1f, -1));
}
