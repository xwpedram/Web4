# Web4 UI ✨  
**Generate Beautiful Web Pages with Just One Prompt — Powered by AI**

Web4 UI is an experimental open-source project that demonstrates how AI can be used to instantly generate clean, responsive, and production-ready HTML/CSS/JS pages based on a simple text or JSON input.

With a flexible backend and AI-driven frontend generation, you can turn your ideas into code — without touching a UI builder.

---

## 🚀 Features

- 🔁 Generate dynamic HTML pages via API
- 💡 Uses OpenAI (GPT-4 or others) to understand page structure
- 🎨 Style and structure generated on-the-fly with clean inline CSS
- ⚙️ Fully customizable prompts per page type (Home, FAQ, Contact, etc.)
- 🔐 Clean architecture using .NET 8 Web API with ADO.NET for high performance

---

## 📦 What's Inside?

- `GenerateController.cs`  
  Core API controller that takes user input and returns generated HTML using OpenAI

- `HelloController.cs` (sample)  
  Example of how to dynamically create pages using a simple route and a pre-defined prompt

- `BaseController.cs`  
  Defines a base prompt for consistent UI style across all generated pages

- `appsettings.json`  
  Includes connection string configuration and other settings

---

## 🧪 How It Works

1. Send a POST request to `/api/generate`  
2. Include a JSON body with:
    ```json
    {
      "userId": "your-guid",
      "inputType": "Text" or "Json",
      "content": "Your page prompt or structured JSON"
    }
    ```
3. The system adds a default base prompt and sends it to OpenAI API  
4. The generated HTML is returned and stored in the database  
5. You can also display it directly as a web page

---

## 🔧 How to Run Locally

```bash
git clone https://github.com/your-username/web4-ui.git
cd web4-ui
dotnet restore
dotnet run
