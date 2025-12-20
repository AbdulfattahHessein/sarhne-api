using System;

namespace Core.Entities;

public class Settings(Guid id) : BaseEntity(id)
{
    public bool AllowMessages { get; set; }
    public bool AllowImages { get; set; }
    public bool AllowAnonymous { get; set; }
}
