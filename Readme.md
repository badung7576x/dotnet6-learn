## 1. Khởi tạo project + các cli cơ bản 

```
$ dotnet new mvc -o <project-name>    # Create new project
$ dotnet dev-certs https --clean      # Force https in develop environment
$ dotnet dev-certs https --trust

$ dotnet add package <package-name>   # Add package
$ dotnet restore                      # Restore dependencies

$ dotnet watch run                    # Run watch for develop
$ dotnet build                        # Build app

```

## 2. Install dot-ef cli tool

```
$ dotnet tool install --global dotnet-ef --version 6.0.3
$ dotnet tool install --global dotnet-aspnet-codegenerator --version 6.0.2
```

## 3. Identity Core

```
$ dotnet add package Microsoft.EntityFrameworkCore.SqlServer
$ dotnet add package Microsoft.EntityFrameworkCore
$ dotnet add package Microsoft.EntityFrameworkCore.Design
$ dotnet add package Microsoft.AspNetCore.Identity
$ dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 6.0
```


## 4. Dotnet EF cli

```
$ dotnet ef migrations add <migration-name> # generate migration file
$ dotnet ef migrations list                 # Check migration status
$ dotnet ef database update                 # Run to update database
$ dotnet ef migrations remove               # Remove last migration
$ dotnet ef database drop -f -v             # Drop database
```

## 5. Add package to send email

```
$ dotnet add package MailKit
$ dotnet add package MimeKit
```


## n. Note document to research


