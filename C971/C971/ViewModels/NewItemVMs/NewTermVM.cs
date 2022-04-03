using C971.Models.DatabaseModels;

namespace C971.ViewModels.NewItemVMs
{
  /// <summary>
  /// VM For a New Academic Term
  /// </summary>
  public class NewTermVM : BaseAddPageVM<AcademicTerm>
  {
    /// <inheritdoc cref="NewTermVM" />
    public NewTermVM()
    {
      Item = new();
    }
  }
}
