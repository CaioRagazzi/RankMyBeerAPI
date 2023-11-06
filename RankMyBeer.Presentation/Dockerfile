FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app 
#
# copy csproj and restore as distinct layers
COPY *.sln .
COPY RankMyBeer.Presentation/*.csproj ./RankMyBeer.Presentation/
COPY RankMyBeer.Application/*.csproj ./RankMyBeer.Application/
COPY RankMyBeer.Domain/*.csproj ./RankMyBeer.Domain/
COPY RankMyBeer.Infrastructure/*.csproj ./RankMyBeer.Infrastructure/
COPY RankMyBeer.Test/*.csproj ./RankMyBeer.Test/
#
RUN dotnet restore 
#
# copy everything else and build app
COPY RankMyBeer.Presentation/. ./RankMyBeer.Presentation/
COPY RankMyBeer.Application/. ./RankMyBeer.Application/
COPY RankMyBeer.Domain/. ./RankMyBeer.Domain/
COPY RankMyBeer.Infrastructure/. ./RankMyBeer.Infrastructure/
COPY RankMyBeer.Test/. ./RankMyBeer.Test/
#
WORKDIR /app/RankMyBeer.Presentation
RUN dotnet publish -c Release -o out
#
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app 
#
COPY --from=build /app/RankMyBeer.Presentation/out ./
COPY rankmybeer-b413dd05a4e1.json .
ENTRYPOINT ["dotnet", "RankMyBeer.Presentation.dll"]