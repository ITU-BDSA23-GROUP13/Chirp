{ pkgs ? import <nixpkgs> {} }: # import (fetchTarball "https://github.com/NixOS/nixpkgs/archive/9b331a0ae55afbaeb3369d2e36c397b981c5ff28.tar.gz") {} }:
pkgs.mkShell {
  nativeBuildInputs = with pkgs; [
    # Dotnet
    #dotnet-sdk_7
    #dotnet-aspnetcore_7
    # Azure
    #azure-cli
    # Python3
    python3
    # Sqlite
    sqlite
  ];
}
