# About
Contains a template for easy setup of a Ucommerce project using a headless approach.

# Key Features
- Installs all necessary packages for a headless Ucommerce experience.
- Includes basic setup and configuration of the application.
- Includes a docker compose setup with services UCommerce require

# How To

## Generate the project
To use the headless template:
- Open a terminal in the folder where you wish to create the project.
- Execute the command:
`dotnet new uc-headless --name "<ProjectName>"`

## Add the project to the solution
`dotnet sln add "<ProjectName>"`

## Run SQL and Elasticserver in Docker

The template generates a docker setup with an Azure SQL and an Elasticsearch server with the
necessary configuration added to just run Ucommerce.

`docker compose -f docker/docker-compose.yml up -d`

## Run the project
`dotnet run --project "<ProjectName>"`

## Access the administration interface
Once your application runs, you can access the administration interface by requesting _/ucommerce_.

# Template options
For a full list of options and commands for the template, you can execute:
`dotnet new uc-headless -h`