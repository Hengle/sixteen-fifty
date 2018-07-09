using System;

namespace SixteenFifty.EventItems.Expressions {
  public interface IComparisonOperator<T> {
    bool Compare(T left, T right);
  }

  [Serializable]
  public class GEQ : IComparisonOperator<int> {
    public bool Compare(int left, int right) => left >= right;
  }

  [Serializable]
  public class EQ : IComparisonOperator<int> {
    public bool Compare(int left, int right) => left == right;
  }

  [Serializable]
  public class LEQ : IComparisonOperator<int> {
    public bool Compare(int left, int right) => left <= right;
  }

  [Serializable]
  public class GT : IComparisonOperator<int> {
    public bool Compare(int left, int right) => left > right;
  }

  [Serializable]
  public class LT : IComparisonOperator<int> {
    public bool Compare(int left, int right) => left < right;
  }

  [Serializable]
  public class NEQ : IComparisonOperator<int> {
    public bool Compare(int left, int right) => left != right;
  }
}
