namespace ApiTester.Models;

public class Statement
{
    public int Id { get; set; }
    public int SeekerId { get; set; }
    public int Status { get; set; }
    public int Value { get; set; }
    public string PassTime { get; set; } = "0";
    public int SuperVisorId { get; set; }
}

public enum StatementStatus
{
    Created = 1,
    Done,
    Checked
}