
help:
	@echo "Usage:"
	@echo "    'make restore'   to restore dependencies."
	@echo "    'make build'     to build the solution."
	@echo "    'make rebuild'   to build the solution without restoring."
	@echo "    'make run'       to run the Chirp.Web project."
	@echo "    'make rerun'     to run the Chirp.Web project without restoring or building."
	@echo "    'make watch'     to run the Chirp.Web project and watch for changes."
	@echo "    'make test'      to run the tests."
	@echo "    'make publish'   to publish the Chirp.Web project."
	@echo "    'make republish' to publish the Chirp.Web project without building."

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
	@dotnet run --project src/Chirp.Web/Chirp.Web.csproj --no-build

rerun:
	@echo "Running Chirp.Web..."
	@dotnet run --project src/Chirp.Web/Chirp.Web.csproj --no-build

watch:
	@echo "Running Chirp.Web..."
	@dotnet watch --project src/Chirp.Web/Chirp.Web.csproj

test: build
	@echo "Running tests..."
	@dotnet test --no-build

publish:
	@echo "Publishing Chirp.Web..."
	@dotnet publish src/Chirp.Web/Chirp.Web.csproj

republish:
	@echo "Publishing Chirp.Web..."
	@dotnet publish src/Chirp.Web/Chirp.Web.csproj --no-build

.PHONY: help build restore run rerun watch rewatch test

