using Microsoft.AspNetCore.Mvc;

namespace Web4.Controllers
{
    public class BaseController : Controller
    {
        public string mainAddress = "https://localhost:7121/";
        public IActionResult Index()
        {
            return View();
        }

        protected readonly string BasePrompt =
        @"<!-- 
        You are a professional UI designer and frontend developer.
        Use clean, semantic HTML and elegant in-page CSS.
        Do not use external libraries or frameworks.
        Avoid unnecessary complexity. Keep things minimal, beautiful, and responsive in dark moode theme.
        Generate only HTML/CSS/JS. Do not explain anything.
        Use modern design principles (spacing, font balance, contrast).
        -->";

        protected string CombinePrompt(string userPrompt)
        {
            return $"{BasePrompt}\n{userPrompt}";
        }

        protected string StripCodeBlock(string html)
        {
            if (html.StartsWith("```"))
            {
                var newlineIndex = html.IndexOf('\n');
                if (newlineIndex >= 0)
                {
                    html = html[(newlineIndex + 1)..];
                }
            }

            if (html.EndsWith("```"))
            {
                html = html.Substring(0, html.Length - 3);
            }

            return html.Trim('\r', '\n');
        }
    }
}
