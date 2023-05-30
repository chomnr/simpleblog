FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM postgres:15
WORKDIR /app

CMD ["postgres"]
EXPOSE 5432

#ARG POSTGRES_DB
#ARG POSTGRES_USER

#ENV POSTGRES_DB $POSTGRES_DB
#ENV POSTGRES_USER $POSTGRES_USER

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"

COPY ["/Web/Web.csproj", "Web/"]
COPY ["/Application/Application.csproj", "Application/"]
RUN dotnet restore "Web/Web.csproj"
COPY . .
WORKDIR "/src/Web"
RUN dotnet build "Web.csproj" -c Release -o /app/build

WORKDIR /src/Web
RUN chmod +x build.sh
RUN bash build.sh

FROM build AS publish
RUN dotnet publish "Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web.dll"]
