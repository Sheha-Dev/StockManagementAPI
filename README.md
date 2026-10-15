# Stock Management API

## Overview

The Stock Management API is an ASP.NET Core Web API developed for the Ravigno Software Engineer take-home assessment. It implements a branch stock-transfer workflow for managing stock movements between a warehouse and retail branches.

The API supports creating transfer requests, dispatching stock, receiving stock, cancelling pending transfers, and viewing stock levels and transfer records.

## Technology Stack

- ASP.NET Core Web API
- .NET 8 or later
- Entity Framework Core
- Microsoft SQL Server
- C#

## Features

- Create stock-transfer requests containing one or more products.
- Retrieve stock levels for a branch.
- Dispatch transfers and deduct stock from the sending branch.
- Receive dispatched transfers and add stock to the receiving branch.
- Cancel transfers that have not been dispatched.
- Retrieve and filter transfers by branch and status.
- Validate branch selections and product quantities.
- Handle errors through centralized exception handling.

## Transfer Status Workflow

The API supports the following status transitions:

- `Requested → Dispatched`
- `Dispatched → Received`
- `Requested → Cancelled`

Invalid status transitions must be rejected.

## Business Rules

- The source and destination branches must be different.
- Product quantities must be greater than zero.
- Stock must never become negative.
- Dispatching a transfer deducts stock from the source branch.
- Receiving a transfer adds stock to the destination branch.
- Repeated dispatch or receive requests must not move stock more than once.
- Only transfers in the `Requested` status can be cancelled.

## Prerequisites

- .NET SDK 8.0 or later
- Microsoft SQL Server
- Git
- Entity Framework Core CLI (`dotnet-ef`)

## Setup and Execution

### 1. Clone the repository

```bash
git clone https://github.com/Sheha-Dev/StockManagementAPI.git
cd StockManagementAPI
```

### 2. Configure the database

Update the connection string in `appsettings.json` or the appropriate configuration source.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=StockTransferDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Replace the example values with your local SQL Server configuration. Do not commit production credentials.

### 3. Restore dependencies

```bash
dotnet restore
```

### 4. Apply database migrations

If the project uses Entity Framework Core migrations, run:

```bash
dotnet ef database update
```

Run this command from the directory containing the relevant project, or specify the project and startup project if required.

### 5. Run the API

```bash
dotnet run
```

Use the URL displayed in the terminal to access the API. If Swagger is enabled, use the Swagger UI to explore and test the endpoints.


