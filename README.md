Task Manager Desktop
Simple desktop task management application built with C# and Windows Forms.
📋 Description
Task Manager Desktop is an intuitive application for managing daily tasks. The application allows you to add, edit, delete, and mark tasks as complete, with automatic data persistence to file.
✨ Features

Add Tasks - Create new tasks with title, description, category, and priority
Edit Tasks - Modify existing tasks
Delete Tasks - Remove unwanted tasks
Toggle Task Status - Mark tasks as completed/incomplete
Task Categories - Work, Home, Shopping, Other
Priority Levels - Low, Medium, High, Critical
Auto Save - Data automatically saved to tasks.dat file
Data Persistence - Tasks are restored after application restart

🛠️ Technologies

C# .NET Framework - Programming language and platform
Windows Forms - User interface framework
Visual Studio - Development environment

🏗️ Architecture
Main Classes

Task.cs - Task data model

Properties: Id, Title, Description, Category, Priority, IsCompleted, CreatedDate, CompletedDate
Enums: TaskCategory, TaskPriority
Methods: MarkAsCompleted(), MarkAsIncomplete()


TaskManager.cs - Business logic

Task list management
CRUD operations (Create, Read, Update, Delete)
Task filtering and sorting
Event notifications for changes


TaskManagerMainForm.cs - User interface

DataGridView for displaying tasks
Controls for adding/editing tasks
User event handling
File save/load functionality