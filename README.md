Oto sprawdzony i poprawiony tekst pod kątem pisowni, gramatyki i stylu:

---

# TicketingTool

### Konfiguracja:
1. Ustawić parametry lokalnej bazy danych w pliku `appsettings.json`, klucz `"default"`:
   
  "ConnectionStrings": {
  "Default":
    "Server=**<LOKALNY SERWER>**;
    Database=TicketingTool;
    Trusted_Connection=True;
    MultipleActiveResultSets=true;
    TrustServerCertificate=True"
  }

3. Wykonać polecenie `Add-Migration Initial` w Package Manager Console.
4. Wykonać polecenie `Update-Database` w Package Manager Console.

### Użytkownicy testowi:
| UserName | Hasło     | Rola globalna | Przypisanie do projektu | Rola w projekcie |
|----------|-----------|---------------|--------------------------|------------------|
| X001     | password  | ADMIN         | *                        | ADMIN            |
| X002     | password  |               | BSC                      | USER             |
| X003     | password  |               | BSC                      | MANAGER          |

---

### Opis programu:
Program jest prostą kopią Jiry:
- Obsługuje projekty.
- W projektach można definiować komponenty.
- Można tworzyć i zarządzać ticketami.

#### Funkcjonalności:
- **Tickety**: Tickety można tworzyć, otwierać, zmieniać. Nie można utworzyć ticketa poza aplikacją. Rozważano opcje takie jak SSH czy użycie Google Forms, które mogą zostać zaimplementowane w przyszłości.
- **Logowanie**: Użytkownik musi się zalogować, podając `UserName` zgodnie z konwencją `X{liczba}{liczba}{liczba}` (odwzorowującą wewnętrzne identyfikatory firmy). Hasło musi zawierać wyłącznie małe litery.
- **Zarządzanie uprawnieniami**: 
  - Użytkownicy z rolą ADMIN (np. X001) muszą przypisać nowych użytkowników do projektów i ról (`USER` lub `MANAGER`) w panelu administracyjnym.
  - Uprawnienia decydują o możliwości tworzenia, wyświetlania i edytowania ticketów w przypisanych projektach.
  - Użytkownicy z rolą MANAGER mogą wyświetlać listę wszystkich członków projektu oraz dodawać komponenty.
  - ADMIN ma automatycznie przypisywaną rolę administratora (lokalna rola) do każdego nowo utworzonego projektu.
- **Techniczni użytkownicy**: są dodani poziomu konfiguracji bazy danych. Ich rola w obecnej wersji programu nie została w pełni wykorzystana, choć planowano, że umożliwią zdalne tworzenie ticketów.

#### Dodatkowe informacje:
- Tickety można komentować.
- Wprowadzono mechanizm logowania zmian w ticketach (zakładka **Updates**), choć ze względu na możliwość rozbudowy ticketów o dodatkowe pola (dostosowane do bardziej zaawansowanych projektów) wynik nie jestj czytelny dla użytkownika.
