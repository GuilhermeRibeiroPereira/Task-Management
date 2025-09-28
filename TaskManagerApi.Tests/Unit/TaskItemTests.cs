using TaskManagerApi.Models;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace TaskManagerApi.Tests.Unit;

public class TaskItemTests
{
    [Fact]
    public void Create_Task()
    {
        var task = new TaskItem
        {
            Title = "Test",
            Description = "Check creation",
            Status = "Done",
            Priority = 1
        };

        Assert.Equal("Test", task.Title);
        Assert.Equal("Check creation", task.Description);
        Assert.Equal("Done", task.Status);
        Assert.Equal(1, task.Priority);
    }

    [Fact]
    public void Create_WithMissingValues()
    {
        var task = new TaskItem
        {
            Title = "Test",
        };

        Assert.Equal("Test", task.Title);
        Assert.Equal(string.Empty, task.Description);
        Assert.Equal("Todo", task.Status);
        Assert.Equal(0, task.Priority);
    }

    [Fact]
    public void Create_WithNoTitle()
    {
        var task = new TaskItem(); // no Title

        var results = Validate(task);
        Assert.Contains(results, v => v.ErrorMessage == "Please enter a Title");
    }

    [Fact]
    public void Create_ViolateTitleMaxLength()
    {
        var task = new TaskItem
        {
            Title = "TESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTESTTEST" // violates max length
        };

        var results = Validate(task);
        Assert.Contains(results, v => v.ErrorMessage == "Title length can't be more than 100");
    }

    [Fact]
    public void Create_ViolateTitleMinLength()
    {
        var task = new TaskItem
        {
            Title = "T" // violates minimum length
        };

        var results = Validate(task);
        Assert.Contains(results, v => v.ErrorMessage == "Title length must be at least 2");
    }

    [Fact]
    public void Create_ViolateMinPriority()
    {
        var task = new TaskItem
        {
            Title = "Test",
            Description = "Check creation",
            Status = "Done",
            Priority = -1 // violates min value
        };

        var results = Validate(task);
        Assert.Contains(results, v => v.ErrorMessage == "Priority must be greater or equal to 0");
    }

    private List<ValidationResult> Validate(object model)
    {
        // validate the task object uising DataAnnotations
        var results = new List<ValidationResult>();
        var context = new ValidationContext(model);
        Validator.TryValidateObject(model, context, results, validateAllProperties: true);
        return results;
    }
}