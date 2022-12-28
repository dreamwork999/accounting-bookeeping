using System;
using System.Net;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AccountingBookkeeping.Server.Controllers.accounting
{
    [Route("odata/accounting/Pricings")]
    public partial class PricingsController : ODataController
    {
        private AccountingBookkeeping.Server.Data.accountingContext context;

        public PricingsController(AccountingBookkeeping.Server.Data.accountingContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<AccountingBookkeeping.Server.Models.accounting.Pricing> GetPricings()
        {
            var items = this.context.Pricings.AsQueryable<AccountingBookkeeping.Server.Models.accounting.Pricing>();
            this.OnPricingsRead(ref items);

            return items;
        }

        partial void OnPricingsRead(ref IQueryable<AccountingBookkeeping.Server.Models.accounting.Pricing> items);

        partial void OnPricingGet(ref SingleResult<AccountingBookkeeping.Server.Models.accounting.Pricing> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/accounting/Pricings(Id={Id})")]
        public SingleResult<AccountingBookkeeping.Server.Models.accounting.Pricing> GetPricing(string key)
        {
            var items = this.context.Pricings.Where(i => i.Id == Uri.UnescapeDataString(key));
            var result = SingleResult.Create(items);

            OnPricingGet(ref result);

            return result;
        }
        partial void OnPricingDeleted(AccountingBookkeeping.Server.Models.accounting.Pricing item);
        partial void OnAfterPricingDeleted(AccountingBookkeeping.Server.Models.accounting.Pricing item);

        [HttpDelete("/odata/accounting/Pricings(Id={Id})")]
        public IActionResult DeletePricing(string key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Pricings
                    .Where(i => i.Id == Uri.UnescapeDataString(key))
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnPricingDeleted(item);
                this.context.Pricings.Remove(item);
                this.context.SaveChanges();
                this.OnAfterPricingDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnPricingUpdated(AccountingBookkeeping.Server.Models.accounting.Pricing item);
        partial void OnAfterPricingUpdated(AccountingBookkeeping.Server.Models.accounting.Pricing item);

        [HttpPut("/odata/accounting/Pricings(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutPricing(string key, [FromBody]AccountingBookkeeping.Server.Models.accounting.Pricing item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.Id != Uri.UnescapeDataString(key)))
                {
                    return BadRequest();
                }
                this.OnPricingUpdated(item);
                this.context.Pricings.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Pricings.Where(i => i.Id == Uri.UnescapeDataString(key));
                ;
                this.OnAfterPricingUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/accounting/Pricings(Id={Id})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchPricing(string key, [FromBody]Delta<AccountingBookkeeping.Server.Models.accounting.Pricing> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Pricings.Where(i => i.Id == Uri.UnescapeDataString(key)).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnPricingUpdated(item);
                this.context.Pricings.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Pricings.Where(i => i.Id == Uri.UnescapeDataString(key));
                ;
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnPricingCreated(AccountingBookkeeping.Server.Models.accounting.Pricing item);
        partial void OnAfterPricingCreated(AccountingBookkeeping.Server.Models.accounting.Pricing item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] AccountingBookkeeping.Server.Models.accounting.Pricing item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null)
                {
                    return BadRequest();
                }

                this.OnPricingCreated(item);
                this.context.Pricings.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Pricings.Where(i => i.Id == item.Id);

                ;

                this.OnAfterPricingCreated(item);

                return new ObjectResult(SingleResult.Create(itemToReturn))
                {
                    StatusCode = 201
                };
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }
    }
}
