{ pkgs ? import (fetchTarball "https://github.com/NixOS/nixpkgs/archive/7fdd1421774a52277fb56d64b26aaf7765e1b3fa.tar.gz") {} }:
pkgs.mkShell rec {
  #dotnetPkg = (with pkgs.dotnetCorePackages; combinePackages [
      #sdk_7_0
  #]);
  nativeBuildInputs = with pkgs; [
    ### Dotnet
    dotnet-sdk_7
    dotnet-runtime_7
    dotnet-aspnetcore_7
    #dotnetPkg
    ### Azure
    #azure-cli
    # Python3
    python3
    ### Sqlite
    sqlite
    ### PatchELF
    patchelf
  ];
  shellHook = ''
    DOTNET_ROOT="${pkgs.dotnet-sdk_7}";
  '';
}

### Change rpath
# $ patchelf ./Chirp --remove-rpath
# $ patchelf ./Chirp --add-rpath /nix/store/xq05361kqwzcdamcsxr4gzg8ksxrb8sg-gcc-12.3.0-lib/lib:/nix/store/xvxaw8q1b4dja27ljmynmc9818aagjz3-gcc-12.3.0-libgcc/lib/

### Change interpreter
# $ patchelf ./Chirp --set-interpreter /nix/store/ld03l52xq2ssn4x0g5asypsxqls40497-glibc-2.37-8/lib/ld-linux-x86-64.so.2
