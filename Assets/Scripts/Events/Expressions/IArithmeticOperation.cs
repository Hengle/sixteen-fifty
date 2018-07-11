using System;

namespace SixteenFifty.EventItems.Expressions {
  public interface IArithmeticOperation {
    int Compute(int x, int y);
  }

  [Serializable]
  [SelectableSubtype(friendlyName = "Addition")]
  public class Addition : IArithmeticOperation {
    public int Compute(int x, int y) => x + y;
  }
}
