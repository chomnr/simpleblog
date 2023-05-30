![SimpleBlogBanner](https://github.com/okjlez/SimpleBlog/blob/master/Web/blob/ReadMeLogo.png?raw=true)
----------------------------------------------------------------
View Preview<br>
https://www.youtube.com/watch?v=qjZLQP8ey7Q

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
I used various new tools, such as the EntityFramework & Blazor Server. I preferably want commits 
just from `#first-timers-only`. But contributions from anyone is welcomed.

## Getting started
Install Postgres on your local machine and or server.<br>
Create an account on SendGrid.<br>
Enter SendGrid key & Postgres credentials in the appsettings.json.
Then follow the migrations directions.

## External Requirements
The only external requirements that are needed is `PostgreSQL` & `SendGrid`.

## Appsettings
### Placeholders
`config["EmailResetPasswordBody"]` 
`{url} {token} {userid} {firstname} {lastname}`
<br><br>
`config["EmailResetPasswordBody"]` 
`{url} {token} {userid} {firstname} {lastname}`


## Migrations
Make sure you're in the solution's directory.<br><br>
Note: Make sure the database is running and the credentials have been set inside the appsettings.json<br><br>
`dotnet ef migrations add "InitialMigration" --project Application --startup-project Web --output-dir Infrastructure/Persistence/Migrations`<br><br>
`dotnet ef database update --project Application --startup-project Web`


