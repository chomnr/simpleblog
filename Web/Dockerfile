FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM postgres:15
# DOCKER HUB ARG
ARG POSTGRES_DB
ARG POSTGRES_HOST
ARG POSTGRES_PASS
ARG POSTGRES_USER
# ENVIRONMENT
ENV POSTGRES_DB $POSTGRES_DB
ENV POSTGRES_HOST $POSTGRES_HOST
ENV POSTGRES_PASS $POSTGRES_PASS
ENV POSTGRES_USER $POSTGRES_USER
EXPOSE 5432

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["/Web/Web.csproj", "Web/"]
COPY ["/Application/Application.csproj", "Application/"]
RUN dotnet restore "Web/Web.csproj"
COPY . .
WORKDIR "/src/Web"
RUN dotnet build "Web.csproj" -c Release -o /app/build

# Extra Stuff..
#COPY Web/build.sh /app/build.sh
COPY ["/Web/build.sh", "Web/"]
RUN chmod +x /app/build.sh
RUN /app/build.sh

FROM build AS publish
RUN dotnet publish "Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web.dll"]
