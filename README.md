# Gift of the Givers Foundation - ASP.NET Web Application

This web application is designed for the **Gift of the Givers Foundation** to streamline the management of donations, disasters, and volunteers. It provides a user-friendly interface for individuals to contribute, register as volunteers, and report disasters. This README file will guide you through setting up and running the project, as well as understanding the key features of the system.

---

## Features

1. **Home Page:**

   - Displays an overview of the foundation's mission and the latest donation statistics (goods and monetary donations).
   - Provides easy navigation to other sections of the web app.

2. **User Registration & Login:**

   - New users can register by providing an email, username, and password.
   - Registered users can log in to access additional features, such as managing volunteers and donating.

3. **Goods Donation:**

   - Users can submit donations of physical goods via the **Goods Donations** section.
   - Includes a simple form to describe the donated items.

4. **Money Donation:**

   - Users can contribute financially through the **Money Donations** section.
   - Users can specify the donation amount and opt to remain anonymous.

5. **Disaster Logging:**

   - Volunteers and users can report disasters, including the type, location, and other necessary information.

6. **Volunteer Management:**
   - Admins can register and manage volunteers.
   - Volunteers are added with basic contact information for communication and scheduling purposes.

---

## Installation

### Prerequisites

Before you begin, ensure you have the following installed:

- **Visual Studio** with ASP.NET Core support
- **.NET Core SDK**
- **SQL Server** for the database
- A web browser for testing the application

### Steps

1. **Clone the Repository:**

   - Clone this repository to your local machine using:
     ```bash
     git clone https://ST10102524@dev.azure.com/ST10102524/Gift%20of%20the%20Givers%20Foundation%20Web%20Application/_git/Gift%20of%20the%20Givers%20Foundation%20Web%20Application
     ```

2. **Open in Visual Studio:**

   - Open the project folder in Visual Studio.
   - Restore NuGet packages to ensure all necessary dependencies are installed.

3. **Update Database Connection:**

   - Open the `appsettings.json` file and update the connection string to point to your SQL Server instance:
     ```json
     "ConnectionStrings": {
       "DefaultConnection": "Server=tcp:manp.database.windows.net,1433;Initial Catalog=man;Persist Security Info=False;User ID=st10102524;Password=Kg@ph0la45;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
     }
     ```

4. **Run Database Migrations:**

   - Run the following commands in the **Package Manager Console** to apply database migrations:
     ```bash
     update-database
     ```

5. **Run the Application:**
   - Click the **green play button** in Visual Studio to build and run the project.
   - The application will launch in your default web browser.

---

## Usage

### Navigation Overview

Once the application is running, the following sections are available through the **navigation bar**:

- **Home:** Displays a summary of donation statistics and disaster information.
- **Goods Donations:** Submit a new donation of goods.
- **Money Donations:** Submit a financial donation with optional anonymity.
- **Disasters:** Log a new disaster by filling in relevant details.
- **Volunteer Management:** Register new volunteers for disaster response efforts.

### Registering a User

1. Click on the **Register** button in the top-right corner.
2. Fill in the required details (email, username, password).
3. Click **Register** to create a new account.

### Making Donations

1. Navigate to the **Goods Donations** or **Money Donations** section via the navigation bar.
2. For **Goods Donations**, click on **Create New** and provide the details of your donation.
3. For **Money Donations**, fill in the date and donation amount, and choose whether to remain anonymous.

### Logging Disasters

1. Navigate to the **Disasters** section.
2. Click **Create New**, fill in the disaster details, and submit the form to log the disaster.

### Managing Volunteers

1. After logging in, access the **Volunteer Management** section.
2. Click on **Register New Volunteer** and fill out the volunteer's information (name, email, phone number).
3. Registered volunteers can be assigned tasks and scheduled for disaster response activities.

---

## Technologies Used

- **ASP.NET Core** for the web application framework.
- **Entity Framework Core** for database management and migrations.
- **SQL Server** as the relational database.
- **CSS** for responsive UI components.
- **Razor Pages** for the front-end templating engine.

---

## Contribution

If you'd like to contribute to this project:

1. Fork the repository.
2. Create a new branch for your feature or bug fix:
   ```bash
   git checkout -b feature-name
   ```
