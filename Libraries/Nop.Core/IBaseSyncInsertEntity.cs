using System;

namespace Nop.Core;

/// <summary>
/// Represents the base class for entities that are synchronized with an external service
/// </summary>
public interface IBaseSyncInsertEntity
{
#nullable enable
    public bool Synchronized { get; set; }
}