using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using Web4.Controllers;

[ApiController]
[Route("[controller]")]
public class ContactController : BaseController
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ContactController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        string content = CombinePrompt("{\r\n  \"pageTitle\": \"Contact Us - Web4 UI\",\r\n  \"sections\": [\r\n    {\r\n      \"type\": \"hero\",\r\n      \"title\": \"Get in Touch\",\r\n      \"subtitle\": \"We’re here to connect — feel free to reach out via the following channels.\",\r\n      \"image\": \"https://placehold.co/600x300\"\r\n    },\r\n    {\r\n      \"type\": \"alternative-contact\",\r\n      \"title\": \"Contact Information\",\r\n      \"items\": [\r\n        {\r\n          \"icon\": \"📧\",\r\n          \"label\": \"Email\",\r\n          \"value\": \"support@web4ui.com\"\r\n        },\r\n        {\r\n          \"icon\": \"🐙\",\r\n          \"label\": \"GitHub\",\r\n          \"value\": \"github.com/web4ui\"\r\n        },\r\n        {\r\n          \"icon\": \"💬\",\r\n          \"label\": \"Twitter\",\r\n          \"value\": \"@web4ui\"\r\n        }\r\n      ]\r\n    },\r\n    {\r\n      \"type\": \"cta\",\r\n      \"title\": \"Need Help Getting Started?\",\r\n      \"subtitle\": \"Check out our FAQ or generate your first page now.\",\r\n      \"buttonText\": \"Visit FAQ\",\r\n      \"buttonLink\": \"/faq\"\r\n    }\r\n  ],\r\n  \"footer\": {\r\n    \"links\": [\r\n      { \"label\": \"Home\", \"url\": \"/\" },\r\n      { \"label\": \"About\", \"url\": \"/about\" },\r\n      { \"label\": \"How It Works\", \"url\": \"/how-it-works\" },\r\n      { \"label\": \"Examples\", \"url\": \"/examples\" },\r\n      { \"label\": \"Pricing\", \"url\": \"/pricing\" }\r\n    ],\r\n    \"copyright\": \"© 2025 Web4 UI. All rights reserved.\"\r\n  },\r\n  \"notes\": \"This version of the contact page only displays static contact options without a message form.\"\r\n}");

        var requestBody = new
        {
            userId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
            inputType = "Text",
            content
        };

        var httpClient = _httpClientFactory.CreateClient();

        var response = await httpClient.PostAsync(
             mainAddress + "api/generate", // آدرس واقعی یا نسبی
            new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
        );

        var jsonResponse = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            return Content($"<h2>خطا در تولید صفحه</h2><pre>{jsonResponse}</pre>", "text/html");

        var doc = JsonDocument.Parse(jsonResponse);
        var html = doc.RootElement.GetProperty("html").GetString();
        html = StripCodeBlock(html);
        return Content(html, "text/html", Encoding.UTF8);
    }
}
