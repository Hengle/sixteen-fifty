using System;

namespace SixteenFifty {
  public class UnhandledFieldType : SixteenFiftyException {
    public readonly Type type;
    
    public UnhandledFieldType(Type type) :
    base(type.ToString()) {
      this.type = type;
    }
  }
}
