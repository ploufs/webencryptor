# webencryptor

[![.NET](https://github.com/ploufs/webencryptor/actions/workflows/dotnet.yml/badge.svg)](https://github.com/ploufs/webencryptor/actions/workflows/dotnet.yml)
[![CodeQL](https://github.com/ploufs/webencryptor/actions/workflows/github-code-scanning/codeql/badge.svg)](https://github.com/ploufs/webencryptor/actions/workflows/github-code-scanning/codeql)
[![Dependabot Updates](https://github.com/ploufs/webencryptor/actions/workflows/dependabot/dependabot-updates/badge.svg)](https://github.com/ploufs/webencryptor/actions/workflows/dependabot/dependabot-updates)

### pull docker image
`docker pull ploufs/webencryptor:latest`
### environement variable
ASPNETCORE_HTTP_PORTS -> 8080
### volume
/app/PGPPublicKey -> pgp public key folder
### run image
`docker run -d --name=webencryptor -e TZ=America/Toronto -p 8080:8080 -v C:\PGPPublicKey:/app/PGPPublicKey ploufs/webencryptor:latest`