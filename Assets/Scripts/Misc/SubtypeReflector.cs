using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

using UnityEngine;

public static class SubtypeReflector {
  /**
   * \brief Gets all subclasses of a given class.
   */
  public static IEnumerable<Type> GetSubtypes<T>() where T : class =>
    GetSubtypes(typeof(T));

  public static IEnumerable<Type> GetSubtypes(Type type) =>
    typeof(SubtypeReflector).Assembly.GetTypes()
    .Where(t => t.IsClass && t.IsSubclassOf(type));
}
