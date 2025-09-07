# Task Manager Desktop

Prosta aplikacja desktopowa do zarządzania zadaniami, napisana w C# z wykorzystaniem Windows Forms.

## 📋 Opis

Task Manager Desktop to intuicyjna aplikacja umożliwiająca zarządzanie codziennymi zadaniami. Aplikacja pozwala na dodawanie, edytowanie, usuwanie oraz oznaczanie zadań jako ukończone, z funkcją automatycznego zapisu danych do pliku.

## ✨ Funkcjonalności

- **Dodawanie zadań** - tworzenie nowych zadań z tytułem, opisem, kategorią i priorytetem
- **Edytowanie zadań** - modyfikacja istniejących zadań
- **Usuwanie zadań** - możliwość usunięcia niepotrzebnych zadań
- **Oznaczanie zadań** - zmiana statusu zadania na ukończone/nieukończone
- **Kategorie zadań** - Work, Home, Shopping, Other
- **Priorytety** - Low, Medium, High, Critical
- **Automatyczny zapis** - dane zapisywane automatycznie do pliku `tasks.dat`
- **Trwałość danych** - zadania są przywracane po ponownym uruchomieniu

## 🛠️ Technologie

- **C# .NET Framework** - język programowania i platforma
- **Windows Forms** - interfejs użytkownika
- **Visual Studio** - środowisko programistyczne

## 📁 Struktura projektu

```
TaskManagerDesktop/
├── TaskManagerDesktop.sln          # Plik solution Visual Studio
├── .gitignore                       # Ignorowane pliki Git
└── TaskManagerDesktop/             # Główny projekt
    ├── TaskManagerDesktop.csproj   # Plik projektu
    ├── Program.cs                   # Punkt wejścia aplikacji
    ├── Task.cs                      # Model danych zadania + enums
    ├── TaskManager.cs               # Logika zarządzania zadaniami
    ├── TaskManagerMainForm.cs       # Główny formularz aplikacji
    ├── TaskManagerMainForm.resx     # Zasoby formularza
    ├── App.config                   # Konfiguracja aplikacji
    └── Properties/                  # Właściwości projektu
        ├── AssemblyInfo.cs
        ├── Resources.resx
        ├── Resources.Designer.cs
        ├── Settings.settings
        └── Settings.Designer.cs
```

## 🏗️ Architektura

### Klasy główne

1. **Task.cs** - Model danych zadania
   - Właściwości: Id, Title, Description, Category, Priority, IsCompleted, CreatedDate, CompletedDate
   - Enums: TaskCategory, TaskPriority
   - Metody: MarkAsCompleted(), MarkAsIncomplete()

2. **TaskManager.cs** - Logika biznesowa
   - Zarządzanie listą zadań
   - Operacje CRUD (Create, Read, Update, Delete)
   - Filtrowanie i sortowanie zadań
   - Event dla powiadamiania o zmianach

3. **TaskManagerMainForm.cs** - Interfejs użytkownika
   - DataGridView do wyświetlania zadań
   - Kontrolki do dodawania/edytowania zadań
   - Obsługa zdarzeń użytkownika
   - Zapis/odczyt z pliku

## 🚀 Installation and Setup

### System Requirements

- Windows 7/10/11
- .NET Framework 4.7.2 or newer
- Visual Studio 2019/2022 (for development)

### Running the Application

1. **From Visual Studio:**
   ```bash
   git clone [repository-url]
   cd TaskManagerDesktop
   # Open TaskManagerDesktop.sln in Visual Studio
   # Press F5 or Ctrl+F5
   ```

2. **From executable:**
   - Build project in Visual Studio (Build → Build Solution)
   - Navigate to `bin/Debug` or `bin/Release` folder
   - Run `TaskManagerDesktop.exe`

## 💾 Data Format

The application saves data to `tasks.dat` file in the following format:
```
ID|Title|Description|Category|Priority|IsCompleted|CreatedDate|CompletedDate
```

File is automatically created in the application folder and updated with every change.

## 🎯 Usage

1. **Adding a task:**
   - Fill in fields: Title, Description, Category, Priority
   - Click "Add Task"

2. **Editing a task:**
   - Select task from the list
   - Click "Edit Task"
   - Modify data and confirm

3. **Marking a task:**
   - Select task from the list
   - Click "Toggle Complete"

4. **Deleting a task:**
   - Select task from the list
   - Click "Delete Task"

## 🔧 Possible Extensions

- [ ] Export to CSV/JSON
- [ ] Advanced filters
- [ ] Task reminders
- [ ] Dark theme
- [ ] Cloud synchronization
- [ ] Statistics and reports
- [ ] Custom categories
- [ ] Task attachments

## 📝 License

MIT License - educational project

## 👨‍💻 Author

Project created as part of learning C# and Windows Forms programming.

---

**Last updated:** September 2025