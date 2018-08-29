using System;

namespace SixteenFifty {
  public static class IEquatableExt {
    /**
    * \brief
    * A helper method for implementing `IEquatable`.
    *
    * Suppose we have a type hierarchy `S : T`.
    * We have implemented `IEquatable<S>.Equals`, so we can check
    * equality of `S` with other objects of type `S`.
    * Now we wish to implement `IEquatable<T>` *for `S`*.
    * The standard way to accomplish this is to attempt casting the
    * target value of type `T` to type `S`, and then use the equality
    * check for type `S`.
    *
    * This generic extension method accomplishes just that!
    */
    public static bool Equals<S, T>(this IEquatable<S> self, T _that) where S : class, T {
      var that = _that as S;
      return null != that && self.Equals(that);
    }
  }
}
