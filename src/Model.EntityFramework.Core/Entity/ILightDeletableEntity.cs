namespace Pally.Model.EntityFramework.Core.Entity
{
    public interface ILightDeletableEntity : IEntity
    {
        bool IsDeleted { get; set; }
    }
}
