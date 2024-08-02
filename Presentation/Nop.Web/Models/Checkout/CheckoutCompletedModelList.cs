//NAS CODE

using Nop.Web.Framework.Models;
using System.Collections;
using System.Collections.Generic;

namespace Nop.Web.Models.Checkout
{
    public partial record CheckoutCompletedModelList : BaseNopModel
    {
        public IList<CheckoutCompletedModel> OrdersModelList { get; set; }
    }
}