# Start a file system based STAC API compliant service

This quick guide describes how to use your Docker container for the STAC API with a file system based catalog.

* Install Docker on your machine: If you haven't installed Docker on your machine, follow the official Docker installation guide based on your operating system.

* Pull the [Docker image](https://github.com/Terradue/DotNetStac.Api/pkgs/container/stacapi-fs): You can pull the [Docker image for your STAC API](https://github.com/Terradue/DotNetStac.Api/pkgs/container/stacapi-fs) using the following command:
  
```console
docker pull ghcr.io/terradue/stacapi-fs:latest
```

* (OPTIONAL) Create a directory for your catalog: Create a directory on your host machine where you want to store your catalog data. This step is optional but makes your catalog persistent on your machine. For example, you can create a directory named my-catalog in your home directory:

```console
mkdir ~/my-stac-catalog
```

* (OPTIONAL) Place your catalog data in the directory: Place your catalog data in the directory you created in the previous step. The directory should contain the STAC catalog JSON files with the right [file structure](xref:fileSystemImplementation#file_system_structure). If you don't have any catalog data, the STAC API will create a new empty catalog for you.

* Run the Docker container: You can run the Docker container with the following command:

```console
docker run -p 8080:80 -v ~/my-stac/catalog:/data ghcr.io/terradue/stacapi-fs
```

> This command will start the Docker container and map the container port 80 to the host port 8080. The -v option is used to mount the my-catalog directory on your host machine to the /data directory in the container. This will allow the STAC API to read and write to the catalog data stored on your host machine.

> If you want to use a different directory for your catalog data inside the container, you can set the environement variable `STACAPIFS_CatalogRootPath`. 

* Use the STAC API: You can now use the STAC API to query your catalog using HTTP requests. The API supports the STAC specification and provides a set of endpoints to query metadata and assets in your catalog. You can find more information on how to use the STAC API in the GitHub repository of your project.

That's it! Go to http://localhost:8080. With these steps, you should be able to use your STAC API Docker container with a mounted volume for your catalog data.