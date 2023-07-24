
using AutoMapper;
using BusinessObjects.DTOs;
using BusinessObjects.Models;
using eClothesAPI.Config;

using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;

namespace eClothesAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private IMapper _mapper;
        private readonly IConfiguration configuration;

        public LoginController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper, IConfiguration configuration)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            this.configuration = configuration;
        }

        [HttpPost]
        public IActionResult Register(UserCreateDTO user)
        {
            try
            {
                if (user is null)
                {
                    return BadRequest("User object is null");
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model object");
                }
                var userExist = _repository.User.FindByCondition(u => u.Email.Equals(user.Email)).FirstOrDefault();
                if (userExist != null)
                {
                    _logger.LogError("This User object has exist.");
                    return StatusCode(424);
                }
                var userDTO = _mapper.Map<User>(user);
                _repository.User.CreateUser(userDTO);
                _repository.Save();
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Register action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public IActionResult Login(LoginModel loginModel)
        {
            try
            {
                User u = _repository.User.Login(loginModel);
                if (u != null)
                {
                    string access_token = JWTConfig.CreateToken(u, configuration);
                    return Ok(access_token);
                }
                return BadRequest("User name or password is invalid");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}