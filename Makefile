
help:
	@echo "Usage:"
	@echo "    'make restore' to restore dependencies."
	@echo "    'make build'   to build the solution."
	@echo "    'make rebuild' to build the solution without restoring."
	@echo "    'make run'     to run the Chirp.Web project."
	@echo "    'make rerun'   to run the Chirp.Web project without restoring or building."
	@echo "    'make watch'   to run the Chirp.Web project and watch for changes."
	@echo "    'make rewatch' to run the Chirp.Web project and watch for changes without restoring or building."
	@echo "    'make test'    to run the tests."

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
	@dotnet run --project src/Chirp.Web/Chirp.Web.csproj --no-restore --no-build

rerun:
	@echo "Running Chirp.Web..."
	@dotnet run --project src/Chirp.Web/Chirp.Web.csproj --no-restore --no-build

watch:
	@echo "Running Chirp.Web..."
	@dotnet watch --project src/Chirp.Web/Chirp.Web.csproj --no-restore --no-build

rewatch:
	@echo "Running Chirp.Web..."
	@dotnet watch --project src/Chirp.Web/Chirp.Web.csproj --no-restore --no-build

test: build
	@echo "Running tests..."
	@dotnet test --no-restore --no-build

.PHONY: help build restore run rerun watch rewatch test

