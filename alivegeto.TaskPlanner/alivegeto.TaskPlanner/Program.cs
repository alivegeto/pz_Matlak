using alivegeto.TaskPlanner.DataAccess;
using alivegeto.TaskPlanner.Domain.Logic;
using alivegeto.TaskPlanner.Domain.Models;
using alivegeto.TaskPlanner.Domain.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

internal static class Program
{
    public static void Main(string[] args)
    {
       
        var repository = new FileWorkItemsRepository();

        
        var planner = new SimpleTaskPlanner(repository);

       
        while (true)
        {
            Console.WriteLine("\nОберіть дію:");
            Console.WriteLine("[A] — Додати завдання");
            Console.WriteLine("[B] — Створити план");
            Console.WriteLine("[M] — Відмітити завдання як виконане");
            Console.WriteLine("[R] — Видалити завдання");
            Console.WriteLine("[Q] — Вихід");

            var choice = Console.ReadLine()?.ToUpper();

            switch (choice)
            {
                case "A":
                    AddWorkItem(repository);
                    break;

                case "B":
                    BuildPlan(planner);
                    break;

                case "M":
                    MarkWorkItemCompleted(repository);
                    break;

                case "R":
                    RemoveWorkItem(repository);
                    break;

                case "Q":
                    Console.WriteLine("Вихід з програми...");
                    repository.SaveChanges();
                    return;

                default:
                    Console.WriteLine("Невідома команда. Спробуйте ще раз.");
                    break;
            }
        }
    }

    static void AddWorkItem(FileWorkItemsRepository repository)
    {
        Console.Write("Введіть назву завдання: ");
        var title = Console.ReadLine();

        Console.Write("Введіть дату дедлайну (yyyy-MM-dd): ");
        var dueDateStr = Console.ReadLine();
        DateTime.TryParse(dueDateStr, out DateTime dueDate);

        Console.WriteLine("Оберіть пріоритет: 1 - High, 2 - Medium, 3 - Low");
        var priorityStr = Console.ReadLine();
        Priority priority = priorityStr switch
        {
            "1" => Priority.High,
            "2" => Priority.Medium,
            _ => Priority.Low
        };

        var workItem = new WorkItem
        {
            Title = title,
            DueDate = dueDate,
            Priority = priority,
            IsCompleted = false
        };

        repository.Add(workItem);
        repository.SaveChanges();

        Console.WriteLine("✅ Завдання додано!");
    }

    static void BuildPlan(SimpleTaskPlanner planner)
    {
        var plan = planner.CreatePlan();

        Console.WriteLine("\n📋 Список завдань у правильному порядку:");
        foreach (var item in plan)
        {
            Console.WriteLine($"- {item.Title} (Пріоритет: {item.Priority}, дедлайн: {item.DueDate:dd.MM.yyyy}, виконано: {item.IsCompleted})");
        }
    }

    static void MarkWorkItemCompleted(FileWorkItemsRepository repository)
    {
        Console.Write("Введіть назву завдання, яке виконано: ");
        var title = Console.ReadLine();

        var item = repository.GetAll().FirstOrDefault(x =>
            x.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

        if (item != null)
        {
            item.IsCompleted = true;
            repository.Update(item);
            repository.SaveChanges();
            Console.WriteLine("✅ Завдання відмічено як виконане!");
        }
        else
        {
            Console.WriteLine("❌ Завдання не знайдено.");
        }
    }

    static void RemoveWorkItem(FileWorkItemsRepository repository)
    {
        Console.Write("Введіть назву завдання для видалення: ");
        var title = Console.ReadLine();

        var item = repository.GetAll().FirstOrDefault(x =>
            x.Title.Equals(title, StringComparison.OrdinalIgnoreCase));

        if (item != null)
        {
            repository.Remove(item.Id);
            repository.SaveChanges();
            Console.WriteLine("🗑️ Завдання видалено!");
        }
        else
        {
            Console.WriteLine("❌ Завдання не знайдено.");
        }
    }
}
