using Nop.Core.Configuration;
using Nop.Plugin.Api.Infrastructure;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Text.Json;

#nullable enable

namespace Nop.Plugin.Api.Domain;

public class MobileSettings : ISettings
{
    #region Reports

    [NopResourceDisplayName("Plugins.Api.Mobile.Report.CustomerOpening")]
    public string CustomerOpening { get; set; } = JsonSerializer.Serialize(new Dictionary<string, object>());

    public Dictionary<string, object> CustomerOpeningDic
    {
        get
        {
            var obj = JsonSerializer.Deserialize<Dictionary<string, object>>(CustomerOpening);

            if (obj == null)
            {
                return new Dictionary<string, object>();
            }

            return obj;
        }
    }

    #endregion

}
