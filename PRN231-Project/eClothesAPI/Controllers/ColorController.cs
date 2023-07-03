using AutoMapper;
using BusinessObjects.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;

namespace eClothesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorController : ControllerBase
    {
        private ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private IMapper _mapper;

        public ColorController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetColors() {
            try
            {
                var colors = _repository.Color.GetAllColors();
                _logger.LogInfo($"Returned all colors from database.");
                var colorResults = _mapper.Map<IEnumerable<ColorDTO>>(colors);
                return Ok(colorResults);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllColors action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
