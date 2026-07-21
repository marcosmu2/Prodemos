namespace Prodemos.Application.Exceptions;
public class NotFoundException : ApplicationException 
{
    public NotFoundException(string name, object key): base($"Entity \"{name}\" with {key} cannot be found")
    {
        
    }
}
