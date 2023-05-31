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
soon.

I originally wrote a blog system in Rust, but it was terrible, so I rewrote it in a more familiar language. 
This project should be used as a personal blog, meaning you should be the only person who uses it. But if you want
more than one to use it, go right ahead. That's fine too.

The Codebase follows the Vertical Slice Architecture, or at least it tries. Note the code does lack abstraction
and is a necessity to ensure flexibility and cleanliness, so if your contributions pertain to improving
the abstraction, thank you.

You can modify anything to your liking. 

## Contributions
I preferably just want commits from `#first-timers-only`. But contributions from anyone is welcomed.

## Building
----------------------------------------------------------------
### Requirements
<details>
<summary>PostgreSQL >= 14 </summary>
</details>
<details>
<summary>SendGrid</summary>
</details>
<details>
<summary>.NET 7</summary>
</details>

### Instructions
For the program to work PostgreSql must be running and be configured inside the `Web/appsettings.json`. 
<br><br>
The project currently will work if you do not configure SendGrid inside `appsettings.json`
<br><br>
If you would like the user to only be able to SignIn with a confirmed account follow the instructions.
* Go to Web/Program.cs
* change `options.SignIn.RequireConfirmedAccount = false` to `true`
* change `options.SignIn.RequireConfirmedEmail = false` to `true`

#### Migrations
Please before running the second migration command `dotnet ef database update --project Application --startup-project Web`
ensure that Postgres is RUNNING && CONFIGURED inside `appsettings.json`.
<br><br>
From the root directory /SimpleBlog, run the following commands IN ORDER.
* `dotnet ef migrations add "InitialMigration" --project Application --startup-project Web --output-dir Infrastructure/Persistence/Migrations`
* `dotnet ef database update --project Application --startup-project Web`

## Features



## Migrations
Make sure you're in the solution's directory.<br><br>
Note: Make sure the database is running and the credentials have been set inside the appsettings.json<br><br>
`dotnet ef migrations add "InitialMigration" --project Application --startup-project Web --output-dir Infrastructure/Persistence/Migrations`<br><br>
`dotnet ef database update --project Application --startup-project Web`


