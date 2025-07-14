namespace TaskManager.Application.DTOs;

public class AddCommentDto
{
    public int TaskId { get; set; }
    public string Content { get; set; } = string.Empty;
    public int UserId { get; set; }
}
