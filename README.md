# Email Sending Service

## Overview

This project implements an email sending service that meets specific client requirements. The service is designed to be used in a high-volume application where email sending should not block the main application workflow. It includes retry logic, logging, and configurable settings.

## Features

- **Asynchronous Email Sending**: Sends emails without blocking the main application thread.
- **Retry Logic**: Retries sending an email up to 3 times if it fails, with a delay between attempts.
- **Logging**: Logs email sending attempts, including sender, recipient, subject, body, and status.
- **Configuration**: Stores email settings in `appsettings.json` for flexibility.
- **API Integration**: Provides an API to send emails via HTTP requests.

  ### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) 6.0 or later
- An SMTP server for sending emails( I used Ethereal (https://ethereal.email/))

### Configuration

1. **Clone the Repository:**

   ```bash
   git clone https://github.com/Czimerin/Clarity-Ventures-Internal-Assignment.git

2. **Configure SMTP Settingsz**

  Update the `appsettings.json` file with your SMTP server settings. This file is used to configure the email settings required by the `EmailSender` service.

  Open `appsettings.json` and replace the existing content with the following, updating the placeholder values with your SMTP server information:

  ```json
  {
    "MailSettings": {
      "Host": "smtp.example.com",
      "Port": 587,
      "UserName": "username@example.com",
      "Password": "password",
      "Name": "Example Name"
    }
  }
  ```
  An example would be :

  ```json

  "MailSettings": {
      "Host": "smtp.ethereal.email",
      "Port": 587,
      "Name": "Alfonzo Kreiger",
      "UserName": "alfonzo.kreiger80@ethereal.email",
      "Password": "***************"
  }
  ```
3. **Build and Run the Application**

  Build the Application

  Navigate to the Application project directory and build the application using the .NET CLI:
  

  ```bash
  cd application
  dotnet build
  ```

  Then run it:

  ```bash

  dotnet run
  ```
4. **API Testing with Postman**

  To test the email sending functionality of the API, you can use Postman. Follow these steps to send a request to the API:

  API Endpoint

  URL: http://localhost:5155/api/email/send

  Method: POST
  Request Headers

    Content-Type: application/json

  Request Body

  Provide the request body in JSON format. Here's an example of a request body you can use:

  ```json
  {
    "toRecipients": [
      {
        "recipientName": "Alfonzo Kreiger",
        "recipientAddress": "alfonzo.kreiger80@ethereal.email"
      }
    ],
    "subject": "Hello World",
    "body": "This is a test email.",
    "ccRecipients": null,
    "attachments": null
  }

  ```



