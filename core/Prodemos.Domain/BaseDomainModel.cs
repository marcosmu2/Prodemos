using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prodemos.Domain;
public class BaseDomainModel
{
    public Guid Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime UpdatedDate { get; set;}
    public string? UpdatedBy { get; set;}
}
