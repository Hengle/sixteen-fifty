using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using UnityEngine;

namespace SixteenFifty.Reflection {
  public static class SubtypeReflector {
    /**
    * \brief
    * Enumerables all types in the assembly containing type `A`.
    */
    public static IEnumerable<Type> GetTypes<A>() =>
      typeof(A).Assembly.GetTypes();

    /**
    * \brief
    * Enumerates all subclasses of a given class type `T` that reside
    * in the same assembly as type `A`.
    */
    public static IEnumerable<Type> GetSubtypes<T, A>() where T : class =>
      GetSubtypes<A>(typeof(T));

    /**
    * \brief
    * Enumerates all subclasses of the class `type`.
    *
    * Prefer calling `GetSubclasses<T>` using a type argument instead
    * of this method, since this method does not check that `type`
    * refers to a class type.
    */
    public static IEnumerable<Type> GetSubtypes<A>(Type type) =>
      GetTypes<A>().Where(t => t.IsClass && t.IsSubclassOf(type));

    /**
    * \brief
    * Enumerates all implementations of the interface `T` that reside
    * in the same assembly as `A`.
    */
    public static IEnumerable<Type> GetImplementations<T, A>() where T : class =>
      GetImplementations<A>(typeof(T));

    /**
    * \brief
    * Enumerates all implementations of the interface `type`.
    *
    * Prefer calling `GetImplementations<T>` using a type argument
    * instead of this method, since this method does not check that
    * `type` refers to an interface type.
    */
    public static IEnumerable<Type> GetImplementations<A>(Type type) =>
      GetTypes<A>().Where(t => t.IsClass && type.IsAssignableFrom(t));

    /**
     * \brief
     * Filters an `IEnumerable<Type>` to contain only those types with
     * the given attribute type `T`.
     */
    public static IEnumerable<Type>
    WithAttribute<T>(this IEnumerable<Type> self) where T : Attribute =>
      self.WithAttribute(typeof(T));

    /**
     * \brief
     * Filters an `IEnumerable<Type>` to contain only those types with
     * the given attribute type `T`.
     *
     * It is preferable to use the generic version of this method,
     * since it statically verifies that the attribute type is a
     * subclass of `Attribute`.
     */
    public static IEnumerable<Type> WithAttribute(this IEnumerable<Type> self, Type attrType) =>
      self.Where(
        type =>
        type.CustomAttributes.Any(a => a.AttributeType == attrType));

    /**
    * \brief
    * Finds the `CustomAttributeData` object of the given `attrType`.
    *
    * \returns
    * The `CustomAttributeData` object, if any, or null.
    */
    public static CustomAttributeData FindCustomAttribute<T>(this Type self) =>
      self.FindCustomAttribute(typeof(T));

    /**
    * \brief
    * Finds the `CustomAttributeData` object of the given `attrType`.
    *
    * It is preferable to call `FindCustomAttribute<T>` using generics when possible.
    *
    * \returns
    * The `CustomAttributeData` object, if any, or null.
    */
    public static CustomAttributeData FindCustomAttribute(this Type self, Type attrType) =>
      self.CustomAttributes.Where(a => a.AttributeType == attrType).FirstOrSentinel(null);

    /**
    * \brief
    * Finds the value of the argument with the given name in this `CustomAttributeData`.
    *
    * \exception TypeMismatch
    * The type argument `T` is the *expected* type of the value named `name`.
    * This expectation is checked, and a cast is performed if the real
    * value of the argument is convertible to type `T`. Otherwise, a
    * TypeMismatch exception is thrown.
    *
    * \returns
    * The well-typed value of the named argument, boxed in a Maybe.
    * If there is no such named argument, then `Nothing`.
    *
    * \sa
    * Maybe
    */
    public static Maybe<T> FindNamedArgument<T>(this CustomAttributeData self, string name) where T : class =>
      self.NamedArguments
      .Where(arg => arg.MemberName == name)
      .Select(
        arg => {
          var val = arg.TypedValue.Value;
          if(val is T)
            return Maybe<T>.Just((T)val);
          else
            throw new TypeMismatch(val.GetType(), typeof(T));
        })
      .FirstOrSentinel(Maybe<T>.Nothing());

    /**
     * \brief
     * Invokes the default constructor for the type.
     *
     * This method uses generics to check that the constructed value
     * is assignable to type `T` in order to return a well-typed
     * value, instead of `object`.
     *
     * If you are statically unsure of the type you are instantiating,
     * use the non-generic version of this method, which returns
     * `object`.
     *
     * \exception TypeMismatch
     * If the constructed value is not assignable to type `T`, a
     * TypeMismatch error is raised.
     */
    public static T Construct<T>(this Type t) {
      Debug.Assert(
        null != t,
        "Type to instantiate must be not null.");
      Typecheck(typeof(T), t);
      return (T)t.GetConstructor(Type.EmptyTypes).Invoke(null);
    }

    /**
     * \brief
     * Invoke the single-argument constructor for this type.
     *
     * \remark
     * The argument `obj` must be not null, as its `GetType` method is
     * used to perform method resolution.
     *
     * \exception TypeMismatch
     * This exception is raised if the type `t` is not assignable to type `T`.
     */
    public static T Construct<T>(this Type t, object obj) {
      Typecheck(typeof(T), t);
      return (T)t.GetConstructor(new [] { obj.GetType() }).Invoke(new [] { obj });
    }

    public static T Construct<S, T>(this Type t, S s) {
      Typecheck(typeof(T), t);
      var ctor = t.GetConstructor(new [] { typeof(S) });
      Debug.Assert(
        null != ctor,
        "The constructor exists.");
      return (T)ctor.Invoke(new object[] { s });
    }

    /**
     * \brief
     * Invokes the default constructor for the type.
     *
     * If the type `t` (or a supertype of it) is statically known,
     * then it is preferable to call the generic version of this
     * method, as it returns a well-typed result, instead of `object`.
     */
    public static object DefaultConstruct(this Type t) {
      return t.GetConstructor(Type.EmptyTypes).Invoke(null);
    }

    /**
     * \brief
     * Decides whether the type `src` is assignable to the type `dst`.
     *
     * \param src
     * The source type. This may be `null`, in which case the check
     * will always succeed.
     *
     * \param dst
     * The destination type. This must not be `null`.
     */
    public static void Typecheck(Type dst, Type src) {
      if(src != null && !dst.IsAssignableFrom(src))
        throw new TypeMismatch(src, dst);
    }
  }
}
