using AutoMapper;
using BusinessObjects.DTOs;
using BusinessObjects.Models;
using BusinessObjects.QueryParameters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
                var metadata = new
                {
                    Users.TotalCount,
                    Users.PageSize,
                    Users.CurrentPage,
                    Users.TotalPages,
                    Users.HasNext,
                    Users.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));
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
        [HttpPost]
        public IActionResult CreateUser([FromBody] UserCreateDTO user)
        {
            try
            {
                if (user is null)
                {
                    _logger.LogError("User object sent from client is null.");
                    return BadRequest("USer object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid user object sent from client.");
                    return BadRequest("Invalid model object");
                }
                var userExist = _repository.User.FindByCondition(u => u.Email.Equals(user.Email)).FirstOrDefault();
                if (userExist != null)
                {
                    _logger.LogError("This Inventory object has exist.");
                    return StatusCode(424);
                }
                var userDTO = _mapper.Map<User>(user);
                _repository.User.CreateUser(userDTO);
                _repository.Save();
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateProduct action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        [ActionName("ExportExcel")]
        public IActionResult GetExportExcel([FromQuery] UserParameters userParameters)
        {
            try
            {
                var allUsers = _repository.User.FindAll();
                userParameters.PageSize = allUsers.Count();
                var Users = _repository.User.GetUsers(userParameters);
                _logger.LogInfo($"Returned all Users from database.");
                var UsersResult = _mapper.Map<IEnumerable<UserDTO>>(Users);
                var metadata = new
                {
                    Users.TotalCount,
                    Users.PageSize,
                    Users.CurrentPage,
                    Users.TotalPages,
                    Users.HasNext,
                    Users.HasPrevious
                };
                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

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
