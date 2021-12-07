# Documentation

## Setup

### Database
This project uses [MS SQL server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads), but can work with another DBMS after some modifications. Database connection string can be changed in the [appsettings.json](todo-backend/api-controllers/appsettings.json) file.
1. Create a new database named TodoAppDB in localdb
2. Run the [initializer script](todo-backend/db/db_init.sql) on the created database

### Backend
Prerequisites: [.NET 5 SDK](https://dotnet.microsoft.com/download/dotnet/5.0), already existing database (see the [database section](#database) to set it up)
1. Open terminal in todo-backend folder
1. Run `dotnet run --project api-controllers`
1. The web server will start after the build finishes

### Frontend
[node.js](https://nodejs.org/en/download/) is required to run the frontend
1. Open terminal in web-frontend directory
1. Run `npm install`
1. Run `npm start`
1. The web application will start in your browser

