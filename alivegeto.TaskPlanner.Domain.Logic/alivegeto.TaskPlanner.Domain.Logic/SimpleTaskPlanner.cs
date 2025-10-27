using alivegeto.TaskPlanner.DataAccess;
using alivegeto.TaskPlanner.DataAccess.Abstractions;
using alivegeto.TaskPlanner.Domain.Models;
using System;
using System.Linq;

namespace alivegeto.TaskPlanner.Domain.Logic
{
    public class SimpleTaskPlanner
    {
        private readonly IWorkItemsRepository _repository;

        
        public SimpleTaskPlanner(IWorkItemsRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // Створює план, отримуючи задачі з репозиторія
        public WorkItem[] CreatePlan()
        {
            var items = _repository.GetAll();

            return items
                .Where(x => !x.Done)
                .OrderByDescending(x => x.Priority)  // 1. Пріоритет (High > Medium > Low)
                .ThenBy(x => x.DueDate)              // 2. Дата дедлайну (раніше — перше)
                .ThenBy(x => x.Title)                // 3. Алфавітний порядок
                .ToArray();
        }
    }
}
