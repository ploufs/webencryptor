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

### api info
openapi : http://localhost:8080/openapi/v1.json
scalar: http://localhost:8080/scalar/v1

## exemple
curl https://localhost:8080/PGPEncrypt --request POST --header 'Content-Type: application/x-www-form-urlencoded' --data-urlencode 'filenamePublicKey={{publickey}}' --data-urlencode 'text={{text}}'
