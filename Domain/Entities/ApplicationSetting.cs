using Tickest.Domain.Entities.Base;

namespace Tickest.Domain.Entities;

public class ApplicationSetting : EntityBase
{
    public string Key { get; set; }
    public string Value { get; set; }
}
