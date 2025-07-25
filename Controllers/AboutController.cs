using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using Web4.Controllers;

[ApiController]
[Route("[controller]")]
public class AboutController : BaseController
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AboutController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        string content = CombinePrompt("{\r\n  \"pageTitle\": \"About Web4 UI\",\r\n  \"sections\": [\r\n    {\r\n      \"type\": \"hero\",\r\n      \"title\": \"Who We Are\",\r\n      \"subtitle\": \"We’re redefining how the web is built — one prompt at a time.\",\r\n      \"image\": \"https://placehold.co/600x350\"\r\n    },\r\n    {\r\n      \"type\": \"mission\",\r\n      \"title\": \"Our Mission\",\r\n      \"content\": \"To empower developers, designers, and small businesses with a faster, smarter way to build modern web interfaces using the power of AI. We believe in a future where anyone can create beautiful, functional websites without writing a single line of frontend code.\",\r\n      \"image\": \"https://placehold.co/500x300\"\r\n    },\r\n    {\r\n      \"type\": \"timeline\",\r\n      \"title\": \"How It Started\",\r\n      \"items\": [\r\n        {\r\n          \"year\": \"2024\",\r\n          \"event\": \"The idea for Web4 UI was born from a simple observation — building frontend UI was too time-consuming for agile teams.\"\r\n        },\r\n        {\r\n          \"year\": \"2025\",\r\n          \"event\": \"MVP launched using GPT-4 and simple JSON inputs to generate production-ready HTML pages.\"\r\n        },\r\n        {\r\n          \"year\": \"Future\",\r\n          \"event\": \"Expanding support for custom LLMs, UI themes, and integrations with popular backend frameworks.\"\r\n        }\r\n      ]\r\n    },\r\n    {\r\n      \"type\": \"values\",\r\n      \"title\": \"What We Believe In\",\r\n      \"items\": [\r\n        {\r\n          \"icon\": \"🚀\",\r\n          \"title\": \"Innovation\",\r\n          \"description\": \"We constantly explore how AI can simplify the development process.\"\r\n        },\r\n        {\r\n          \"icon\": \"\U0001f9e0\",\r\n          \"title\": \"Simplicity\",\r\n          \"description\": \"The best tools are the ones you don’t even notice — fast, intuitive, and flexible.\"\r\n        },\r\n        {\r\n          \"icon\": \"🌍\",\r\n          \"title\": \"Accessibility\",\r\n          \"description\": \"We want great UI to be accessible to teams of all sizes, everywhere.\"\r\n        }\r\n      ]\r\n    },\r\n    {\r\n      \"type\": \"team\",\r\n      \"title\": \"The Team Behind Web4 UI\",\r\n      \"description\": \"We are a small team of developers, designers, and dreamers passionate about AI and the future of the web.\",\r\n      \"image\": \"https://placehold.co/600x300\"\r\n    },\r\n    {\r\n      \"type\": \"cta\",\r\n      \"title\": \"Ready to try Web4 UI?\",\r\n      \"subtitle\": \"Start creating amazing UIs instantly.\",\r\n      \"buttonText\": \"Get Started\",\r\n      \"buttonLink\": \"/how-it-works\"\r\n    }\r\n  ],\r\n  \"footer\": {\r\n    \"links\": [\r\n      { \"label\": \"Home\", \"url\": \"/\" },\r\n      { \"label\": \"FAQ\", \"url\": \"/faq\" },\r\n      { \"label\": \"Examples\", \"url\": \"/examples\" },\r\n      { \"label\": \"Contact\", \"url\": \"/contact\" },\r\n      { \"label\": \"Pricing\", \"url\": \"/pricing\" }\r\n    ],\r\n    \"copyright\": \"© 2025 Web4 UI. All rights reserved.\"\r\n  },\r\n  \"notes\": \"This About page should inspire confidence and introduce the vision, values, and journey of Web4 UI.\"\r\n}\r\n");

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
