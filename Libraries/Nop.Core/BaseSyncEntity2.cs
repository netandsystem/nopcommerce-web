using System;

namespace Nop.Core;

/// <summary>
/// Represents the base class for entities that are synchronized with an external service
/// </summary>
public abstract class BaseSyncEntity2 : BaseSyncEntity
{
#nullable enable
    public string? ExtId { get; set; }
}