# Timesheet Reports Generator

A comprehensive solution for generating and managing timesheet reports with a .NET backend and Chrome Extension integration.

## 🚀 Features

- Automated timesheet report generation
- Chrome Extension for enhanced user interaction
- RESTful API endpoints for data management
- Database integration with Entity Framework Core
- Customizable report templates
- Docker support for easy deployment

## 📋 Prerequisites

- .NET 6.0 or later
- Docker (optional, for containerized deployment)
- Chromium-based browser (for extension functionality)
- MySQL Database (or any EF-compatible database)

## 🛠️ Project Structure

```
TimesheetReportGenerator/
├── ChromeExtension/       # Browser extension for enhanced functionality
├── Controllers/           # API endpoints and request handling
├── Models/               # Data models and entities
├── Services/             # Business logic and service layer
├── Repositories/         # Data access layer
├── Templates/           # Report templates
├── ViewModels/          # Data transfer objects
├── Migrations/          # Database migration files
└── Properties/          # Project properties and launch settings
```

## 🚀 Getting Started

### Installation

1. Clone the repository:
```bash
git clone https://github.com/yourusername/timesheet-reports-generator.git
cd timesheet-reports-generator
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Update the database:
```bash
dotnet ef database update
```

### Configuration

1. Update the connection string in `appsettings.json`
2. Configure AzureDevOps section in `appsettings.json`
3. For development, modify `appsettings.Development.json` as needed

Example of AzureDevOps settings
```json
{
  "AzureDevOps": {
    "OrganizationUrl": "https://dev.azure.com/YourOrganization",
    "ProjectName": "YourProject",
    "Team": "YourTeam",
    "PersonalAccessToken": "your_personal_access_token_here"
  }
}
```

### Running the Application

#### Using .NET CLI:
```bash
dotnet run
```

#### Using Docker:
```bash
docker build -t timesheet-generator .
docker run -p 8080:80 timesheet-generator
```

## 🔧 Chrome Extension

The Chrome Extension component provides additional functionality for interacting with the timesheet system directly from your browser.

To install the extension:
1. Open Chrome
2. Navigate to `chrome://extensions/`
3. Enable Developer Mode
4. Load unpacked extension from the `ChromeExtension` directory

## 📝 Database Migrations

For database updates and migrations, refer to `HowToMigrate.txt` in the project root.

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Open a Pull Request

## 📄 License

This project is licensed under the terms found in the LICENSE file (MIT) in the root directory.

## 🆘 Support

For support and questions, please open an issue in the repository. 