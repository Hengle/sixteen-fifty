using System;

public enum FadeDirection { IN, OUT }
  
public static class FadeDirectionExt {

  /**
   * Returns the value to being the fade at and the multiplier apply to the alpha-delta.
   */
  public static Tuple<float, int> GetInitialValueAndMultiplier(this FadeDirection self) {
    switch(self) {
    case FadeDirection.IN:
      return Tuple.Create(0f, 1);
    case FadeDirection.OUT:
      return Tuple.Create(1f, -1);
    default:
      throw new Exception("FadeDirection exhaustively analyzed.");
    }
  }
}
