using System.ComponentModel.DataAnnotations;
namespace TaskManagerApi.Models;

public class TaskItem {
    public int Id { get; set; }
    [Required(ErrorMessage="Please enter a Title")]
    [StringLength(100, ErrorMessage = "Title length can't be more than 100")]
    [MinLength(2, ErrorMessage = "Title length must be at least 2")]
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = "Todo"; // "Todo", "InProgress", "Done"
    [Range(0, int.MaxValue, ErrorMessage = "Priority must be greater or equal to 0")]
    public int Priority { get; set; } = 0; // higher number = higher priority
}