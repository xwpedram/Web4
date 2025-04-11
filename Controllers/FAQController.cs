using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using Web4.Controllers;

[ApiController]
[Route("[controller]")]
public class FAQController : BaseController
{
    private readonly IHttpClientFactory _httpClientFactory;

    public FAQController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        string content = CombinePrompt("{\r\n  \"pageTitle\": \"Frequently Asked Questions - Web4 UI\",\r\n  \"sections\": [\r\n    {\r\n      \"type\": \"hero\",\r\n      \"title\": \"Got Questions? We’ve Got Answers.\",\r\n      \"subtitle\": \"Find quick answers to the most common questions about Web4 UI.\",\r\n      \"image\": \"https://placehold.co/600x300\"\r\n    },\r\n    {\r\n      \"type\": \"faq\",\r\n      \"title\": \"General Questions\",\r\n      \"items\": [\r\n        {\r\n          \"question\": \"What is Web4 UI?\",\r\n          \"answer\": \"Web4 UI is a tool that uses AI to generate HTML, CSS, and JavaScript web pages based on your natural language input or structured JSON.\"\r\n        },\r\n        {\r\n          \"question\": \"Do I need to know coding to use it?\",\r\n          \"answer\": \"Not at all! Just describe the page you want, and Web4 UI will generate the full code for you.\"\r\n        },\r\n        {\r\n          \"question\": \"How many pages can I create for free?\",\r\n          \"answer\": \"You can generate up to 20 pages for free. After that, you can subscribe or connect your own OpenAI token.\"\r\n        },\r\n        {\r\n          \"question\": \"What kind of pages can I generate?\",\r\n          \"answer\": \"Anything from product showcases, dashboards, contact forms, pricing pages, landing pages, and more.\"\r\n        },\r\n        {\r\n          \"question\": \"Can I customize the generated HTML?\",\r\n          \"answer\": \"Yes, the output is clean and editable. You can use it in any framework or editor of your choice.\"\r\n        },\r\n        {\r\n          \"question\": \"Is the generated code responsive?\",\r\n          \"answer\": \"Yes. We prompt the AI to generate mobile-friendly, responsive layouts by default.\"\r\n        },\r\n        {\r\n          \"question\": \"Can I use my own AI model?\",\r\n          \"answer\": \"Yes. You can either bring your own OpenAI token or connect your own LLM in future versions.\"\r\n        },\r\n        {\r\n          \"question\": \"Is this safe for production use?\",\r\n          \"answer\": \"Absolutely. But always review and test the output before deployment. You can also filter or post-process the generated code for security.\"\r\n        }\r\n      ]\r\n    },\r\n    {\r\n      \"type\": \"cta\",\r\n      \"title\": \"Still have questions?\",\r\n      \"subtitle\": \"We’d love to help you out. Contact us anytime.\",\r\n      \"buttonText\": \"Contact Us\",\r\n      \"buttonLink\": \"/contact\"\r\n    }\r\n  ],\r\n  \"footer\": {\r\n    \"links\": [\r\n      { \"label\": \"Home\", \"url\": \"/\" },\r\n      { \"label\": \"About\", \"url\": \"/about\" },\r\n      { \"label\": \"How It Works\", \"url\": \"/how-it-works\" },\r\n      { \"label\": \"Examples\", \"url\": \"/examples\" },\r\n      { \"label\": \"Contact\", \"url\": \"/contact\" }\r\n    ],\r\n    \"copyright\": \"© 2025 Web4 UI. All rights reserved.\"\r\n  },\r\n  \"notes\": \"This FAQ page is designed to give quick, accurate, and helpful answers to potential users, reducing hesitation and improving trust in the product.\"\r\n}\r\n");

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
