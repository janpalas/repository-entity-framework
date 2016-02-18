namespace Pally.Model.EntityFramework.Core.Entity
{
    public interface IConcurrentEntity
    {
        int RowVersion { get; set; }
    }
}
