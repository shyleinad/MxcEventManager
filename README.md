# MxcEventManager

## Áttekintés

Ez a projekt egy **ASP.NET Core Web API** alkalmazás, amely események (Event), helyszínek (Location) és országok (Country) kezelésére szolgál. A feladat célja egy jól strukturált, karbantartható backend API megvalósítása Entity Framework Core használatával.

A projekt kifejezetten **tesztfeladat** jelleggel készült, ezért fontos szempont volt:

* egyszerű futtathatóság
* tiszta architektúra
* jól érthető kód
* minimális külső függőség

---

## Fő technológiák

* **.NET 8 / ASP.NET Core Web API**
* **Entity Framework Core**
* **SQL Server (LocalDB vagy SQL Express)**
* **ASP.NET Core Identity**
* **Swagger / OpenAPI**

---

## Projekt struktúra

```
MxcEventManager
│
├── Controllers        // API végpontok
├── Data               // DbContext-ek, seedelés
├── DTOs               // Adatátviteli objektumok + MapHelper
├── Models             // EF Core entitások
├── Program.cs         // Alkalmazás belépési pont
└── appsettings.json   // Konfigurációk
```

---

## Adatmodell

### Kapcsolatok

* **Country** 1 → N **Location**
* **Location** 1 → N **Event**

A mentés **foreign key** alapon történik, a navigation property-ket az EF Core tölti fel lekérdezéskor.

---

## Adatbázis

Az alkalmazás **SQL Server**-t használ.

Jelenlegi connection string (LocalDB):

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=MxcEventManager;Trusted_Connection=True;",
  "DefaultIdentityConnection": "Server=(localdb)\\MSSQLLocalDB;Database=MxcEventManager.Identity;Trusted_Connection=True;"
}
```

Így nincs szükség felhasználónév/jelszó megadására, az alkalmazás azonnal futtatható.

---

## Migrációk

A projekt **két DbContext-et** használ:

* `AppDbContext` – üzleti adatok
* `AppIdentityDbContext` – Identity

Migráció létrehozása (alapból tartalmazza a repository, nem szükséges):

```bash
dotnet ef migrations add InitialCreate --context AppDbContext

dotnet ef migrations add IdentityInit --context AppIdentityDbContext
```

Adatbázis frissítése:

```bash
dotnet ef database update --context AppDbContext

dotnet ef database update --context AppIdentityDbContext
```

---

## Seed adatok

Induláskor automatikusan feltöltésre kerülnek:

* országok
* helyszínek
* események
* egy admin felhasználó (Identity)

Ez a `SeedData` és `IdentitySeedData` osztályokon keresztül történik.

---

## API dokumentáció

Swagger elérhető fejlesztői módban:

```
https://localhost:{port}/
```

---

## Futtatás

1. Repository klónozása
2. `dotnet restore`
4. `dotnet run`
5. A Homepage a Swagger UI-t fogja megnyitni.

---
