using alivegeto.TaskPlanner.Domain.Logic;
using alivegeto.TaskPlanner.Domain.Models;
using alivegeto.TaskPlanner.Domain.Models.Enums;
using System;

internal static class Program
{
    public static void Main(string[] args)
    {
        var items = new WorkItem[]
        {
            new WorkItem { Title = "Зробити домашнє завдання", DueDate = new DateTime(2025, 9, 25), Priority = Priority.Medium },
            new WorkItem { Title = "Купити продукти", DueDate = new DateTime(2025, 9, 23), Priority = Priority.Low },
            new WorkItem { Title = "Погуляти з собакою", DueDate = new DateTime(2025, 9, 22), Priority = Priority.High },
        };

        var planner = new SimpleTaskPlanner();
        var plan = planner.CreatePlan(items);

        Console.WriteLine("Список завдань у правильному порядку:");
        foreach (var item in plan)
        {
            Console.WriteLine($"- {item.Title} (Пріоритет {item.Priority}, дедлайн {item.DueDate:dd.MM.yyyy})");
        }
    }
}
