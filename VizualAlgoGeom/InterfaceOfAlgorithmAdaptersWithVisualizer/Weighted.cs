namespace InterfaceOfAlgorithmAdaptersWithVisualizer
{
  public class Weighted<T>
  {
    public Weighted(T element, double weight)
    {
      Element = element;
      Weight = weight;
    }

    public T Element { get; private set; }
    public double Weight { get; private set; }
  }
}