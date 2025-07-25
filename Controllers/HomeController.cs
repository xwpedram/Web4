using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using Web4.Controllers;

[ApiController]
[Route("[controller]")]
public class HomeController : BaseController
{
    private readonly IHttpClientFactory _httpClientFactory;
    string route = "Home";
    public HomeController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        string content = CombinePrompt("{\r\n  \"pageTitle\": \"Web4 UI - Build Web Pages with AI in Seconds\",\r\n  \"sections\": [\r\n    {\r\n      \"type\": \"hero\",\r\n      \"title\": \"Welcome to Web4 UI\",\r\n      \"subtitle\": \"Create beautiful web pages with a single prompt.\",\r\n      \"image\": \"https://placehold.co/600x400\",\r\n      \"ctaText\": \"Get Started Free\",\r\n      \"ctaLink\": \"/how-it-works\"\r\n    },\r\n    {\r\n      \"type\": \"features\",\r\n      \"title\": \"Why Web4 UI?\",\r\n      \"items\": [\r\n        {\r\n          \"icon\": \"⚡️\",\r\n          \"title\": \"Lightning Fast\",\r\n          \"description\": \"Generate full HTML pages in seconds from a simple description.\"\r\n        },\r\n        {\r\n          \"icon\": \"🎨\",\r\n          \"title\": \"Beautiful & Responsive\",\r\n          \"description\": \"Your content, our design. Responsive layouts, forms, charts, and more.\"\r\n        },\r\n        {\r\n          \"icon\": \"🤖\",\r\n          \"title\": \"AI-Powered by LLM\",\r\n          \"description\": \"Backed by GPT-4 or your own custom model for dynamic UI generation.\"\r\n        }\r\n      ]\r\n    },\r\n    {\r\n      \"type\": \"how-it-works\",\r\n      \"title\": \"How Does It Work?\",\r\n      \"description\": \"You describe the page. We build it. It’s that simple.\",\r\n      \"image\": \"https://placehold.co/600x300\",\r\n      \"linkText\": \"Learn More\",\r\n      \"linkUrl\": \"/how-it-works\"\r\n    },\r\n    {\r\n      \"type\": \"examples\",\r\n      \"title\": \"See It In Action\",\r\n      \"description\": \"Here are some pages created with Web4 UI:\",\r\n      \"items\": [\r\n        {\r\n          \"title\": \"Product Showcase Page\",\r\n          \"image\": \"https://placehold.co/300x200\",\r\n          \"link\": \"/examples\"\r\n        },\r\n        {\r\n          \"title\": \"Analytics Dashboard\",\r\n          \"image\": \"https://placehold.co/300x200\",\r\n          \"link\": \"/examples\"\r\n        },\r\n        {\r\n          \"title\": \"Contact Form\",\r\n          \"image\": \"https://placehold.co/300x200\",\r\n          \"link\": \"/examples\"\r\n        }\r\n      ]\r\n    },\r\n    {\r\n      \"type\": \"cta\",\r\n      \"title\": \"Start Creating Today!\",\r\n      \"subtitle\": \"First 20 pages are completely free.\",\r\n      \"buttonText\": \"Generate Your Page\",\r\n      \"buttonLink\": \"/how-it-works\"\r\n    }\r\n  ],\r\n  \"footer\": {\r\n    \"links\": [\r\n      { \"label\": \"About\", \"url\": \"/about\" },\r\n      { \"label\": \"FAQ\", \"url\": \"/faq\" },\r\n      { \"label\": \"Examples\", \"url\": \"/examples\" },\r\n      { \"label\": \"Contact\", \"url\": \"/contact\" },\r\n      { \"label\": \"Pricing\", \"url\": \"/pricing\" }\r\n    ],\r\n    \"copyright\": \"© 2025 Web4 UI. All rights reserved.\"\r\n  },\r\n  \"notes\": \"This is the main landing page for the service. The design should be modern, elegant, and convey trust. Images are placeholders to be replaced later.\"\r\n}\r\n");

        var requestBody = new
        {
            userId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"),
            inputType = "Text",
            content
        };

        var httpClient = _httpClientFactory.CreateClient();

        var response = await httpClient.PostAsync(
             mainAddress + "api/generate",
            new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
        );

        var jsonResponse = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            return Content($"<h2>Error to generate </h2><pre>{jsonResponse}</pre>", "text/html");

        var doc = JsonDocument.Parse(jsonResponse);
        var html = doc.RootElement.GetProperty("html").GetString();
        html = StripCodeBlock(html);
        return Content(html, "text/html", Encoding.UTF8);
    }
}
