using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace AccountingBookkeeping.Client.Pages
{
    public partial class AddPricing
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }
        [Inject]
        public accountingService accountingService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            pricing = new AccountingBookkeeping.Server.Models.accounting.Pricing();
        }
        protected bool errorVisible;
        protected AccountingBookkeeping.Server.Models.accounting.Pricing pricing;

        protected async Task FormSubmit()
        {
            try
            {
                var result = await accountingService.CreatePricing(pricing);
                DialogService.Close(pricing);
            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }


        protected bool hasChanges = false;
        protected bool canEdit = true;

        [Inject]
        protected SecurityService Security { get; set; }
    }
}