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
        string content = "{\r\n  \"pageTitle\": \"Contact Us - Web4 UI\",\r\n  \"sections\": [\r\n    {\r\n      \"type\": \"hero\",\r\n      \"title\": \"Let’s Talk\",\r\n      \"subtitle\": \"Have questions, feedback, or ideas? We’d love to hear from you.\",\r\n      \"image\": \"https://placehold.co/600x300\"\r\n    },\r\n    {\r\n      \"type\": \"contact-form\",\r\n      \"title\": \"Send Us a Message\",\r\n      \"fields\": [\r\n        { \"label\": \"Name\", \"type\": \"text\", \"name\": \"name\", \"required\": true },\r\n        { \"label\": \"Email\", \"type\": \"email\", \"name\": \"email\", \"required\": true },\r\n        { \"label\": \"Subject\", \"type\": \"text\", \"name\": \"subject\", \"required\": false },\r\n        { \"label\": \"Message\", \"type\": \"textarea\", \"name\": \"message\", \"required\": true }\r\n      ],\r\n      \"submitButtonText\": \"Send Message\"\r\n    },\r\n    {\r\n      \"type\": \"alternative-contact\",\r\n      \"title\": \"Other Ways to Reach Us\",\r\n      \"items\": [\r\n        {\r\n          \"icon\": \"📧\",\r\n          \"label\": \"Email\",\r\n          \"value\": \"support@web4ui.com\"\r\n        },\r\n        {\r\n          \"icon\": \"🐙\",\r\n          \"label\": \"GitHub\",\r\n          \"value\": \"github.com/web4ui\"\r\n        },\r\n        {\r\n          \"icon\": \"💬\",\r\n          \"label\": \"Twitter\",\r\n          \"value\": \"@web4ui\"\r\n        }\r\n      ]\r\n    },\r\n    {\r\n      \"type\": \"cta\",\r\n      \"title\": \"Need Help Getting Started?\",\r\n      \"subtitle\": \"Check out our FAQ or generate your first page now.\",\r\n      \"buttonText\": \"Visit FAQ\",\r\n      \"buttonLink\": \"/faq\"\r\n    }\r\n  ],\r\n  \"footer\": {\r\n    \"links\": [\r\n      { \"label\": \"Home\", \"url\": \"/\" },\r\n      { \"label\": \"About\", \"url\": \"/about\" },\r\n      { \"label\": \"How It Works\", \"url\": \"/how-it-works\" },\r\n      { \"label\": \"Examples\", \"url\": \"/examples\" },\r\n      { \"label\": \"Pricing\", \"url\": \"/pricing\" }\r\n    ],\r\n    \"copyright\": \"© 2025 Web4 UI. All rights reserved.\"\r\n  },\r\n  \"notes\": \"This contact page includes a clean contact form and optional static contact methods. The form fields can be wired up to an email handler or backend endpoint.\"\r\n}\r\n";

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
        html = html.Substring(8);
        html = html.Substring(0, html.Length - 3);
        return Content(html, "text/html", Encoding.UTF8);
    }
}
