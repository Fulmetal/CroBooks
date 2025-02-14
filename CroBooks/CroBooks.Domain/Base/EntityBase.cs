﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CroBooks.Domain.Base;

public interface IEntityBase<TKey>
{
    TKey Id { get; set; }
}

public interface IDeleteEntity
{
    bool IsDeleted { get; set; }
    DateTime? DeletedDate { get; set; }
}

public interface IDeleteEntity<TKey> : IDeleteEntity, IEntityBase<TKey>
{
}

public interface IAuditEntity
{
    DateTime CreatedDate { get; set; }
    DateTime? UpdatedDate { get; set; }
}

public interface IAuditEntity<TKey> : IAuditEntity, IDeleteEntity<TKey>
{
}


public abstract class EntityBase<TKey> : IEntityBase<TKey>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public virtual TKey Id { get; set; } = default!;
}

public abstract class DeleteEntity<TKey> : EntityBase<TKey>, IDeleteEntity<TKey>
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletedDate { get; set; }
}

public abstract class AuditEntity<TKey> : DeleteEntity<TKey>, IAuditEntity<TKey>
{
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}