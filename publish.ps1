# source: https://www.meziantou.net/create-a-multi-arch-docker-image-for-a-dotnet-application.htm
# Create a new web project
#dotnet new web

# Create 2 images for x64 and ARM64
$registry = "docker.io"
$image = "ploufs/webencryptor"
$tag = "20241214"

dotnet publish -p:PublishProfile=DefaultContainer --configuration Release --os linux --arch x64 -p:ContainerImageTag=$tag-x64 -p:ContainerRepository=$image -p:ContainerRegistry=$registry
dotnet publish -p:PublishProfile=DefaultContainer --configuration Release --os linux --arch arm64 -p:ContainerImageTag=$tag-arm64 -p:ContainerRepository=$image -p:ContainerRegistry=$registry

#combine the images into a manifest and push the manifest to the registry
docker manifest create "$registry/${image}:$tag" "$registry/${image}:$tag-x64" "$registry/${image}:$tag-arm64" --amend
docker manifest push "$registry/${image}:$tag"