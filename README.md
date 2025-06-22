# Roast My Resume

Roast My Resume is a fun and interactive web app that lets users upload their resume and receive a humorous AI-powered roast using LLMs like **LLaMA 3** (via Groq or alternatives). Whether you're prepping for interviews or just want a laugh, this app brings lighthearted critique to your CV.

---

## 🚀 Live Demo

📍 **Frontend**: [https://ghamani121.github.io/Resume-Roaster/#/](https://ghamani121.github.io/Resume-Roaster/#/)  
🧠 **Powered by**: LLaMA 3-70B (via Groq API or alternatives)

---

## 🛠️ Tech Stack

### Frontend:
- Angular 17 (Standalone Components)
- Hosted on GitHub Pages

### Backend:
- ASP.NET Core 8 Web API
- Dockerized and deployed on Azure App Service
- PDF & DOCX parsing using:
  - `PdfPig` (for PDFs)
  - `OpenXML` (for DOCX)

### LLM Integration:
- Currently using: `llama3-70b-8192` (via Groq)
- Fallbacks: Support for OpenRouter or other LLM providers planned

---

## 📸 Screenshots

![image1](https://github.com/user-attachments/assets/21315354-6b8b-449c-8a32-20d6cc5fb68b)  
![image2](https://github.com/user-attachments/assets/0e6f1173-7dfd-4659-b228-fa4f4105bc35)  
![image3](https://github.com/user-attachments/assets/112d1f5d-1358-42e9-ae5b-0f93bbb9dadf)

---

## 📦 Features

- Upload your resume as PDF or DOCX  
- Backend parses your resume using C#  
- Resume content is sent to a humorous AI model  
- Results are displayed with optional toast or animation  
- Fully deployed frontend + backend — no local setup needed  

---

## 🚧 Local Development

### Frontend (Angular):
```bash
git clone https://github.com/ghamani121/Resume-Roaster.git
cd Resume-Roaster
npm install
ng serve
```

### Backend (ASP.NET Core):

```bash
cd Resume-Roaster/backend/backend
dotnet build
dotnet run
```

🌐 Deployment
Frontend: GitHub Pages via Angular CLI ng deploy
Backend: Azure App Service (Docker container via Azure portal)
