# BoardApp

## Init UI submodules

git submodule init  
git submodule update

## Adjust sqlite DB by CLI

1. install ef tools:
	dotnet tool install -g dotnet-ef

2. create migration:
	dotnet ef migrations add InitialCreate -p ../Board.DataLayer

3. apply migration:
	dotnet ef database update
