using System;

using UnityEngine;

namespace SixteenFifty.Variables {
  /**
   * \brief
   * Represents a simple variable of type `T`, whose value can be read
   * and written.
   */
  public interface IVariable<T> {
    T Value {
      get;
      set;
    }
  }

  /**
   * \brief
   * A variable of type `T`.
   */
  public class Variable<T> : ScriptableObject, IVariable<T> {
    /**
     * \brief
     * The raw value of the variable.
     */
    public T value;

    /**
     * \brief
     * Access the value of the variable.
     *
     * Using the setter of this property will raise #Changed.
     */
    public T Value {
      get {
        return value;
      }
      set {
        this.value = value;
        Changed?.Invoke(value);
      }
    }

    /**
     * \brief
     * Raised when the value is changed.
     */
    public event Action<T> Changed;
  }
}
