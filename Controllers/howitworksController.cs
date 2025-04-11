using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using Web4.Controllers;

[ApiController]
[Route("how-it-works")]
public class HowItWorksController : BaseController
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HowItWorksController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        string content = CombinePrompt("{\r\n  \"pageTitle\": \"How It Works - Web4 UI\",\r\n  \"sections\": [\r\n    {\r\n      \"type\": \"hero\",\r\n      \"title\": \"Build Web Pages with Just One Prompt\",\r\n      \"subtitle\": \"Let AI turn your thoughts into functional HTML.\",\r\n      \"image\": \"https://placehold.co/600x350\"\r\n    },\r\n    {\r\n      \"type\": \"steps\",\r\n      \"title\": \"How It Works\",\r\n      \"items\": [\r\n        {\r\n          \"step\": 1,\r\n          \"title\": \"Describe Your Page\",\r\n          \"description\": \"Write what you want. It can be a short sentence like 'A product page with an image, title, price, and buy button.'\",\r\n          \"image\": \"https://placehold.co/500x300\"\r\n        },\r\n        {\r\n          \"step\": 2,\r\n          \"title\": \"We Generate HTML\",\r\n          \"description\": \"Web4 UI sends your prompt to a powerful AI model (like GPT-4) and returns full HTML/CSS/JS instantly.\",\r\n          \"image\": \"https://placehold.co/500x300\"\r\n        },\r\n        {\r\n          \"step\": 3,\r\n          \"title\": \"Use It Anywhere\",\r\n          \"description\": \"Copy the result into your own website, CMS, or app — or even build full applications with multiple pages.\",\r\n          \"image\": \"https://placehold.co/500x300\"\r\n        }\r\n      ]\r\n    },\r\n    {\r\n      \"type\": \"benefits\",\r\n      \"title\": \"Why It's Different\",\r\n      \"items\": [\r\n        {\r\n          \"icon\": \"\U0001f9e9\",\r\n          \"title\": \"No Templates Required\",\r\n          \"description\": \"Each page is uniquely generated based on your input — no fixed layouts.\"\r\n        },\r\n        {\r\n          \"icon\": \"🕐\",\r\n          \"title\": \"Time-Saving\",\r\n          \"description\": \"You don’t need to build components or write frontend code manually anymore.\"\r\n        },\r\n        {\r\n          \"icon\": \"💡\",\r\n          \"title\": \"Smart UI Design\",\r\n          \"description\": \"LLM adapts to your intent, learns your style, and evolves with every use.\"\r\n        }\r\n      ]\r\n    },\r\n    {\r\n      \"type\": \"faq-link\",\r\n      \"title\": \"Got more questions?\",\r\n      \"description\": \"Check out our Frequently Asked Questions page for more details.\",\r\n      \"buttonText\": \"Go to FAQ\",\r\n      \"buttonLink\": \"/faq\"\r\n    },\r\n    {\r\n      \"type\": \"cta\",\r\n      \"title\": \"Start Creating Now!\",\r\n      \"subtitle\": \"The first 20 pages are free. No signup required.\",\r\n      \"buttonText\": \"Generate My First Page\",\r\n      \"buttonLink\": \"/\"\r\n    }\r\n  ],\r\n  \"footer\": {\r\n    \"links\": [\r\n      { \"label\": \"Home\", \"url\": \"/\" },\r\n      { \"label\": \"About\", \"url\": \"/about\" },\r\n      { \"label\": \"FAQ\", \"url\": \"/faq\" },\r\n      { \"label\": \"Examples\", \"url\": \"/examples\" },\r\n      { \"label\": \"Contact\", \"url\": \"/contact\" }\r\n    ],\r\n    \"copyright\": \"© 2025 Web4 UI. All rights reserved.\"\r\n  },\r\n  \"notes\": \"This page is intended to walk users through the process of generating HTML pages with Web4 UI, making the tool feel easy and magical.\"\r\n}\r\n");

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
