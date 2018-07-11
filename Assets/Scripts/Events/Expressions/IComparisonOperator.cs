using System;

namespace SixteenFifty.EventItems.Expressions {
  public interface IComparisonOperator<T> {
    bool Compare(T left, T right);
  }

  [Serializable]
  [SelectableSubtype(friendlyName = ">=")]
  public class GEQ : IComparisonOperator<int> {
    public bool Compare(int left, int right) => left >= right;
  }

  [Serializable]
  [SelectableSubtype(friendlyName = "==")]
  public class EQ : IComparisonOperator<int> {
    public bool Compare(int left, int right) => left == right;
  }

  [Serializable]
  [SelectableSubtype(friendlyName = "<=")]
  public class LEQ : IComparisonOperator<int> {
    public bool Compare(int left, int right) => left <= right;
  }

  [Serializable]
  [SelectableSubtype(friendlyName = ">")]
  public class GT : IComparisonOperator<int> {
    public bool Compare(int left, int right) => left > right;
  }

  [Serializable]
  [SelectableSubtype(friendlyName = "<")]
  public class LT : IComparisonOperator<int> {
    public bool Compare(int left, int right) => left < right;
  }

  [Serializable]
  [SelectableSubtype(friendlyName = "/=")]
  public class NEQ : IComparisonOperator<int> {
    public bool Compare(int left, int right) => left != right;
  }
}
