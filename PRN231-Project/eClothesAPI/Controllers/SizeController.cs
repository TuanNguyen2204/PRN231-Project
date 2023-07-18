using AutoMapper;
using BusinessObjects.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;

namespace eClothesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SizeController : ControllerBase
    {
        private ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private IMapper _mapper;

        public SizeController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetSizes()
        {
            try
            {
                var Sizes = _repository.Size.GetAllSizes();
                _logger.LogInfo($"Returned all Sizes from database.");
                var SizeResults = _mapper.Map<IEnumerable<SizeDTO>>(Sizes);
                return Ok(SizeResults);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllSizes action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
