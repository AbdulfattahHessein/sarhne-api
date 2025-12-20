using System;

namespace Core.Entities;

public class BaseEntity
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public BaseEntity() { }

    public BaseEntity(Guid id)
    {
        Id = id;
    }
}
