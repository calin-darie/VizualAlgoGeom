namespace InterfaceOfSnapshotsWithAlgorithmsAndVisualizer
{
  public class SnapshotDescription
  {
    public static readonly SnapshotDescription Empty = new SnapshotDescription
    {
      PseudocodeLine = 0,
      Remark = string.Empty
    };

    public int PseudocodeLine { get; set; }
    public string Remark { get; set; }

    public SnapshotDescription WithFormattedRemark(params object[] formatObjects)
    {
      return new SnapshotDescription
      {
        PseudocodeLine = PseudocodeLine,
        Remark = string.Format(Remark, formatObjects)
      };
    }
  }
}