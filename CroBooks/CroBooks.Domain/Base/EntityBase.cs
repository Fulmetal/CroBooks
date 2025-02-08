using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CroBooks.Domain.Base;

public interface IEntityBase<TKey>
{
    TKey Id { get; set; }
}

public interface ITenantEntity<TKey, TTennantKey> : IEntityBase<TKey>
{
    TTennantKey TenantId { get; set; }
}

public interface IDeleteEntity
{
    bool IsDeleted { get; set; }
    DateTime? DeletedDate { get; set; }
}

public interface IDeleteEntity<TKey> : IDeleteEntity, IEntityBase<TKey>
{
}

public interface IDeleteEntityWithTenant<TKey, TTennantKey, TTennantKeyNullable> : IDeleteEntity,
    ITenantEntity<TKey, TTennantKey>
{
    TTennantKeyNullable? DeletedBy { get; set; }
}

public interface IDeleteEntityWithUser<TKey, TTennantKeyNullable> : IDeleteEntity, IEntityBase<TKey>
{
    TTennantKeyNullable? DeletedBy { get; set; }
}

public interface IAuditEntity
{
    DateTime CreatedDate { get; set; }
    DateTime? UpdatedDate { get; set; }
}

public interface IAuditEntity<TKey> : IAuditEntity, IDeleteEntity<TKey>
{
}

public interface IAuditEntityWithTenant<TKey, TTennantKey, TTennantKeyNullable> : IAuditEntity,
    IDeleteEntityWithTenant<TKey, TTennantKey, TTennantKeyNullable>
{
    TTennantKey CreatedBy { get; set; }
    TTennantKeyNullable? UpdatedBy { get; set; }
}

public interface IAuditEntityWithUser<TKey, TTennantKey, TTennantKeyNullable> : IAuditEntity,
    IDeleteEntityWithUser<TKey, TTennantKeyNullable>
{
    TTennantKey CreatedBy { get; set; }
    TTennantKeyNullable? UpdatedBy { get; set; }
}

public abstract class EntityBase<TKey> : IEntityBase<TKey>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public virtual TKey Id { get; set; } = default!;
}

public abstract class TenantEntity<TKey, TTennantKey> : EntityBase<TKey>, ITenantEntity<TKey, TTennantKey>
{
    public virtual TTennantKey TenantId { get; set; } = default!;
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

public abstract class DeleteEntityWithTenant<TKey, TTennatKey, TTennantKeyNullable> : TenantEntity<TKey, TTennatKey>,
    IDeleteEntityWithTenant<TKey, TTennatKey, TTennantKeyNullable>
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletedDate { get; set; }
    public TTennantKeyNullable? DeletedBy { get; set; }
}

public abstract class DeleteEntityWithUser<TKey, TTennantKeyNullable> : EntityBase<TKey>,
    IDeleteEntityWithUser<TKey, TTennantKeyNullable>
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletedDate { get; set; }
    public TTennantKeyNullable? DeletedBy { get; set; }
}

public abstract class AuditEntityWithTenant<TKey, TTennantKey, TTennantKeyNullable> :
    DeleteEntityWithTenant<TKey, TTennantKey, TTennantKeyNullable>,
    IAuditEntityWithTenant<TKey, TTennantKey, TTennantKeyNullable>
{
    public DateTime CreatedDate { get; set; }
    public TTennantKey CreatedBy { get; set; } = default!;
    public DateTime? UpdatedDate { get; set; }
    public TTennantKeyNullable? UpdatedBy { get; set; }
}

public abstract class AuditEntityWithUser<TKey, TTennantKey, TTennantKeyNullable> :
    DeleteEntityWithUser<TKey, TTennantKeyNullable>,
    IAuditEntityWithUser<TKey, TTennantKey, TTennantKeyNullable>
{
    public DateTime CreatedDate { get; set; }
    public TTennantKey CreatedBy { get; set; } = default!;
    public DateTime? UpdatedDate { get; set; }
    public TTennantKeyNullable? UpdatedBy { get; set; }
}