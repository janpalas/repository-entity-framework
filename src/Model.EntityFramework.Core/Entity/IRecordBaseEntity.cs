using System;

namespace Pally.Model.EntityFramework.Core.Entity
{
    public interface IRecordBaseEntity
    {
        DateTime Created { get; set; }

        int CreatorId { get; set; }

        DateTime? Edited { get; set; }

        int? EditorId { get; set; }
    }
}
