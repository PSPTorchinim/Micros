# System Zarządzania Eventami

## Przegląd
System oparty na architekturze mikroserwisowej, składający się z wielu usług backendowych i frontendowych. Wykorzystuje Docker do konteneryzacji oraz zawiera skonfigurowane pipeline'y CI/CD.

## Struktura Projektu
```
├── .gitignore
├── Docker/
│   ├── dj-panel-composer.yml
│   ├── microfrontend.Dockerfile
│   ├── microservice.Dockerfile
│   └── nginx.conf
├── README.md
├── Frontends/
├── Micros.sln
├── Pipelines/
├── Scripts/
│   ├── run-chat.bat
│   ├── run-chat.sh
│   ├── run-dj-panel.bat
│   └── run-dj-panel.sh
└── Services/
    ├── CompanyAPI/        # Zarządzanie firmami
    ├── DJHostGateway/     # Gateway API
    ├── DocumentsAPI/      # Obsługa dokumentów
    ├── EquipmentAPI/      # Zarządzanie sprzętem
    ├── IdentityAPI/       # Autentykacja i autoryzacja
    ├── MailingAPI/        # Obsługa wiadomości email
    ├── MusicAPI/          # Zarządzanie playlistami
    ├── PartyAPI/          # Zarządzanie eventami
    └── Shared/            # Współdzielone komponenty
```

## Uruchamianie Aplikacji

### Panel Czatu
```bash
# Windows
Scripts/run-chat.bat

# Linux/MacOS
./Scripts/run-chat.sh
```

### Panel DJ-a
```bash
# Windows
Scripts/run-dj-panel.bat

# Linux/MacOS
./Scripts/run-dj-panel.sh
```

## Konfiguracja Docker

Pliki konfiguracyjne Docker znajdują się w katalogu `Docker/`:

- `dj-panel-composer.yml` - Konfiguracja Docker Compose dla Panelu DJ-a
- `microfrontend.Dockerfile` - Konfiguracja budowania frontendów
- `microservice.Dockerfile` - Konfiguracja budowania mikroserwisów
- `nginx.conf` - Konfiguracja serwera NGINX

## Mikroserwisy

System składa się z następujących mikroserwisów:

| Serwis | Opis |
|--------|------|
| CompanyAPI | Zarządzanie firmami i ich profilami |
| DJHostGateway | API Gateway dla całego systemu |
| DocumentsAPI | Zarządzanie dokumentami i umowami |
| EquipmentAPI | Zarządzanie sprzętem i inwentarzem |
| IdentityAPI | Autentykacja i zarządzanie użytkownikami |
| MailingAPI | Obsługa komunikacji email |
| MusicAPI | Zarządzanie playlistami i utworami |
| PartyAPI | Zarządzanie eventami i rezerwacjami |

## Rozwój Projektu

### Wymagania
- .NET 7.0 lub nowszy
- Docker Desktop
- Node.js 16+ (dla frontendów)
- Visual Studio 2022 lub JetBrains Rider

### Budowanie Projektu
```bash
dotnet build Micros.sln
```

### Uruchamianie Testów
```bash
dotnet test Micros.sln
```

## CI/CD

Pipeline'y CI/CD znajdują się w katalogu `Pipelines/` i obsługują:
- Automatyczne budowanie
- Testy jednostkowe i integracyjne
- Deployment na środowiska testowe i produkcyjne

## Licencja
Projekt jest objęty licencją MIT. Szczegóły w pliku LICENSE.

## Wsparcie
W przypadku problemów lub pytań, prosimy o utworzenie Issue w repozytorium projektu.