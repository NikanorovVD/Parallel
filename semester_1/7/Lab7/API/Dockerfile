# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# build
COPY . .
RUN dotnet publish -c release -o /app 

# ssl certificate 
RUN mkdir -p /app/https
RUN PASSWORD=$(openssl rand -base64 32)

RUN openssl req -x509 -newkey rsa:2048 -sha256 -nodes -keyout /app/https/domain.key -out /app/https/domain.crt -days 365 -subj "/CN=lab7"
RUN openssl pkcs12 -export -in /app/https/domain.crt -inkey /app/https/domain.key -out /app/https/aspnetapp.pfx -passout pass:"$PASSWORD"

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:9.0

WORKDIR /app
COPY --from=build /app ./

ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/app/https/aspnetapp.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=$PASSWORD

ENTRYPOINT ["dotnet", "API.dll"]

