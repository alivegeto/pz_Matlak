using alivegeto.TaskPlanner.DataAccess.Abstractions;
using alivegeto.TaskPlanner.Domain.Models.Enums;
using alivegeto.TaskPlanner.Domain.Logic;
using alivegeto.TaskPlanner.Domain.Models;
using Moq;
using System;
using Xunit;

namespace alivegeto.TaskPlanner.Domain.Logic.Tests
{
    public class SimpleTaskPlannerTests
    {
        private WorkItem[] GetSampleTasks()
        {
            return new WorkItem[]
            {
                new WorkItem { Id = Guid.NewGuid(), Title = "Alpha", Done = false, Priority = Priority.Medium, DueDate = DateTime.Today.AddDays(3) },
                new WorkItem { Id = Guid.NewGuid(), Title = "Bravo", Done = true, Priority = Priority.High, DueDate = DateTime.Today.AddDays(1) },
                new WorkItem { Id = Guid.NewGuid(), Title = "Charlie", Done = false, Priority = Priority.High, DueDate = DateTime.Today },
                new WorkItem { Id = Guid.NewGuid(), Title = "Delta", Done = false, Priority = Priority.Low, DueDate = DateTime.Today.AddDays(2) }
            };
        }

        [Fact]
        public void CreatePlan_ReturnsOnlyIncompleteTasks()
        {
            // Arrange
            var mockRepo = new Mock<IWorkItemsRepository>();
            mockRepo.Setup(r => r.GetAll()).Returns(GetSampleTasks());
            var planner = new SimpleTaskPlanner(mockRepo.Object);

            // Act
            var plan = planner.CreatePlan();

            // Assert
            Assert.DoesNotContain(plan, t => t.Done);       // Жодне завдання не виконане
            Assert.Equal(3, plan.Length);                   // Всього 3 невиконані завдання
        }

        [Fact]
        public void CreatePlan_SortsTasksByPriorityThenDueDateThenTitle()
        {
            // Arrange
            var mockRepo = new Mock<IWorkItemsRepository>();
            mockRepo.Setup(r => r.GetAll()).Returns(GetSampleTasks());
            var planner = new SimpleTaskPlanner(mockRepo.Object);

            // Act
            var plan = planner.CreatePlan();

            // Assert
            Assert.Equal("Charlie", plan[0].Title);  // High priority, earliest due date
            Assert.Equal("Alpha", plan[1].Title);    // Medium priority
            Assert.Equal("Delta", plan[2].Title);    // Low priority
        }

        [Fact]
        public void CreatePlan_IncludesAllRelevantTasks()
        {
            // Arrange
            var mockRepo = new Mock<IWorkItemsRepository>();
            mockRepo.Setup(r => r.GetAll()).Returns(GetSampleTasks());
            var planner = new SimpleTaskPlanner(mockRepo.Object);

            // Act
            var plan = planner.CreatePlan();

            // Assert
            var titles = new[] { "Alpha", "Charlie", "Delta" };
            foreach (var title in titles)
            {
                Assert.Contains(plan, t => t.Title == title);
            }
        }
    }
}
