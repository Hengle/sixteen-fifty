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

  public abstract class AnyVariable : ScriptableObject {
  }

  /**
   * \brief
   * A variable of type `T`.
   */
  public class Variable<T> : AnyVariable, IVariable<T> {
    /**
     * \brief
     * The raw value of the variable.
     */
    public T value;

    /**
     * \brief
     * The raw value of the variable at runtime.
     *
     * `OnEnable` will assign `value` (the static value) to this
     * value.
     */
    public T runtimeValue;

    /**
     * \brief
     * Access the value of the variable.
     *
     * Using the setter of this property will raise #Changed.
     */
    public T Value {
      get {
        return runtimeValue;
      }
      set {
        runtimeValue = value;
        Changed?.Invoke(runtimeValue);
      }
    }

    /**
     * \brief
     * Raised when the value is changed.
     */
    public event Action<T> Changed;

    void OnEnable() {
      runtimeValue = value;
    }
  }
}
