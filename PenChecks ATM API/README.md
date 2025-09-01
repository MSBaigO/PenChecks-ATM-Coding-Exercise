# PenChecks ATM Solution

This solution contains an ATM API, a web client, and a dedicated test project.

---

## Technical Details

**Framework:**  
- .NET 8.0  

**API Packages:**  
- [Swashbuckle.AspNetCore](https://www.nuget.org/packages/Swashbuckle.AspNetCore) – Used for generating the Swagger UI for API documentation and testing.  

**Testing Packages:**  
- [xunit](https://xunit.net/)  
- [Microsoft.NET.Test.Sdk](https://www.nuget.org/packages/Microsoft.NET.Test.Sdk)  

**Web Client Tech Stack:**  
- HTML – Used for the structure of the web client (index.html).  
- CSS – Provides styling (style.css).  
- JavaScript – Implements client-side functionality (atm.js).  

---

## Getting Started

To run the solution, follow these steps:

1. Open the **`PenChecks_ATM_API.sln`** file in Visual Studio.

---

## Launching the API

1. In the Solution Explorer, right-click on the **`PenChecks_ATM_API`** project and select **Set as Startup Project**.  
2. Press **F5** to run the API.  
   - A browser will automatically open to the **Swagger UI** page.

---

## Launching the Web Client

1. Navigate to the **`ATM Web Client`** folder in your file system.  
2. Double-click the **`index.html`** file to open the web client in your default browser.

---

## Running Tests

To run the unit tests for the API's business logic, use the **Test Explorer**:

1. Go to **Test > Test Explorer** in the Visual Studio menu.  
2. Click **Run All** to execute all tests in the **`PenChecks_ATM_API.Tests`** project.  

---

## Project Structure

### PenChecks ATM API

PenChecks_ATM_API/
│── Controllers/
│ └── ATMController.cs # Handles API requests for ATM operations
│
│── Core/
│ ├── DTOs/
│ │ ├── ATMRequestDTOs.cs # Data Transfer Objects for incoming requests
│ │ └── ATMResponseDTOs.cs # Data Transfer Objects for API responses
│ │
│ ├── Interfaces/
│ │ └── IATMService.cs # Service interface definition
│ │
│ ├── Models/
│ │ ├── Account.cs # Represents a bank account
│ │ └── Transaction.cs # Represents a transaction record
│ │
│ └── Services/
│ └── ATMService.cs # Implements ATM operations logic
│
│── appsettings.json # Application configuration
│── Program.cs # Application entry point
│── README.md # Documentation

### PenChecks ATM API Tests

PenChecks_ATM_API.Tests/
│── ATMServiceTests.cs # Unit tests for ATM service logic

### ATM Web Client

ATM Web Client/
│── index.html # Main entry point for the web client
│── style.css # Styling for the web client
│── atm.js # JavaScript logic for ATM operations and UI interactions

---