using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Radzen;

using AccountingBookkeeping.Server.Data;

namespace AccountingBookkeeping.Server
{
    public partial class accountingService
    {
        accountingContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly accountingContext context;
        private readonly NavigationManager navigationManager;

        public accountingService(accountingContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);


        public async Task ExportPricingsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/accounting/pricings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/accounting/pricings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportPricingsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/accounting/pricings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/accounting/pricings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnPricingsRead(ref IQueryable<AccountingBookkeeping.Server.Models.accounting.Pricing> items);

        public async Task<IQueryable<AccountingBookkeeping.Server.Models.accounting.Pricing>> GetPricings(Query query = null)
        {
            var items = Context.Pricings.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnPricingsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnPricingGet(AccountingBookkeeping.Server.Models.accounting.Pricing item);

        public async Task<AccountingBookkeeping.Server.Models.accounting.Pricing> GetPricingById(string id)
        {
            var items = Context.Pricings
                              .AsNoTracking()
                              .Where(i => i.Id == id);

  
            var itemToReturn = items.FirstOrDefault();

            OnPricingGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnPricingCreated(AccountingBookkeeping.Server.Models.accounting.Pricing item);
        partial void OnAfterPricingCreated(AccountingBookkeeping.Server.Models.accounting.Pricing item);

        public async Task<AccountingBookkeeping.Server.Models.accounting.Pricing> CreatePricing(AccountingBookkeeping.Server.Models.accounting.Pricing pricing)
        {
            OnPricingCreated(pricing);

            var existingItem = Context.Pricings
                              .Where(i => i.Id == pricing.Id)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Pricings.Add(pricing);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(pricing).State = EntityState.Detached;
                throw;
            }

            OnAfterPricingCreated(pricing);

            return pricing;
        }

        public async Task<AccountingBookkeeping.Server.Models.accounting.Pricing> CancelPricingChanges(AccountingBookkeeping.Server.Models.accounting.Pricing item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnPricingUpdated(AccountingBookkeeping.Server.Models.accounting.Pricing item);
        partial void OnAfterPricingUpdated(AccountingBookkeeping.Server.Models.accounting.Pricing item);

        public async Task<AccountingBookkeeping.Server.Models.accounting.Pricing> UpdatePricing(string id, AccountingBookkeeping.Server.Models.accounting.Pricing pricing)
        {
            OnPricingUpdated(pricing);

            var itemToUpdate = Context.Pricings
                              .Where(i => i.Id == pricing.Id)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(pricing);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterPricingUpdated(pricing);

            return pricing;
        }

        partial void OnPricingDeleted(AccountingBookkeeping.Server.Models.accounting.Pricing item);
        partial void OnAfterPricingDeleted(AccountingBookkeeping.Server.Models.accounting.Pricing item);

        public async Task<AccountingBookkeeping.Server.Models.accounting.Pricing> DeletePricing(string id)
        {
            var itemToDelete = Context.Pricings
                              .Where(i => i.Id == id)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnPricingDeleted(itemToDelete);


            Context.Pricings.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterPricingDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}