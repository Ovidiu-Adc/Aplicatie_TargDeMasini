### Documentație Proiect: Targul de Mașini

---

#### 1. Descriere Proiect din Punct de Vedere al Utilizatorului

**Targul de Mașini** este o aplicație desktop destinată gestionării tranzacțiilor auto. Aceasta oferă utilizatorilor o interfață intuitivă și ușor de utilizat pentru a adăuga, căuta, edita și șterge tranzacții auto. Principalele funcționalități ale aplicației sunt:

- **Adăugare Tranzacție**: Utilizatorii pot adăuga noi tranzacții auto completând câmpurile necesare, cum ar fi numele vânzătorului, numele cumpărătorului, tipul și modelul mașinii, anul fabricației, culoarea, data tranzacției, prețul și opțiunile disponibile (ex. Cutie Automată, Aer Condiționat, Navigație GPS, etc.).
- **Căutare Tranzacție**: Utilizatorii pot căuta tranzacții auto pe baza tipului sau modelului mașinii și a unei perioade de timp specificate. Rezultatele căutării sunt afișate într-o listă, iar utilizatorii pot vedea detalii despre fiecare tranzacție.
- **Editare Tranzacție**: Utilizatorii pot edita tranzacțiile existente. Aceștia pot modifica detaliile tranzacției și salva modificările.
- **Ștergere Tranzacție**: Utilizatorii pot șterge tranzacțiile auto din baza de date.
- **Vizualizare Tranzacții**: Toate tranzacțiile sunt afișate într-un tabel, oferind o vizualizare clară și organizată a tuturor tranzacțiilor înregistrate.

---

#### 2. Descriere Proiect din Punct de Vedere al Programatorului

**Targul de Mașini** este o aplicație desktop dezvoltată în C# utilizând Windows Forms. Proiectul este structurat în mai multe componente și fișiere, fiecare având un rol specific în funcționarea aplicației. Mai jos sunt detaliate principalele componente și funcționalități ale proiectului:

##### Structura Proiectului

- **Aplicatie_TargDeMasini**
  - **TargDeMasiniInterfata**
    - **Form1.cs**: Acesta este fișierul principal al interfeței utilizatorului. Conține codul pentru inițializarea componentelor UI și gestionarea evenimentelor
    - **Form1.Designer.cs**: Conține codul generat automat pentru designul formularului.
    - **Program.cs**: Punctul de intrare al aplicației. Inițializează și rulează aplicația.
  - **ClaseNecesare**
    - **Clase.cs**: Conține definițiile claselor utilizate în proiect, cum ar fi `TranzactieAuto`, `Culoare`, și `Optiuni`.
  - **StocareDate**
    - **FisierText.cs**: Conține clasa `ManagerTranzactii`, responsabilă pentru citirea și scrierea tranzacțiilor în fișierul text `tranzactii.txt`.

##### Funcționalități Principale

- **Adăugare Tranzacție**:
  - Metoda `btnAdauga_Click` din `Form1.cs` gestionează evenimentul de click pe butonul de adăugare. Verifică validitatea datelor introduse și adaugă tranzacția în listă și în fișierul text.
  - Metoda `AdaugaTranzactiePeFormular` adaugă tranzacția în interfața utilizatorului.
  - Metoda `ReseteazaControale` resetează câmpurile formularului după adăugarea unei tranzacții.

- **Căutare Tranzacție**:
  - Metoda `btnCauta_Click` gestionează evenimentul de click pe butonul de căutare. Filtrează tranzacțiile pe baza criteriilor introduse de utilizator și afișează rezultatele în `lstAfisare`.
  - Metoda `CeaMaiCautataMasina` din `Clase.cs` calculează cea mai căutată mașină într-o anumită perioadă.

- **Editare Tranzacție**:
  - Metoda `btnEditareTr_Click` gestionează evenimentul de click pe butonul de editare. Permite utilizatorilor să modifice detaliile tranzacțiilor existente.
  - Metoda `dataGridTranzactii_CellEndEdit` salvează modificările făcute de utilizatori în DataGridView.

- **Ștergere Tranzacție**:
  - Metoda `dataGridTranzactii_CellContentClick` gestionează evenimentul de click pe butonul de ștergere din DataGridView. Șterge tranzacția selectată din listă și din fișierul text.

- **Stocare și Citire Date**:
  - Clasa `ManagerTranzactii` din `FisierText.cs` gestionează citirea și scrierea tranzacțiilor în fișierul text `tranzactii.txt`.
  - Metoda `CitesteTranzactii` citește tranzacțiile din fișierul text și le încarcă în listă la pornirea aplicației.
  - Metoda `AdaugaTranzactie` adaugă o nouă tranzacție în fișierul text.
  - Metoda `SalveazaTranzactii` salvează toate tranzacțiile în fișierul text.

##### Considerații de Implementare

- **Validarea Datelor**: Înainte de a adăuga sau edita o tranzacție, aplicația validează datele introduse de utilizator pentru a se asigura că sunt corecte și complete.
- **Gestionarea Erorilor**: Aplicația gestionează erorile care pot apărea în timpul citirii sau scrierii în fișierul text și afișează mesaje de eroare corespunzătoare.
- **Interfața Utilizator**: Designul interfeței utilizatorului este realizat folosind Windows Forms, oferind o experiență de utilizare intuitivă și ușor de navigat.
