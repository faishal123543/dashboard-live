namespace LoanDashboard.Models;

public class StageCountDto
{
    public int StageNo { get; set; }
    public string StageName { get; set; } = string.Empty;
    public int Count { get; set; }
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}
