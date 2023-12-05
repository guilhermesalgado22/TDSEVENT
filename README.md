# Planejaí

Project developed as grade requirement for `Tecnology in System's Development` discipline of UTFPR (Universidade Tecnológica Federal do Paraná).

## Requirements

- `Visual Studio 2022` or `.NET SDK 6.0+`;
- `EntityFramework Core`:
  - For installing, type: `dotnet tool install --global dotnet-ef` and `dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL`;
- `Docker 4.19.0+`;
- Available `700 MB` storage;

## Installation

- On the root directory type on command prompt: `docker-compose build`. It will build the containers for you (you must have your Docker app opened). 
- After building, on the same directory command line, type: `docker-compose up -d`. It will turn on the containers and set it up for use.
  - Maybe, the `planejai_webapi` container may exit with the code 139, but you can turn it on manually.

## Running the application

- After following the steps above, the application is up to date and ready to use, so all you have to do is to open your browser and type `http://localhost:5293` on your search field and enjoy!

## Directory Structure of the project

Below is the directory tree structure of the project. \
The project was developed using Visual Studio Community 2022, free for academic purposes.

```javascript
├── .gitignore
├── docker-compose.yml
├── README.md
├── PlanejaiBack
│   ├── appsettings.Development.json
│   ├── appsettings.json
│   ├── Dockerfile
│   ├── PlanejaiBack.csproj
│   ├── Program.cs
│   ├── Controllers
│   │   ├── ActivityController.cs
│   │   ├── EventController.cs
│   │   ├── GuestController.cs
│   │   ├── ScheduleController.cs
│   │   └── UserController.cs
│   ├── Data
│   │   ├── AppDbContext.cs
│   │   └── DbInitializer.cs
│   ├── Migrations
│   │   ├── 20230623232838_Initial.cs
│   │   ├── 20230623232838_Initial.Designer.cs
│   │   └── AppDbContextModelSnapshot.cs
│   ├── Models
│   │   ├── ActivityModel.cs
│   │   ├── EventModel.cs
│   │   ├── EventsGuests.cs
│   │   ├── GuestModel.cs
│   │   ├── ScheduleModel.cs
│   │   └── UserModel.cs
│   └── Properties
│       └── launchSettings.json
└── PlanejaiFront
    ├── appsettings.Development.json
    ├── appsettings.json
    ├── Dockerfile
    ├── PlanejaiFront.csproj
    ├── PlanejaiFront.sln
    ├── Program.cs
    ├── Models
    │   ├── ActivityModel.cs
    │   ├── EventModel.cs
    │   ├── EventsGuests.cs
    │   ├── GuestModel.cs
    │   ├── ScheduleModel.cs
    │   └── UserModel.cs
    ├── Pages
    │   ├── Index.cshtml
    │   ├── Index.cshtml.cs
    │   ├── _ViewImports.cshtml
    │   ├── _ViewStart.cshtml
    │   ├── Activities
    │   │   ├── Add.cshtml
    │   │   ├── Add.cshtml.cs
    │   │   ├── Edit.cshtml
    │   │   ├── Edit.cshtml.cs
    │   │   ├── Remove.cshtml
    │   │   └── Remove.cshtml.cs
    │   ├── Calendar
    │   │   ├── Index.cshtml
    │   │   └── Index.cshtml.cs
    │   ├── Events
    │   │   ├── Create.cshtml
    │   │   ├── Create.cshtml.cs
    │   │   ├── Details.cshtml
    │   │   ├── Details.cshtml.cs
    │   │   ├── Edit.cshtml
    │   │   ├── Edit.cshtml.cs
    │   │   ├── Index.cshtml
    │   │   ├── Index.cshtml.cs
    │   │   ├── Remove.cshtml
    │   │   └── Remove.cshtml.cs
    │   ├── Guests
    │   │   ├── Add.cshtml
    │   │   ├── Add.cshtml.cs
    │   │   ├── Remove.cshtml
    │   │   └── Remove.cshtml.cs
    │   ├── Schedules
    │   │   ├── Create.cshtml
    │   │   ├── Create.cshtml.cs
    │   │   ├── Details.cshtml
    │   │   ├── Details.cshtml.cs
    │   │   ├── Index.cshtml
    │   │   ├── Index.cshtml.cs
    │   │   ├── Remove.cshtml
    │   │   └── Remove.cshtml.cs
    │   └── Shared
    │       ├── _MainLayout.cshtml
    │       ├── _MenuPartialPage.cshtml
    │       └── _ViewImports.cshtml
    ├── Properties
    │   └── launchSettings.json
    └── wwwroot
        └── images
            └── favicon.ico
```
