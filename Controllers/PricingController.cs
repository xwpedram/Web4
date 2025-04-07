using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using Web4.Controllers;

[ApiController]
[Route("[controller]")]
public class PricingController : BaseController
{
    private readonly IHttpClientFactory _httpClientFactory;

    public PricingController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        string content = "{\r\n  \"pageTitle\": \"Pricing Plans - Web4 UI\",\r\n  \"sections\": [\r\n    {\r\n      \"type\": \"hero\",\r\n      \"title\": \"Simple & Transparent Pricing\",\r\n      \"subtitle\": \"Start for free. Upgrade only when you need more.\",\r\n      \"image\": \"https://placehold.co/600x300\"\r\n    },\r\n    {\r\n      \"type\": \"plans\",\r\n      \"title\": \"Choose the Plan That Works for You\",\r\n      \"items\": [\r\n        {\r\n          \"name\": \"Free\",\r\n          \"price\": \"$0\",\r\n          \"description\": \"Perfect for testing and personal projects.\",\r\n          \"features\": [\r\n            \"Up to 20 generated pages\",\r\n            \"Access to all core features\",\r\n            \"Responsive HTML output\",\r\n            \"OpenAI token provided by us\"\r\n          ],\r\n          \"buttonText\": \"Start Free\",\r\n          \"highlight\": false\r\n        },\r\n        {\r\n          \"name\": \"Pro\",\r\n          \"price\": \"$15/month\",\r\n          \"description\": \"For developers and small teams who need more power.\",\r\n          \"features\": [\r\n            \"Up to 500 generated pages/month\",\r\n            \"Bring your own OpenAI API key\",\r\n            \"Priority generation speed\",\r\n            \"Email support\"\r\n          ],\r\n          \"buttonText\": \"Go Pro\",\r\n          \"highlight\": true\r\n        },\r\n        {\r\n          \"name\": \"Enterprise\",\r\n          \"price\": \"Custom\",\r\n          \"description\": \"Custom models, private deployment & full control.\",\r\n          \"features\": [\r\n            \"Unlimited generations\",\r\n            \"Use your own LLM or private API\",\r\n            \"Dedicated support\",\r\n            \"Custom integrations\"\r\n          ],\r\n          \"buttonText\": \"Contact Sales\",\r\n          \"highlight\": false\r\n        }\r\n      ]\r\n    },\r\n    {\r\n      \"type\": \"faq-link\",\r\n      \"title\": \"Not sure which plan is right?\",\r\n      \"description\": \"Check out our FAQ or reach out — we’re happy to help.\",\r\n      \"buttonText\": \"Visit FAQ\",\r\n      \"buttonLink\": \"/faq\"\r\n    },\r\n    {\r\n      \"type\": \"cta\",\r\n      \"title\": \"Get Started in Seconds\",\r\n      \"subtitle\": \"No credit card required. Just start building.\",\r\n      \"buttonText\": \"Generate a Page\",\r\n      \"buttonLink\": \"/how-it-works\"\r\n    }\r\n  ],\r\n  \"footer\": {\r\n    \"links\": [\r\n      { \"label\": \"Home\", \"url\": \"/\" },\r\n      { \"label\": \"About\", \"url\": \"/about\" },\r\n      { \"label\": \"Examples\", \"url\": \"/examples\" },\r\n      { \"label\": \"FAQ\", \"url\": \"/faq\" },\r\n      { \"label\": \"Contact\", \"url\": \"/contact\" }\r\n    ],\r\n    \"copyright\": \"© 2025 Web4 UI. All rights reserved.\"\r\n  },\r\n  \"notes\": \"This pricing page should present three distinct plans with clarity. The 'Pro' plan is highlighted as the most recommended option.\"\r\n}\r\n";

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
