
CC = clang

help:
	@echo "Use 'make restore' to restore dependencies."
	@echo "Use 'make build'   to build the solution."
	@echo "Use 'make rebuild' to build the solution without restoring."
	@echo "Use 'make run'     to run the Chirp.Web project."
	@echo "Use 'make rerun'   to run the Chirp.Web project without restoring or building."
	@echo "Use 'make test'    to run the tests."

restore:
	@echo "Restoring dependencies..."
	@dotnet restore

build: restore
	@echo "Building solution..."
	@dotnet build --no-restore

rebuild: 
	@echo "Building solution..."
	@dotnet build --no-restore

run: build
	@echo "Running Chirp.Web..."
	@dotnet run --project src/Chirp.Web/Chirp.Web.csproj

rerun:
	@echo "Running Chirp.Web..."
	@dotnet run --project src/Chirp.Web/Chirp.Web.csproj --no-restore --no-build

test: build
	@echo "Running tests..."
	@dotnet test --no-restore --no-build

.PHONY: build restore run rerun

