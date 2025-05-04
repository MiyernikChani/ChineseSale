using ChneseSaleApi.models;
using ChneseSaleApi.repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using sale.buisness;
using sale.controllers;
using sale.repositories;
using System.Diagnostics.Eventing.Reader;
using System.Threading.Tasks;

namespace ChneseSaleApi.buisness
{
    public class DonatorService:IDonatorService
    {
        private readonly IDonatorRepository _repository;
        private readonly IGiftRepository _giftRepository;
        private readonly IGiftService _giftService;
        private readonly ILogger<AuthController> _logger;

        public DonatorService(IDonatorRepository repository, IGiftRepository giftRepository, IGiftService giftService, ILogger<AuthController> logger)
        {
            _repository = repository;
            _giftRepository = giftRepository;
            _giftService = giftService;
            _logger = logger;
        }

        //get all donators
        public async Task<IEnumerable<Donator>> GetAllDonators()
        {
            try
            {
                return await _repository.GetAllDonators();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetAllDonators: {ex.Message}");
                return Enumerable.Empty<Donator>();
            }
        }

        //create donator
        public async Task<IActionResult> AddDonator(Donator donator)
        {
            try
            {
                IActionResult isExist = IsDonatorExist(donator);
                if (isExist != null)
                    return isExist;
                Donator newDonator = new Donator();
                newDonator.Name = donator.Name;
                newDonator.Phone = donator.Phone;
                newDonator.Mail = donator.Mail;
                newDonator.Address = donator.Address;
                return await _repository.AddDonator(newDonator);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in AddDonator: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }

        //delete a donator and delete donator gifts
        public async Task<IActionResult> deleteDonator(int id)
        {
            try
            {
                IEnumerable<Donator> donators = await _repository.GetAllDonators();
                Donator donator = donators.FirstOrDefault(d => d.Id == id);
                if (donator == null)
                    return new BadRequestObjectResult("There is no donator id = " + id);
                foreach (Gift g in donator.Gifts)
                {
                    IActionResult result = await _giftService.deleteGift(g.Id);
                    if (result.GetType() == typeof(BadRequestObjectResult))
                        return new BadRequestObjectResult("can't delete user by id: " + id + " because " + result);
                }
                return await _repository.deleteDonator(donator);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in deleteDonator: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }

        //update donator
        public async Task<IActionResult> updateDonator(Donator donator)
        {
            try
            {
                IEnumerable<Donator> donators = await _repository.GetAllDonators();
                Donator d = donators.FirstOrDefault(d => d.Id == donator.Id);
                if (d == null)
                    return new BadRequestObjectResult("There is no donator id = " + donator.Id);
                else
                {
                    d.Name = donator.Name;
                    d.Phone = donator.Phone;
                    d.Mail = donator.Mail;
                    d.Address = donator.Address;
                    d.Gifts = donator.Gifts;

                    foreach (Donator x in donators)
                    {
                        if (x.Name.Equals(donator.Name) && x.Mail.Equals(donator.Mail) && x.Id != donator.Id)
                            return new BadRequestObjectResult("donator already exists");
                    }

                    return await _repository.updateDonator(d);
                }
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error in updateDonator: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }

        //filter donator by name\mail\gift
        public async Task<IActionResult> filterDonator(string filterBy, string value)
        {
            try
            {
                IEnumerable<Donator> donators = await _repository.GetAllDonators();
                IEnumerable<Donator> d = new List<Donator>();
                switch (filterBy)
                {
                    case "name":
                        d = donators.Where(x => x.Name.Equals(value));
                        break;
                    case "email":
                        d = donators.Where(x => x.Mail.Equals(value));
                        break;
                    case "gift":
                        d = donators.Where(x => x.Gifts.Any(g => g.Name.Equals(value)));
                        break;
                    default:
                        return new BadRequestObjectResult("no filter by " + filterBy + " field");
                }

                if (d.IsNullOrEmpty())
                    return new BadRequestObjectResult("no donator by " + filterBy + ": " + value + ".");
                return new OkObjectResult(d);
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error in filterDonator: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }

        private IActionResult IsDonatorExist(Donator donator)
        {
            try
            {
                IEnumerable<Donator> donators = _repository.GetAllDonators().Result;
                foreach (Donator d in donators)
                {
                    if (d.Name.Equals(donator.Name) && d.Mail.Equals(donator.Mail))
                        return new BadRequestObjectResult("donator already exists");
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in IsDonatorExist: {ex.Message}");
                return new StatusCodeResult(500);
            }
        }
    }
}
