using System;

namespace Nop.Core;

/// <summary>
/// Represents the base interface for entities that are synchronized with an external service
/// </summary>
public interface IBaseSyncEntity
{
    public DateTime UpdatedOnUtc { get; set; }
}