#FROM nginx:alpine AS base
#
#FROM mcr.microsoft.com/dotnet/sdk:5.0 AS publish
#WORKDIR /app
#
#COPY ["src/Clients/BlazorWebApp/WebApp/WebApp.csproj", "src/Clients/BlazorWebApp/WebApp/"]
#RUN dotnet restore "src/Clients/BlazorWebApp/WebApp/WebApp.csproj"
#
#COPY . .
##WORKDIR "/src/Clients/BlazorWebApp/WebApp"
#RUN dotnet build "src/Clients/BlazorWebApp/WebApp/WebApp.csproj" -c Release -o /app/build
#
#FROM nginx:alpine
#COPY src/Clients/BlazorWebApp/WebApp/nginx.conf /etc/nginx/nginx.conf
#COPY --from=publish /app/build/wwwroot /usr/share/nginx/html/


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app
COPY . ./
#RUN dotnet publish -c Release -o output
RUN dotnet publish "src/Clients/BlazorWebApp/WebApp/WebApp.csproj" -c Release -o /app/build

FROM nginx:alpine
WORKDIR /var/www/web
COPY --from=build-env /app/build/wwwroot .
COPY src/Clients/BlazorWebApp/WebApp/nginx.conf /etc/nginx/nginx.conf
#EXPOSE 80