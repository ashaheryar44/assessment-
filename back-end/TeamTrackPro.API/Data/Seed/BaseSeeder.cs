using System;
using Microsoft.EntityFrameworkCore;
using TeamTrackPro.API.Models;

namespace TeamTrackPro.API.Data.Seed
{
    public abstract class BaseSeeder<TEntity> where TEntity : BaseEntity
    {
        protected readonly ModelBuilder _modelBuilder;
        protected static readonly DateTime _seedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        protected BaseSeeder(ModelBuilder modelBuilder)
        {
            _modelBuilder = modelBuilder;
        }

        protected void SetCommonProperties(TEntity entity)
        {
            entity.CreatedAt = _seedDate;
            entity.UpdatedAt = null;
            entity.IsActive = true;
        }

        public abstract void Seed();
    }
} 