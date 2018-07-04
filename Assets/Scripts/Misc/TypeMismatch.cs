using System;

namespace SixteenFifty.Reflection {
  public class TypeMismatch : SixteenFiftyException {
    public static string Format(Type actual, Type expected) =>
      String.Format(
        "Type mismatch: expected type '{0}'; actual type is '{1}'",
        expected,
        actual);

    public Type actual;
    public Type expected;

    public TypeMismatch(Type actual, Type expected) : base(Format(actual, expected)) {
      this.actual = actual;
      this.expected = expected;
    }
  }
}
