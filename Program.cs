using ApiTester;
using ApiTester.ApiRequests;

Console.WriteLine("Hello, World!");

// Загрузка всех соискателей с фильтрами
var response = ApiRequest.PostRequest("https://localhost:7179/api/seekersinfo", $"id=1&fromTime={DateTime.UtcNow-TimeSpan.FromHours(255)}&endTime={DateTime.UtcNow}");
Console.WriteLine(response);

await Logic.Start();

Console.ReadLine();

