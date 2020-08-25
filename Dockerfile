FROM mcr.microsoft.com/dotnet/core/sdk:3.1

COPY ./publish /publish
WORKDIR /publish
EXPOSE 5000/tcp
ENTRYPOINT ["dotnet", "API.dll"]