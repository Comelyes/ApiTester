using ApiTester.ApiRequests;
using ApiTester.Models;
using Newtonsoft.Json;

namespace ApiTester;

public class Logic
{
    public static async Task Start()
    {
        PerformFullLogic();
    }
    private static async Task PerformFullLogic()
    {
        await Task.Delay(1000);
        
        Seeker seeker = new Seeker()
        {
            Name = "Ivan",
            LastName = "Seeker",
            Patronymic = "Ivanovich",
            FirstStatementTime = DateTime.UtcNow,
            Number = 8901,
            PostId = 3, // id 3 = Программист
            TaskTime = DateTime.UtcNow + TimeSpan.FromDays(3),
            WorkerId = 2 // Руководитель 
        };
        Statement statement = new Statement()
        {
            Status = StatementStatus.Created.GetHashCode(),
            SuperVisorId = 2, // Руководитель
            Value = -1,
        };

        // Добавление нового соискателя, который не выполнит задание
        var data = $"name=Alex&lastName={seeker.LastName}&number={seeker.Number}&patronymic={seeker.Patronymic}&postId={seeker.PostId}" +
                   $"&taskTime={(DateTime.UtcNow + TimeSpan.FromSeconds(20)).ToString()}&workerId={seeker.WorkerId}";
        var response = ApiRequest.PostRequest("https://localhost:7179/api/newseeker", data);
        seeker.Id = Int32.Parse(response);
        statement.SeekerId = seeker.Id;
        Console.WriteLine($"Seeker id {response}");
        
        // Предоставление задания 
        data = $"seekerId={statement.SeekerId}&superVisorId={statement.SuperVisorId}";
        response = ApiRequest.PostRequest("https://localhost:7179/api/newstatement", data);
        statement.Id = Int32.Parse(response);
        
        // Добавление нового соискателя 
        data = $"name={seeker.Name}&lastName={seeker.LastName}&number={seeker.Number}&patronymic={seeker.Patronymic}&postId={seeker.PostId}" +
                   $"&taskTime={seeker.TaskTime.ToString()}&workerId={seeker.WorkerId}";
        response = ApiRequest.PostRequest("https://localhost:7179/api/newseeker", data);
        seeker.Id = Int32.Parse(response);
        statement.SeekerId = seeker.Id;
        Console.WriteLine($"Seeker id {response}");

        // Предоставление задания 
        data = $"seekerId={statement.SeekerId}&superVisorId={statement.SuperVisorId}";
        response = ApiRequest.PostRequest("https://localhost:7179/api/newstatement", data);
        statement.Id = Int32.Parse(response);
        
        // Изменение статуса задания (Сдача)
        statement.Status = StatementStatus.Done.GetHashCode();
        data = $"statementId={statement.Id}&status={statement.Status}&value={statement.Value}";
        ApiRequest.PostRequest("https://localhost:7179/api/updatestatement", data);
        
        // Загрузка данных о задании, изменение статуса - проверка
        response = ApiRequest.PostRequest("https://localhost:7179/api/getstatement", $"statementId={statement.Id}");
        Statement dbStatement = JsonConvert.DeserializeObject<Statement>(response);
        Console.WriteLine($"Result from db with id {dbStatement.Id} and pass time {dbStatement.PassTime}");

        dbStatement.Status = StatementStatus.Checked.GetHashCode();
        dbStatement.Value = 4;

        data = $"statementId={dbStatement.Id}&status={dbStatement.Status}&value={dbStatement.Value}&passTime={dbStatement.PassTime}";
        response = ApiRequest.PostRequest("https://localhost:7179/api/updatestatement", data);
        
        // Загрузка всех соискателесь (Без фильтров)
        response = ApiRequest.PostRequest("https://localhost:7179/api/seekersinfo", $"id=1");
        
        // Загрузка всех соискателей с фильтрами
        response = ApiRequest.PostRequest("https://localhost:7179/api/seekersinfo", $"id=1&fromTime={DateTime.UtcNow-TimeSpan.FromHours(255)}&endTime={DateTime.UtcNow}");
        Console.WriteLine($"Все соискатели с фильтрами по времени:\n{response}");

    }
}