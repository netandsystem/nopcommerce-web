using Nop.Core;
using System.Collections.Generic;

namespace Nop.Plugin.Api.Models;

#nullable enable

public class DbResult<T> where T : BaseEntity
{
    public List<T> Updated { get; set; } = new();
    public List<T> Inserted { get; set; } = new();
    public List<int> Deleted { get; set; } = new();
}
