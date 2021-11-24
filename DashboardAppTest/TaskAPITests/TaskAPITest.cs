using Application.Tasks.Commands.CreateTask;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace DashboardAppTest
{
    [TestClass]
    public class TaskAPITest
    {
        [Test]
        public async Task ShouldCreateTask()
        {
            var userId = await Testing.CreateInitUser();

            var command = new CreateTaskCommand
            {
                TaskName = "task",
                IsCompleted = false,
            };
            var resultModel = await Testing.SendAsync(command);

            var item = await Testing.FindAsync<DashboardApp.Domain.Entities.Task>(Guid.Parse(resultModel.Data.ToString()));

            item.Should().NotBeNull();
            item.TaskName.Should().Be(command.TaskName);
            item.IsCompleted.Should().Be(command.IsCompleted);
            item.CreatedBy.Should().Be(userId);
        }
    }
}
