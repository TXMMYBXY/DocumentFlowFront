using DocumentFlowing.Interfaces.Client;

namespace DocumentFlowing.Models.Boss;

public class CreateTemplateModel
{
    private readonly IBossClient _bossClient;
    
    public CreateTemplateModel(IBossClient bossClient)
    {
        _bossClient = bossClient;
    }
    
    
}