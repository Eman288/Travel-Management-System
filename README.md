# Travel Management System

## Table of Contents

- [Introduction](#introduction)
- [Features](#features)
- [Technologies](#technologies)
- [Installation](#installation)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)

## Introduction

Welcome to the Travel Management System! This project is designed to manage and streamline travel arrangements, including booking flights, accommodations, and other travel-related activities. The system aims to provide an efficient way to handle travel logistics for users.

## Features

- User authentication and authorization
- Booking and managing travel itineraries
- Searching and filtering travel options
- Payment processing
- Admin panel for managing users and travel options
- Responsive design for mobile and desktop users

## Technologies

- **Frontend:** HTML, CSS, JavaScript, Bootstrap
- **Backend:** .NET Core, ASP.NET MVC, Entity Framework
- **Database:** SQL Server
- **Authentication:** Identity Framework

## Installation

### Prerequisites

- [.NET Core SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

### Steps

1. **Clone the repository**
    ```bash
    git clone https://github.com/Eman288/Travel-Management-System.git
    cd Travel-Management-System
    ```

2. **Setup the database**
    - Update the connection string in `appsettings.json` to match your SQL Server configuration.
    - Run the following commands to create the database and apply migrations:
        ```bash
        dotnet ef database update
        ```

3. **Run the application**
    ```bash
    dotnet run
    ```

## Usage

1. **Register and log in**
    - Create a new account or log in with existing credentials.

2. **Book travel**
    - Search for available travel options and book your preferred itinerary.

3. **Manage bookings**
    - View and manage your bookings from the user dashboard.

4. **Admin panel**
    - If you are an admin, access the admin panel to manage users and travel options.

## Contributing

Contributions are welcome! Please follow these steps to contribute:

1. **Fork the repository**
2. **Create a new branch**
    ```bash
    git checkout -b feature/your-feature-name
    ```
3. **Make your changes**
4. **Commit your changes**
    ```bash
    git commit -m "Add feature: your feature name"
    ```
5. **Push to your branch**
    ```bash
    git push origin feature/your-feature-name
    ```
6. **Create a pull request**

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
