using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Prodemos.Domain;
public class BaseDomainModel
{
    [Key]
    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; }
    [Column(TypeName = "NVARCHAR(36)")]
    public string? CreatedBy { get; set; }
    public DateTime UpdatedDate { get; set; }
    [Column(TypeName = "NVARCHAR(36)")]
    public string? UpdatedBy { get; set; }
}
