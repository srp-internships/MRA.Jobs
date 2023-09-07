using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MRA.Identity.Application.Common.Interfaces.Services;
using MRA.Identity.Domain.Entities;
using MRA.Identity.Infrastructure.Persistence;

namespace MRA.Identity.Infrastructure.Services;
public class SmsService : ISmsService
{
    private readonly HttpClient _client;
    private readonly ApplicationDbContext _context;

    public SmsService(HttpClient client, ApplicationDbContext context)
    {
        _client = client;
        _context = context;
    }
    public async Task<string> SendSms(string phoneNumber)
    {
        int code;
        var config = new Dictionary<string, string>();
        config["dlm"] = ";"; // не надо менять!!! 
        config["t"] = "23"; // не надо менять!!!

        config["login"] = "Ваш логин"; // Ваш логин
        config["pass_hash"] = "Ваш хэш код"; // Ваш хэш код
        config["sender"] = "Ваш алфанумерик"; // Ваш алфанумерик

        var txn_id = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds; // Должен быть уникальным для каждого запроса

        var str_hash = Sha256Hash(txn_id + config["dlm"] + config["login"] + config["dlm"] + config["sender"] + config["dlm"] + phoneNumber + config["dlm"] + config["pass_hash"]);

        string url = $"https://api.osonsms.com/sendsms_v1.php?login={config["login"]}&from={config["sender"]}&phone_number={phoneNumber}&msg={GenerateMessage(out code)}&txn_id={txn_id}&str_hash={str_hash}";

        var confirmationCodeTable = new ConfirmationCode
        {
            PhoneNumber = phoneNumber,
            Code = code
        };
        HttpResponseMessage response = await _client.GetAsync(url);
        await _context.ConfirmationCodes.AddAsync(confirmationCodeTable);
        await _context.SaveChangesAsync();

        return await response.Content.ReadAsStringAsync();
    }

    private static String Sha256Hash(String value)
    {
        StringBuilder Sb = new StringBuilder();

        using (SHA256 hash = SHA256Managed.Create())
        {
            Encoding enc = Encoding.UTF8;
            Byte[] result = hash.ComputeHash(enc.GetBytes(value));

            foreach (Byte b in result)
                Sb.Append(b.ToString("x2"));
        }

        return Sb.ToString();
    }

    private static String GenerateMessage(out int code)
    {
        Random random = new Random();
        code = random.Next(1000, 10001);
        return $"Your confirmation code is: {code}. Please enter this code to verify your phone number.";
    }

    public async Task<bool> CheckCode(string phoneNumber, int code)
    {
        var expirationTime = DateTime.Now.AddMinutes(-1); 

        var result = await _context.ConfirmationCodes
            .FirstOrDefaultAsync(c => c.Code == code && c.PhoneNumber == phoneNumber && c.SentAt >= expirationTime);

        if (result != null)
        {
            return true;
        }
        return false;
    }
}
