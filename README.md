graph TD
    subgraph "Warstwa Prezentacji (KERP.BlazorUI)"
        UI_Component["Komponent Blazor (np. Strona Aktualizacji)"]
    end

    subgraph "Warstwa Aplikacji (KERP.Application)"
        A_Mediator["IMediator (DiyMediator)"]
        
        subgraph "Strona Zapisu (Commands)"
            C_Command["PurchaseOrderUpdateCommand"]
            C_Handler["PurchaseOrderUpdateCommandHandler : ICommandHandler<..._>"]
            C_Validator["Walidatory"]
        end
        
        subgraph "Strona Odczytu (Queries)"
            Q_Query["GetPurchaseOrderHistoryQuery"]
            Q_Handler["GetPurchaseOrderHistoryQueryHandler : IQueryHandler<..._>"]
        end
    end

    subgraph "Warstwa Domeny (KERP.Domain)"
        D_Entities["Encje (np. PurchaseOrderReceiptDateUpdateEntity)"]
        D_Interfaces["Interfejsy Repozytoriów (np. IPurchaseOrderRepository)"]
    end

    subgraph "Warstwa Infrastruktury (KERP.Infrastructure)"
        I_Repositories["Implementacje Repozytoriów (np. PurchaseOrderRepository)"]
        I_DbContext["DbContext (EF Core)"]
        I_UoW["UnitOfWork"]
    end

    subgraph "Baza Danych"
        Database["Baza Danych SQL"]
    end

    %% Przepływ Komendy (Zapis)
    UI_Component -- "1.Tworzy i wysyła Command" --> A_Mediator
    A_Mediator -- "2.Znajduje i wywołuje CommandHandler" --> C_Handler
    C_Handler -- "3.Używa walidatorów" --> C_Validator
    C_Handler -- "4.Używa repozytorium (przez interfejs)" --> D_Interfaces
    C_Handler -- "5.Tworzy/modyfikuje encje" --> D_Entities
    D_Interfaces -- "DI wstrzykuje" --> I_Repositories
    I_Repositories -- "6.Operuje na DbContext" --> I_DbContext
    C_Handler -- "7.Zapisuje zmiany przez Unit of Work" --> I_UoW
    I_UoW -- "8.Wywołuje SaveChangesAsync()" --> I_DbContext
    I_DbContext -- "9.Generuje SQL INSERT/UPDATE" --> Database

    %% Przepływ Zapytania (Odczyt)
    UI_Component -- "1.Tworzy i wysyła Query" --> A_Mediator
    A_Mediator -- "2.Znajduje i wywołuje QueryHandler" --> Q_Handler
    Q_Handler -- "3.Używa repozytorium (przez interfejs)" --> D_Interfaces
    I_Repositories -- "4.Operuje na DbContext" --> I_DbContext
    I_DbContext -- "5.Generuje SQL SELECT" --> Database
    Database -- "6.Zwraca dane" --> I_DbContext
    I_DbContext -- "7.Mapuje na DTO/ViewModel" --> Q_Handler
    Q_Handler -- "8.Zwraca wynik" --> A_Mediator
    A_Mediator -- "9.Zwraca wynik do UI" --> UI_Component
