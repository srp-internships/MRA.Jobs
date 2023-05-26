namespace MockTestingApi.Entities;

public class CreateTestRequest
{
    public List<string> Categories { get; set; }
    public int QuestionCount { get; set; }
}
