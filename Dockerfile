FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App

COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0

RUN groupadd -g 1000 imagens-group && \
    useradd -u 1000 -g imagens-group -m makebe-user

RUN mkdir -p /app/uploads/HTML /app/uploads/Images && \
    chown -R makebe-user:imagens-group /app/uploads && \
    chmod -R 755 /app/uploads
	
RUN apt-get update && apt-get install -y curl	

WORKDIR /App

COPY --from=build-env /App/out .

RUN chown -R makebe-user:imagens-group /App

USER makebe-user

EXPOSE 80

ENV ASPNETCORE_ENVIRONMENT=Production


ENTRYPOINT ["dotnet", "api.makebe.agenda.dll"] 

