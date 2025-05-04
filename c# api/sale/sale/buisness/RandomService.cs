using ChneseSaleApi.models;
using ChneseSaleApi.repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using sale.repositories;
using System.ComponentModel;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Net.Mail;
using System.Net;
using System;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

using System.Net;
using System.Net.Mail;
using Microsoft.Identity.Client;
using sale.controllers;



namespace sale.buisness
{
    public class RandomService : ControllerBase, IRandomService
    {
        private readonly IRandomRepository _repository;
        private readonly IPurchaseService _purchaseService;
        private readonly IGiftService _giftService;
        private readonly ILogger<AuthController> _logger;

        public RandomService(IRandomRepository repository, IPurchaseService purchaseService, IGiftService giftService, ILogger<AuthController> logger)
        {
            _repository = repository;
            _purchaseService = purchaseService;
            _giftService = giftService;
            _logger = logger;
        }

        //get all winners
        public async Task<IEnumerable<Winner>> getAllWinners()
        {
            try
            {
                return await _repository.getAllWinners();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in getAllWinners: {ex.Message}");
                return Enumerable.Empty<Winner>();
            }
        }

        //random win to gift by user id and gift id
        public async Task<IActionResult> randomWin(int giftId)
        {
            try
            {
                IEnumerable<Gift> gifts = await _giftService.getAllGifts();
                Gift gift = gifts.First(g => g.Id == giftId);

                IActionResult result = await _purchaseService.getPurchasesByGiftId(giftId);
                if (result.GetType() == typeof(NotFoundObjectResult))
                    return new NotFoundObjectResult(result);

                if (gift.Status == false)
                    return new BadRequestObjectResult("The gift is already won");

                IEnumerable<Purchase> purchases = convertResultToIEnumerable(result);

                List<Customer> customers = new List<Customer>();
                foreach (Purchase p in purchases)
                {
                    for (int i = 0; i < p.Ammount; i++)
                    {
                        customers.Add(p.Customer);
                    }
                }

                Random random = new Random();
                int randomIndex = random.Next(0, customers.Count() - 1);
                Customer customer = purchases.ElementAt(randomIndex).Customer;

                Winner winner = new Winner();
                winner.GiftId = giftId;
                winner.CustomerId = customer.Id;
                _repository.addWinner(winner);

                gift.Status = false;
                await _giftService.updateGift(gift);

                return new OkObjectResult("The winner is: " + customer.FirstName + " " + customer.LastName);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in randomWin for gift id: {giftId}: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }


        //Make all gifts available
        public async Task<IActionResult> MakeAllGiftsAvailable()
        {
            try
            {
                return await _repository.MakeAllGiftsAvailable();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in MakeAllGiftsAvailable: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }


        //Total revenue
        public async Task<IActionResult> getTotalRevenue()
        {
            try
            {
                IEnumerable<Purchase> purchases = await _purchaseService.getAllPurchases();
                double totalRevenue = 0;
                foreach (Purchase p in purchases)
                {
                    totalRevenue += p.TotalPrice;
                }
                return new OkObjectResult(totalRevenue);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in getTotalRevenue: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }

        public IEnumerable<Purchase> convertResultToIEnumerable(IActionResult result)
        {
            if (result is OkObjectResult okResult)
                return okResult.Value as IEnumerable<Purchase>;
            else
                return Enumerable.Empty<Purchase>();
        }

        private async Task<bool> isFinishRandom()
        {
            IEnumerable<Gift> gifts = await _giftService.getAllGifts();
            foreach (Gift g in gifts)
            {
                if (g.Status == true)
                    return false;
            }
            return true;
        }


        //create winner report
        public async Task<IActionResult> createWinnerReport()
        {
            try
            {
                IEnumerable<Winner> winners = await _repository.getAllWinners();

                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Data");

                    // הגדרת כותרות
                    worksheet.Cells[1, 1].Value = "ID";
                    worksheet.Cells[1, 2].Value = "First Name";
                    worksheet.Cells[1, 3].Value = "Last Name";
                    worksheet.Cells[1, 4].Value = "Phone";
                    worksheet.Cells[1, 5].Value = "Email";
                    worksheet.Cells[1, 6].Value = "Address";
                    worksheet.Cells[1, 7].Value = "Gift Id";
                    worksheet.Cells[1, 8].Value = "Gift";

                    // הוספת נתונים לשורות
                    for (int i = 0; i < winners.Count(); i++)
                    {
                        worksheet.Cells[i + 2, 1].Value = winners.ElementAt(i).Id;
                        worksheet.Cells[i + 2, 2].Value = winners.ElementAt(i).Customer.FirstName;
                        worksheet.Cells[i + 2, 3].Value = winners.ElementAt(i).Customer.LastName;
                        worksheet.Cells[i + 2, 4].Value = winners.ElementAt(i).Customer.Phone;
                        worksheet.Cells[i + 2, 5].Value = winners.ElementAt(i).Customer.Mail;
                        worksheet.Cells[i + 2, 6].Value = winners.ElementAt(i).Customer.Address;
                        worksheet.Cells[i + 2, 7].Value = winners.ElementAt(i).Gift.Id;
                        worksheet.Cells[i + 2, 8].Value = winners.ElementAt(i).Gift.Name;
                    }

                    var stream = new MemoryStream();
                    package.SaveAs(stream);

                    // Save the file on the server if needed
                    //string folderPath = @"D:\End Project\api\דוחות זוכים";
                    string excelName = $"Data-{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                    //string fullPath = Path.Combine(folderPath, excelName);
                    //FileInfo file = new FileInfo(fullPath);
                    //package.SaveAs(file);

                    // Prepare the file for download
                    stream.Position = 0; // Reset the stream position to the beginning
                    string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    return File(stream, contentType, excelName);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error in createWinnerReport: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }

        //send mail to the winners
        public async Task<IActionResult> sendMailToWinners()
        {
            try
            {
                IEnumerable<Winner> winners = await _repository.getAllWinners();
                foreach (Winner w in winners)
                {
                    await SendMail(w.Customer.Mail, w.Gift.Name);
                }
                return new OkObjectResult("Mails sent successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in sendMailToWinners: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }

        private async Task SendMail(string mail, string name)
        {
            var clientId = "1fd20414-3231-4276-8034-ca132bc02f46"; // הכנס את ה-ID של האפליקציה שלך
            var tenantId = "f8cdef31-a31e-4b4a-93e4-5f571e91255a"; // הכנס את ה-ID של השוכר שלך
            var clientSecret = "jnj8Q~mE_iSzBHqx5LM6Zj4vDqW1-KPXX5Khybu1"; // הכנס את הסוד של הלקוח שלך
            var scope = "https://graph.microsoft.com/.default"; // טווח עבור Microsoft Graph

            // השתמש ב-MSAL כדי להשיג את טוקן הגישה
            IConfidentialClientApplication app = ConfidentialClientApplicationBuilder.Create(clientId)
                .WithClientSecret(clientSecret)
                .WithAuthority(new Uri($"https://login.microsoftonline.com/{tenantId}/v2.0"))
                .Build();

            var result = await app.AcquireTokenForClient(new[] { scope }).ExecuteAsync();
            var accessToken = result.AccessToken;

            var fromAddress = new MailAddress("chneseSaleChani@outlook.co.il", "חני מירניק");
            var toAddress = new MailAddress(mail, "winner");
            const string subject = "אתה המנצח!";
            var body = $"מזל טוב! אתה המנצח של המתנה: {name}. אנא צור קשר כדי לקבל את הפרס שלך.";

            // הגדרות SMTP
            var smtp = new SmtpClient
            {
                Host = "smtp.office365.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("chneseSaleChani@outlook.co.il", accessToken) // השתמש בטוקן הגישה לאימות
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
    }
}


