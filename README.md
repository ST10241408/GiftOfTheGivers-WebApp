Gift of the Givers Web Application

Default Admin Login Credentials

Email: admin@admin.com

Password: Test1234,

> These credentials are automatically seeded into the database when the application starts. Use them to log in as an Admin and access the Admin Dashboard.




---

Project Overview

Gift of the Givers is a humanitarian web application designed to assist communities during disasters by enabling secure user registration, disaster reporting, resource donations, and volunteer management. The application supports role-based dashboards for Users and Admins, ensuring transparency, efficiency, and organized disaster relief operations.


---

Features Implemented

Feature	Description	Marks

User Registration and Login	Secure authentication system using ASP.NET Identity with role-based access (Admin/User). Users can manage their profiles.	15
Disaster Incident Reporting	Users can submit incident reports, including disaster type, location, and dates. Admins can view all reported incidents.	15
Resource Donation	Users can donate goods or money. Donations are tracked and displayed for admin management.	10
Volunteer Management	Volunteers can register, browse available tasks, and be assigned by admins. Tracks contributions and task statuses.	10
Git Repository & Collaboration	Source code managed in Azure Repos with a Gitflow branching strategy. Pull requests and descriptive commits implemented.	10
Azure Pipelines	CI/CD pipeline configured to build, test, and deploy the application automatically.	10
Source Code & Documentation	Complete, well-organized source code with project documentation.	10
Screenshots & Demo	Screenshots of the application interface and deployed environment.	10



---

Technologies Used

Backend: ASP.NET Core MVC, C#

Database: SQL Server (Entity Framework Core)

Authentication: ASP.NET Identity with Roles

Frontend: Razor Pages, Bootstrap 5

Version Control: Git (Azure Repos / GitHub)

CI/CD: Azure Pipelines

Hosting/Deployment: Azure App Service (optional)



---

Getting Started

Prerequisites

Visual Studio 2022 or later

.NET SDK 7.x

SQL Server / LocalDB

Azure DevOps account (for repo and pipeline)


Installation

1. Clone the repository:

git clone https://dev.azure.com/<your-organization>/<your-project>/_git/<repo-name>
cd GiftOfTheGiversWebApp


2. Update appsettings.json with your database connection string:

"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=GiftOfTheGiversDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}


3. Apply migrations:

dotnet ef database update


4. Run the application:

dotnet run


5. Access the application in your browser at:
👉 https://localhost:7188




---

User Roles & Access

Role	Capabilities

Admin	View/manage all donations, disasters, and volunteers; assign tasks; manage users
User	Submit disaster reports, donate goods/money, register as volunteer
Guest/Public	View public information about mission and donation options



---

Screenshots

User Dashboard – Displays user’s donations, disaster reports, and volunteer commitments.

Admin Dashboard – Provides overview of all users, donations, and volunteer assignments.

Disaster Report Form – Allows users to submit incident details (type, location, date).

Donation Submission – For donating money or goods.

Volunteer Assignment – Admins assign volunteers to specific relief tasks.



---

Azure Pipelines Configuration

Pipeline YAML: .azure-pipelines.yml

Build Trigger

On push to develop branch


Tasks Included:

1. Restore NuGet packages


2. Build solution


3. Run tests


4. Publish build artifacts


5. Deploy to Azure App Service (optional)



⚠️ Note: Due to Azure DevOps free tier restrictions (parallel job limitations), full automated CI/CD could not run without approval. However, the pipeline has been fully configured with YAML and tested locally.


---

Git Repository Structure

/GiftOfTheGiversWebApp
│
├─ Controllers/
├─ Models/
├─ Views/
├─ Data/
├─ wwwroot/
├─ Migrations/
├─ .azure-pipelines.yml
├─ appsettings.json
├─ Program.cs
└─ README.md


---

Future Improvements

Add an analytics dashboard for advanced reporting (admin insights).

Mobile-friendly PWA version for easier accessibility.

SMS/email alerts for urgent disaster notifications.



---

Author

👤 Phathisa Ndaliso
Final Year Software Development Student – Rosebank College, Johannesburg

GitHub: GiftOfTheGivers-WebApp

Azure DevOps: ST10241408 – GiftOfTheGivers-WebApp



