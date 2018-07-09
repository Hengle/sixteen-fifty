namespace SixteenFifty.Editor {
  using EventItems;
  
  public class EventItemControl : SubtypeControl<IScript> {
    public EventItemControl(string selectorLabel, SubtypeSelectorContext<IScript> context) :
    base(selectorLabel, context) {
    }
  }
}
