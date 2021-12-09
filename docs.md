# Documentation

## Setup

### Database
This project uses [MS SQL server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) but can work with another DBMS after some modifications. The database connection string can be changed in the [appsettings.json](todo-backend/api-controllers/appsettings.json) file.
1. Create a new database named TodoAppDB in localdb
1. Run the [initializer script](todo-backend/db/db_init.sql) on the created database to add the required tables
1. Run [this](todo-backend/db/preserve-item-order_trigger.sql) script to add the order preservation trigger to the database (this trigger is written in T-SQL, so it requires using MS SQL Server)

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

To use the frontend, backend services should be running with a correctly set up database.

## REST API
By default, the backend server is running on port 5000 at localhost, listening for HTTP requests, and port 5001 for HTTPS. To access the services of the backend running locally, use http://localhost:5000 as the base URL.

A DTO is used to transfer todo items between the client and the server. The JSON objects representing these items have the following structure:  

    {
        "id": 1,
        "title": "name",
        "description": "desc",
        "deadline": "2000-02-02T20:00:00.000Z",
        "orderNumber": 1,
        "categoryName": "In progress"
    }

The value of *categoryName* must match with one of the categories stored in a separate table (*Categories*) of the database. When sending data to the server, the capitalization of the first letter is not important.  
The date string value in the *deadline* property is using the ISO-8601 standard for date formatting (YYYY-MM-DDTHH:mm:ss.sssZ).  
Some properties are optional when sending objects to the server. These are:
- *description*: Initialized to an empty string if omitted.
- *deadline*: Initialized to null if omitted.
- *orderNumber*: Only optional when inserting a new item. See the section about [POST](#post) requests.

### GET
#### Get the list of all todos:
`GET /todos`  
The server returns a list of all the todo items found in the database.
The list is represented with a JSON array.

#### Get a specific todo item by ID:
`GET /todos/{id}`  
The server returns a JSON object containing the properties of the todo item with the specified ID.  
If no item is found with this ID, a 404 response will be sent back to the client.

### POST
`POST /todos BODY {item}`  
The body of the request should include the JSON object representing the todo item which should be added to the database. Besides the usual optional properties, *orderNumber* is also optional. If unspecified, it gets a value so that the item will be the last in the order, by having the largest *orderNumber*. This means it will be initialized to `max(orderNumbers) + 1`.  
Possible response codes:
- 201: Operation successful, item was inserted. The whole todo object can be found in the body of the response, and the *Location* header is also set to the path of the new todo item.
- 400: The JSON object received by the server was not valid. Most probably the *title* property was missing (or null) or the value of *categoryName* is not an existing category.
- 500: The request was correct but the item could not be inserted into the database.

### PUT
`PUT /todos/{id} BODY {item}`  
Updates the item identified by the ID in the path. This ID and the JSON object's ID should be the same. The body should include the DTO with the appropriate data to update the item in the database.  
Possible response codes:
- 200: Item was successfully updated.
- 400: Incorrect JSON object. Possible causes: a required property is unspecified, *categoryName* is invalid, IDs (path and body) do not match, date format of *deadline* is incorrect.
- 500: The request was correct but the item could not be updated in the database.

### DELETE
`DELETE /todos/{id}`  
Deletes the item with the specified ID.  
Response code is 204 for success. If the item was not even in the database, the operation is considered successful.

## Architecture

The application uses a 3-layered architecture.

### Data access layer
Consist of the database-dependent [data-layer](/todo-backend/data-layer) project, tightly coupled with the database. Part of the backend.  
Responsible for the management of persistent storage.

### Business layer
The [api-controllers](/todo-backend/api-controllers) project represents the business layer of this application. Part of the backend.  
Its responsibility is communication between the data and presentation layers, and enforcing business rules.

### Presentation layer
Implemented in the [web-frontend](/web-frontend) directory as a react SPA.  
Responsible for presenting the data to the user and collecting user input (modifications, interactions).