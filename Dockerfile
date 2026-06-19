FROM node:20 AS node-build
WORKDIR /app/frontend

COPY frontend/package*.json ./
RUN npm install
COPY frontend/ ./
RUN npm run build -- --configuration production


FROM mcr.microsoft.com/dotnet/sdk:10.0 AS dotnet-build
WORKDIR /src
COPY ["Backend/Backend/Backend.csproj", "Backend/Backend/"]
RUN dotnet restore "Backend/Backend/Backend.csproj"

COPY Backend/Backend/ Backend/Backend/
WORKDIR "/src/Backend/Backend"
RUN dotnet publish "Backend.csproj" -c Release -o /app/publish /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=dotnet-build /app/publish .

COPY --from=node-build /app/frontend/dist/frontend/browser ./wwwroot/

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "Backend.dll"]
