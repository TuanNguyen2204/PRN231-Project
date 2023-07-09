using AutoMapper;
using BusinessObjects.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;

namespace eClothesAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private ILoggerManager _logger;
        private readonly IDashboardRepository _repository;
        private IMapper _mapper;

        public DashboardController(ILoggerManager logger, IDashboardRepository repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult Top5BestSelers() { 
            List<BestSeller> bestSellers = _repository.GetBestSellers();
            return Ok(bestSellers);
        }

        [HttpGet]
        public IActionResult Top5Users() {
            List<TopUser> bestUsers = _repository.GetTopUsers();
            return Ok(bestUsers);   
        }
        [HttpGet]
        public IActionResult RevenueMonths()
        {
            List<RevenuePerMonth> RevenueMonths = _repository.GetRevenuePerMonth();
            return Ok(RevenueMonths);
        }
    }
}
