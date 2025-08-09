using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

public class InvoiceApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public InvoiceApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateInvoice_ValidInvoice_ReturnsCreated()
    {
        var invoice = new InvoiceCreateDto
        {
            CustomerName = "Test Customer",
            InvoiceDate = DateTime.Today,
            Lines = new List<InvoiceLineDto>
            {
                new() { Description = "Item 1", Quantity = 2, UnitPrice = 10m },
                new() { Description = "Item 2", Quantity = 1, UnitPrice = 20m }
            }
        };

        var response = await _client.PostAsJsonAsync("/invoices", invoice);

        response.EnsureSuccessStatusCode();
        var returnedInvoice = await response.Content.ReadFromJsonAsync<Invoice>();
        Assert.NotNull(returnedInvoice);
        Assert.Equal(invoice.CustomerName, returnedInvoice.CustomerName);
        Assert.Equal(invoice.Lines.Count, returnedInvoice.Lines.Count);
    }
}
