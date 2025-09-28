using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using TaskManagerApi.Models;
using TaskManagerApi.Data; 
using Xunit;

namespace TaskManagerApi.Tests.Integration;

public class TaskManagerApiTests : IClassFixture<TaskManagerWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public TaskManagerApiTests(TaskManagerWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateTask_ShouldReturnCreatedTask()
    {
        var newTask = new TaskItem
        {
            Title = "Test",
            Status = "Todo",
            Priority = 1
        };
        var postResponse = await _client.PostAsJsonAsync("/tasks", newTask);

        var createdTask = await postResponse.Content.ReadFromJsonAsync<TaskItem>();
        Assert.NotNull(createdTask);
        Assert.Equal("Test", createdTask.Title);
    }

    [Fact]
    public async Task CreateTask_ShouldReturnBadRequest_WhenInvalid()
    {
        var newTask = new TaskItem
        {
            Title = "", // Invalid: Title is required
        };
        var postResponse = await _client.PostAsJsonAsync("/tasks", newTask);
        Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);
    }

    [Fact]
    public async Task GetTasks_WhenTasksExist_ReturnsTasks()
    {
        var newTask = new TaskItem
        {
            Id = 2,
            Title = "Test",
            Status = "Todo",
            Priority = 1
        };
        var postResponse = await _client.PostAsJsonAsync("/tasks", newTask);
        Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);  // Verify the creation

        var getResponse = await _client.GetAsync("/tasks");

        var tasks = await getResponse.Content.ReadFromJsonAsync<List<TaskItem>>();
        Assert.NotNull(tasks);
        Assert.True(tasks.Count > 0); // At least one tasks
        Assert.Equal("Test", tasks[2].Title);
    }

    [Fact]
    public async Task GetTask_WhenExists_ReturnsTask()
    {
        var newTask = new TaskItem
        {
            Id = 3,
            Title = "Test",
            Status = "Todo",
            Priority = 1
        };
        var postResponse = await _client.PostAsJsonAsync("/tasks", newTask);
        Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);  // Verify the creation

        var getResponse = await _client.GetAsync("/tasks/3");

        var task = await getResponse.Content.ReadFromJsonAsync<TaskItem>();
        Assert.NotNull(task);
        Assert.Equal(3, task.Id);
        Assert.Equal("Test", task.Title);
    }

    [Fact]
    public async Task GetTask_WhenNotExists_ReturnsNotFound()
    {
        var getResponse = await _client.GetAsync("/tasks/999");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task UpdateTask_WhenExists_ReturnsNoContent()
    {
        var newTask = new TaskItem
        {
            Id = 4,
            Title = "Test",
            Status = "Todo",
            Priority = 1
        };
        var postResponse = await _client.PostAsJsonAsync("/tasks", newTask);
        Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);  // Verify the creation

        var updatedTask = new TaskItem
        {
            Id = 4,
            Title = "Updated Test",
            Status = "InProgress",
            Priority = 2
        };
        var putResponse = await _client.PutAsJsonAsync("/tasks/4", updatedTask);
        Assert.Equal(HttpStatusCode.NoContent, putResponse.StatusCode);

        // Verify the update
        var getResponse = await _client.GetAsync("/tasks/4");
        var task = await getResponse.Content.ReadFromJsonAsync<TaskItem>();
        Assert.NotNull(task);
        Assert.Equal("Updated Test", task.Title);
        Assert.Equal("InProgress", task.Status);
        Assert.Equal(2, task.Priority);
    }

    [Fact]
    public async Task UpdateTask_WhenNotExists_ReturnsNotFound()
    {
        var updatedTask = new TaskItem
        {
            Id = 999,
            Title = "Updated Test",
            Status = "InProgress",
            Priority = 2
        };
        var putResponse = await _client.PutAsJsonAsync("/tasks/999", updatedTask);
        Assert.Equal(HttpStatusCode.NotFound, putResponse.StatusCode);
    }

    [Fact]
    public async Task UpdateTask_WhenIdsNotMatch_ReturnsBadRequest()
    {
        var updatedTask = new TaskItem
        {
            Id = 998,
            Title = "Updated Test",
            Status = "InProgress",
            Priority = 2
        };
        var putResponse = await _client.PutAsJsonAsync("/tasks/999", updatedTask);
        Assert.Equal(HttpStatusCode.BadRequest, putResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteTask_WhenExists_ReturnsNoContent()
    {
        var newTask = new TaskItem
        {
            Id = 5,
            Title = "Test",
            Status = "Todo",
            Priority = 1
        };
        var postResponse = await _client.PostAsJsonAsync("/tasks", newTask);
        Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);  // Verify the creation

        var deleteResponse = await _client.DeleteAsync("/tasks/5");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        // Verify the task is deleted
        var getResponse = await _client.GetAsync("/tasks/5");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteTask_WhenNotExists_ReturnsNotFound()
    {   
        var deleteResponse = await _client.DeleteAsync("/tasks/999");
        Assert.Equal(HttpStatusCode.NotFound, deleteResponse.StatusCode);
    }
}
