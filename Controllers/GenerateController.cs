// Required namespaces
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Web4.Controllers;
[ApiController]
[Route("api/[controller]")]
public class GenerateController : BaseController
{
    //private readonly string _connectionString = "Server=DESKTOP-L0AVETU;Database=web4;User Id=sa;Password=1;TrustServerCertificate=True";
    private readonly string _connectionString = "Server=DESKTOP-01CIS7M\\MSSQL2022;Database=web4;User Id=sa;Password=1;TrustServerCertificate=True";
    private readonly string _openAiApiKey = "aa-brCPYPw4YdvShQdZ6jeuRYwFkAiyHodwocsBJVnUg54uiISt";

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] GenerationRequest request)
    {
        var generationId = Guid.NewGuid();
        var inputId = Guid.NewGuid();
        var outputId = Guid.NewGuid();
        var createdAt = DateTime.UtcNow;

        // Step 1: Save Generation and Input
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();

            using (var cmd = new SqlCommand("INSERT INTO Generations (GenerationId, UserId, CreatedAt, Status, ModelUsed) VALUES (@GenId, @UserId, @CreatedAt, 'InProgress', 'gpt-4')", conn))
            {
                cmd.Parameters.AddWithValue("@GenId", generationId);
                cmd.Parameters.AddWithValue("@UserId", request.UserId);
                cmd.Parameters.AddWithValue("@CreatedAt", createdAt);
                await cmd.ExecuteNonQueryAsync();
            }

            using (var cmd = new SqlCommand("INSERT INTO GenerationInputs (InputId, GenerationId, InputType, InputContent) VALUES (@InputId, @GenId, @Type, @Content)", conn))
            {
                cmd.Parameters.AddWithValue("@InputId", inputId);
                cmd.Parameters.AddWithValue("@GenId", generationId);
                cmd.Parameters.AddWithValue("@Type", request.InputType);
                cmd.Parameters.AddWithValue("@Content", request.Content);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        // Step 2: Generate Prompt
        var fullPrompt = "<!-- \r\nGenerate clean, elegant HTML, CSS, and JavaScript using Bootstrap.\r\nKeep all styles inline or embedded within the same file.\r\nDo not include any extra explanations — only the code.\r\n-->\n" + request.Content;

        string resultHtml = "";
        int promptTokens = 0;
        int completionTokens = 0;

        try
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_openAiApiKey}");

                var requestBody = new
                {
                    model = "gpt-4o-mini",
                    //model = "meta.llama3-3-70b-instruct-v1:0",
                    messages = new[]
                    {
                        new { role = "user", content = fullPrompt }
                    },
                    temperature = 0.7
                };

                var response = await httpClient.PostAsync(
                    "https://api.avalai.ir/v1/chat/completions",
                    //"https://api.OpenAi.com/v1/chat/completions",
                    new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
                );

                var jsonResponse = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(jsonResponse);
                resultHtml = doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();

                if (doc.RootElement.TryGetProperty("usage", out var usage))
                {
                    promptTokens = usage.GetProperty("prompt_tokens").GetInt32();
                    completionTokens = usage.GetProperty("completion_tokens").GetInt32();
                }
            }

            // Save output
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                using (var cmd = new SqlCommand("INSERT INTO GenerationOutputs (OutputId, GenerationId, HtmlContent, TokenUsagePrompt, TokenUsageCompletion) VALUES (@OutputId, @GenId, @Html, @PromptTokens, @CompletionTokens)", conn))
                {
                    cmd.Parameters.AddWithValue("@OutputId", outputId);
                    cmd.Parameters.AddWithValue("@GenId", generationId);
                    cmd.Parameters.AddWithValue("@Html", resultHtml);
                    cmd.Parameters.AddWithValue("@PromptTokens", promptTokens);
                    cmd.Parameters.AddWithValue("@CompletionTokens", completionTokens);
                    await cmd.ExecuteNonQueryAsync();
                }

                using (var cmd = new SqlCommand("UPDATE Generations SET Status = 'Success' WHERE GenerationId = @GenId", conn))
                {
                    cmd.Parameters.AddWithValue("@GenId", generationId);
                    await cmd.ExecuteNonQueryAsync();
                }
            }

            return Ok(new { generationId, html = resultHtml });
        }
        catch (Exception ex)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (var cmd = new SqlCommand("INSERT INTO ErrorLogs (GenerationId, ErrorMessage, StackTrace) VALUES (@GenId, @Message, @Trace)", conn))
                {
                    cmd.Parameters.AddWithValue("@GenId", generationId);
                    cmd.Parameters.AddWithValue("@Message", ex.Message);
                    cmd.Parameters.AddWithValue("@Trace", ex.StackTrace ?? "");
                    await cmd.ExecuteNonQueryAsync();
                }

                using (var cmd = new SqlCommand("UPDATE Generations SET Status = 'Error' WHERE GenerationId = @GenId", conn))
                {
                    cmd.Parameters.AddWithValue("@GenId", generationId);
                    await cmd.ExecuteNonQueryAsync();
                }
            }

            return StatusCode(500, new { error = "Error in generate HTML." });
        }
    }
}

// کلاس مدل ورودی
public class GenerationRequest
{
    public Guid UserId { get; set; }
    public string InputType { get; set; } // "Text" یا "JSON"
    public string Content { get; set; }
}
