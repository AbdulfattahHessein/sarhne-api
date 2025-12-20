using System;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public static class ModelBuilderExtensions
{
    extension(ModelBuilder modelBuilder)
    {
        public void ApplyBaseEntityConfiguration()
        {
            var entityTypes = modelBuilder.Model.GetEntityTypes()
                .Where(e => typeof(BaseEntity).IsAssignableFrom(e.ClrType));

            foreach (var entity in entityTypes)
            {
                modelBuilder.Entity(entity.ClrType)
                    .Property("Id")
                    .HasDefaultValueSql("NEWSEQUENTIALID()");
            }
        }
    }

}
