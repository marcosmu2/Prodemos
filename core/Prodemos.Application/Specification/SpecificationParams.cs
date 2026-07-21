namespace Prodemos.Application.Specification;
public abstract class SpecificationParams
{
    public string Sort { get; set; } = string.Empty;
    public int PageIndex { get; set; }
    
    private const int MaxPageSize = 50;

    private int _pageSize = 3;

    public int PageSize {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }

    public string Serach { get; set; } = string.Empty;
}
