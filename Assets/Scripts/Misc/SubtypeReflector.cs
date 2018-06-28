using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using UnityEngine;

public static class SubtypeReflector {
  /**
   * \brief
   * Enumerates all types in the assembly that contains the SubtypeReflector class.
   */
  public static IEnumerable<Type> GetTypes() =>
    typeof(SubtypeReflector).Assembly.GetTypes();

  /**
   * \brief
   * Enumerates all subclasses of a given class type `T`.
   */
  public static IEnumerable<Type> GetSubtypes<T>() where T : class =>
    GetSubtypes(typeof(T));

  /**
   * \brief
   * Enumerates all subclasses of the class `type`.
   *
   * Prefer calling `GetSubclasses<T>` using a type argument instead
   * of this method, since this method does not check that `type`
   * refers to a class type.
   */
  public static IEnumerable<Type> GetSubtypes(Type type) =>
    GetTypes().Where(t => t.IsClass && t.IsSubclassOf(type));

  /**
   * \brief
   * Enumerates all implementations of the interface `T`.
   */
  public static IEnumerable<Type> GetImplementations<T>() where T : class =>
    GetImplementations(typeof(T));

  /**
   * \brief
   * Enumerates all implementations of the interface `type`.
   *
   * Prefer calling `GetImplementations<T>` using a type argument
   * instead of this method, since this method does not check that
   * `type` refers to an interface type.
   */
  public static IEnumerable<Type> GetImplementations(Type type) =>
    GetTypes().Where(t => t.IsClass && type.IsAssignableFrom(t));

  public static IEnumerable<Type> WithAttribute<T>(this IEnumerable<Type> self) =>
    self.WithAttribute(typeof(T));

  public static IEnumerable<Type> WithAttribute(this IEnumerable<Type> self, Type attrType) =>
    self.Where(
      type =>
      type.CustomAttributes.Any(a => a.AttributeType == attrType));
}
