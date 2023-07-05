using AutoMapper;
using BusinessObjects.DTOs;
using BusinessObjects.QueryParameters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;

namespace eClothesAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private IMapper _mapper;

        public UserController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetUsers([FromQuery] UserParameters UserParameters)
        {
            try
            {
                var Users = _repository.User.GetUsers(UserParameters);
                _logger.LogInfo($"Returned all Users from database.");
                var UsersResult = _mapper.Map<IEnumerable<UserDTO>>(Users);
                return Ok(UsersResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetUsers action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpGet("{id}")]
        public IActionResult GetUserDetail(int id)
        {
            try
            {
                var User = _repository.User.GetUserById(id);
                if (User == null)
                {
                    _logger.LogError($"User with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned owner with details for id: {id}");

                    var UserResult = _mapper.Map<UserDTO>(User);
                    return Ok(UserResult);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetUserDetail action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UserDTO UserDto)
        {
            try
            {
                if (UserDto is null)
                {
                    _logger.LogError("User object sent from client is null.");
                    return BadRequest("User object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid User object sent from client.");
                    return BadRequest("Invalid model object");
                }
                var UserEntity = _repository.User.GetUserById(id);
                if (UserEntity is null)
                {
                    _logger.LogError($"User with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                _mapper.Map(UserDto, UserEntity);
                _repository.User.UpdateUser(UserEntity);
                _repository.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateUser action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                var User = _repository.User.GetUserById(id);
                if (User == null)
                {
                    _logger.LogError($"User with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                _repository.User.DeleteUser(User);
                _repository.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteUser action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [ActionName("ExportExcel")]
        public IActionResult GetExportExcel()
        {
            try
            {
                var Users = _repository.User.ExportExel();
                _logger.LogInfo($"Returned all Users from database.");
                var UsersResult = _mapper.Map<IEnumerable<UserDTO>>(Users);
                return Ok(UsersResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllOwners action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }

}
