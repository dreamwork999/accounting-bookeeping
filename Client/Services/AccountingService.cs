
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Web;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Radzen;

namespace AccountingBookkeeping.Client
{
    public partial class accountingService
    {
        private readonly HttpClient httpClient;
        private readonly Uri baseUri;
        private readonly NavigationManager navigationManager;

        public accountingService(NavigationManager navigationManager, HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;

            this.navigationManager = navigationManager;
            this.baseUri = new Uri($"{navigationManager.BaseUri}odata/accounting/");
        }


        public async System.Threading.Tasks.Task ExportPricingsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/accounting/pricings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/accounting/pricings/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportPricingsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/accounting/pricings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/accounting/pricings/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetPricings(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<AccountingBookkeeping.Server.Models.accounting.Pricing>> GetPricings(Query query)
        {
            return await GetPricings(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<AccountingBookkeeping.Server.Models.accounting.Pricing>> GetPricings(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Pricings");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetPricings(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<AccountingBookkeeping.Server.Models.accounting.Pricing>>(response);
        }

        partial void OnCreatePricing(HttpRequestMessage requestMessage);

        public async Task<AccountingBookkeeping.Server.Models.accounting.Pricing> CreatePricing(AccountingBookkeeping.Server.Models.accounting.Pricing pricing = default(AccountingBookkeeping.Server.Models.accounting.Pricing))
        {
            var uri = new Uri(baseUri, $"Pricings");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(pricing), Encoding.UTF8, "application/json");

            OnCreatePricing(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<AccountingBookkeeping.Server.Models.accounting.Pricing>(response);
        }

        partial void OnDeletePricing(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeletePricing(string id = default(string))
        {
            var uri = new Uri(baseUri, $"Pricings('{HttpUtility.UrlEncode(id.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeletePricing(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetPricingById(HttpRequestMessage requestMessage);

        public async Task<AccountingBookkeeping.Server.Models.accounting.Pricing> GetPricingById(string expand = default(string), string id = default(string))
        {
            var uri = new Uri(baseUri, $"Pricings('{HttpUtility.UrlEncode(id.Trim().Replace("'", "''"))}')");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetPricingById(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<AccountingBookkeeping.Server.Models.accounting.Pricing>(response);
        }

        partial void OnUpdatePricing(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdatePricing(string id = default(string), AccountingBookkeeping.Server.Models.accounting.Pricing pricing = default(AccountingBookkeeping.Server.Models.accounting.Pricing))
        {
            var uri = new Uri(baseUri, $"Pricings('{HttpUtility.UrlEncode(id.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(pricing), Encoding.UTF8, "application/json");

            OnUpdatePricing(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }
    }
}