![SimpleBlogBanner](https://github.com/okjlez/SimpleBlog/blob/master/Web/blob/ReadMeLogo.png?raw=true)
----------------------------------------------------------------
<p align="center">
  <img src="https://img.shields.io/codacy/grade/75a7625558db465cbdf9943b45ee345a"/>
  <img src="https://img.shields.io/github/commit-activity/m/okjlez/simpleblog?color=ff69b4"/>
  <img src="https://img.shields.io/github/repo-size/okjlez/simpleblog"/>
</p>
View Preview<br>
https://www.youtube.com/watch?v=MOxt6Icv19Q

Live Preview<br>
https://projects.zeljko.me/simpleblog/

I originally wrote a blog system in Rust, but it was terrible, so I rewrote it in a more familiar language. 
This project should be used as a personal blog, meaning you should be the only person who uses it. But if you want
more than one to use it, go right ahead. That's fine too.

The Codebase follows the Vertical Slice Architecture, or at least it tries. Note the code does lack abstraction
and is a necessity to ensure flexibility and cleanliness, so if your contributions pertain to improving
the abstraction, thank you.

You can modify anything to your liking. 

## Contributions
I preferably just want commits from `#first-timers-only`. But contributions from anyone is welcomed.
* Bug Fixes (love it)
* Features (you shouldn't have)
* More tests (i'm sorry)

## Getting started
<b>For the program to work PostgreSQL must be running and be configured inside the `Web/appsettings.json` before execution.</b>

1. Install [PostgreSQL 15.3](https://www.enterprisedb.com/downloads/postgres-postgresql-downloads)
2. Install [.NET 7 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)

### 1. Installation
```bash
git clone https://github.com/chomnr/SimpleBlog.git
cd simpleblog

# Build
dotnet build

# Run
cd Web
dotnet run

# Unit tests
dotnet test Tests/Tests.csproj 

# Publish
dotnet publish Web/Web.csproj --configuration Release 
```

### 2. Migrations
Please before running the migration commands ensure that Postgres is RUNNING && CONFIGURED inside `appsettings.json`.

```bash
# Run directly from root folder. ./simpleblog
dotnet ef migrations add "InitialMigration" --project Application --startup-project Web --output-dir Infrastructure/Persistence/Migrations
dotnet ef database update --project Application --startup-project Web
```

### 3* Configuring a domain with a pathbase.
If you plan to host your site on a pathbase, please continue reading. If not, don't worry.
#### What is a pathbase?
Here's an example.
<br>
<br>
✅ 
```
example.com/pathbase and test.example.com/pathbase have a pathbase..
```

❌
```
example.com and test.example.com do not have a pathbase.
```
#### Setting up a pathbase.
* Go to `Web/Program.cs`
* Add `app.UsePathBase("/pathbase");` make sure you change `/pathbase` to your PathBase

### 4* Setting up email confirmation.
If you would like the user to only be able to sign in with a confirmed account, please follow the instructions.
#### Setup
* Go to `Web/Program.cs`
* change `options.SignIn.RequireConfirmedAccount = false` to `true`
* change `options.SignIn.RequireConfirmedEmail = false` to `true`

