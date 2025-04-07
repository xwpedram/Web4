﻿using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using Web4.Controllers;

[ApiController]
[Route("[controller]")]
public class ExamplesController : BaseController
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ExamplesController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        string content = "{\r\n  \"pageTitle\": \"Examples - What You Can Build with Web4 UI\",\r\n  \"sections\": [\r\n    {\r\n      \"type\": \"hero\",\r\n      \"title\": \"Explore Web Pages Created with AI\",\r\n      \"subtitle\": \"From dashboards to product pages — discover what’s possible with just a few words.\",\r\n      \"image\": \"https://placehold.co/600x350\"\r\n    },\r\n    {\r\n      \"type\": \"gallery\",\r\n      \"title\": \"Popular Example Pages\",\r\n      \"items\": [\r\n        {\r\n          \"title\": \"Product Showcase Page\",\r\n          \"description\": \"A modern, responsive product page with image, title, price, description and CTA.\",\r\n          \"image\": \"https://placehold.co/400x250\",\r\n          \"link\": \"/examples/product-showcase\"\r\n        },\r\n        {\r\n          \"title\": \"Dashboard with Charts\",\r\n          \"description\": \"A professional dashboard layout using Chart.js to visualize user and sales data.\",\r\n          \"image\": \"https://placehold.co/400x250\",\r\n          \"link\": \"/examples/dashboard\"\r\n        },\r\n        {\r\n          \"title\": \"Contact Form Page\",\r\n          \"description\": \"A clean contact form with name, email, subject, and message fields — fully responsive.\",\r\n          \"image\": \"https://placehold.co/400x250\",\r\n          \"link\": \"/examples/contact-form\"\r\n        },\r\n        {\r\n          \"title\": \"Pricing Page\",\r\n          \"description\": \"A pricing comparison table with 3 plans and feature highlights.\",\r\n          \"image\": \"https://placehold.co/400x250\",\r\n          \"link\": \"/examples/pricing-page\"\r\n        },\r\n        {\r\n          \"title\": \"Landing Page\",\r\n          \"description\": \"A hero-driven landing page for startups with bold call-to-action and value propositions.\",\r\n          \"image\": \"https://placehold.co/400x250\",\r\n          \"link\": \"/examples/landing\"\r\n        }\r\n      ]\r\n    },\r\n    {\r\n      \"type\": \"cta\",\r\n      \"title\": \"Ready to Build Your Own?\",\r\n      \"subtitle\": \"Try it free and create your first page in less than a minute.\",\r\n      \"buttonText\": \"Generate My Page\",\r\n      \"buttonLink\": \"/how-it-works\"\r\n    }\r\n  ],\r\n  \"footer\": {\r\n    \"links\": [\r\n      { \"label\": \"Home\", \"url\": \"/\" },\r\n      { \"label\": \"About\", \"url\": \"/about\" },\r\n      { \"label\": \"How It Works\", \"url\": \"/how-it-works\" },\r\n      { \"label\": \"FAQ\", \"url\": \"/faq\" },\r\n      { \"label\": \"Contact\", \"url\": \"/contact\" }\r\n    ],\r\n    \"copyright\": \"© 2025 Web4 UI. All rights reserved.\"\r\n  },\r\n  \"notes\": \"This page showcases multiple sample pages generated by the platform, each linking to a live demo or preview, giving users inspiration and trust.\"\r\n}\r\n";

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
