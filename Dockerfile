FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY published/ .
ENV ASPNETCORE_URLS=http://0.0.0.0:80

ENTRYPOINT ["dotnet", "api.makebe.agenda.dll"] 

