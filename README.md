# Project Name

## Overview
This project is a microservices-based application with multiple services and frontends. It uses Docker for containerization and includes CI/CD pipeline configurations.

## Directory Structure
```
├── .gitignore
├── Docker/
|   ├── dj-panel-composer.yml
|   ├── microfrontend.Dockerfile
|   ├── microservice.Dockerfile
|   └── nginx.conf
├── README.md
├── Frontends/
├── Micros.sln
├── Pipelines/
├── run-chat.bat
├── run-chat.sh
├── run-dj-panel.bat
├── run-dj-panel.sh
└── Services/
    ├── CompanyAPI/
    ├── DJHostGateway/
    ├── DocumentsAPI/
    ├── EquipmentAPI/
    ├── IdentityAPI/
    ├── MailingAPI/
    ├── MusicAPI/
    ├── PartyAPI/
    └── Shared/
```

## Running the Services

### Chat
To run the Chat service, use the following commands:

#### On Windows:
```sh
run-chat.bat
```

#### On Unix-based systems:
```sh
./run-chat.sh
```

### DJ Panel
To run the DJ Panel service, use the following commands:

#### On Windows:
```sh
run-dj-panel.bat
```

#### On Unix-based systems:
```sh
./run-dj-panel.sh
```

## Docker
The project uses Docker for containerization. The Docker configuration files are located in the `Docker` directory.

- `dj-panel-composer.yml`: Docker Compose file for the DJ Panel.
- `microfrontend.Dockerfile`: Dockerfile for building microfrontends.
- `microservice.Dockerfile`: Dockerfile for building microservices.
- `nginx.conf`: NGINX configuration file.

## Services
The `Services` directory contains the following microservices:

- `CompanyAPI`
- `DJHostGateway`
- `DocumentsAPI`
- `EquipmentAPI`
- `IdentityAPI`
- `MailingAPI`
- `MusicAPI`
- `PartyAPI`
- `Shared`

Each service has its own `.dockerignore` file and project-specific files.

## Frontends
The `Frontends` directory contains the frontend code for the application.

## Solution File
The `Micros.sln` file is the Visual Studio solution file that includes all the projects in the workspace.

## Pipelines
The `Pipelines` directory contains CI/CD pipeline configurations.

## License
This project is licensed under the MIT License.