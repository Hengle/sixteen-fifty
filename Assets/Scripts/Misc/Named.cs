public class Named<T> {
  public readonly string name;
  public readonly T value;

  public Named(string name, T value) {
    this.name = name;
    this.value = value;
  }
}
