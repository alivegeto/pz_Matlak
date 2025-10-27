using alivegeto.TaskPlanner.Domain.Models.Enums;

namespace alivegeto.TaskPlanner.Domain.Models
{
    public class WorkItem
    {
        public Guid Id { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime DueDate { get; set; }
        public Priority Priority { get; set; }
        public Complexity Complexity { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public bool Done { get; set; }

        public WorkItem Clone()
        {
            return new WorkItem
            {
                Id = this.Id,
                Done = this.Done,
                CreationDate = this.CreationDate,
                DueDate = this.DueDate,
                Priority = this.Priority,
                Complexity = this.Complexity,
                Title = this.Title,
                Description = this.Description,
                IsCompleted = this.IsCompleted
            };
        }
        public override string ToString()
        {
            return $"{Title}: due {DueDate:dd.MM.yyyy}, {Priority.ToString().ToLower()} priority";
        }
    }
}
