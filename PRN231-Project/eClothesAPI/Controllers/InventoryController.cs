using AutoMapper;
using BusinessObjects.DTOs;
using BusinessObjects.Models;
using BusinessObjects.QueryParameters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.Interfaces;
using Repositories.Repository;

namespace eClothesAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private IMapper _mapper;

        public InventoryController(ILoggerManager logger, IRepositoryWrapper repository, IMapper mapper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetInventories([FromQuery] InventoryParameters InventoryParameters)
        {
            try
            {
                var Inventories = _repository.Inventory.GetInventories(InventoryParameters);
                _logger.LogInfo($"Returned all Inventories from database.");
                var InventorysResult = _mapper.Map<IEnumerable<InventoryDTO>>(Inventories);
                return Ok(InventorysResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetInventorys action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]        
        
        public IActionResult GetColorById(int id)
        {
            var color = _repository.Inventory.GetColorByProductId(id);
            return Ok(color);
        }

        [HttpGet("{productId}/sizes")]
        public IActionResult GetSizesByColorId(int productId, int colorId)
        {
            var sizes = _repository.Inventory.GetSizeByColorId(productId, colorId);
            return Ok(sizes);
        }


        [HttpPost]
        public IActionResult CreateInventory([FromBody] InventoryCreateUpdateDTO Inventory)
        {
            try
            {
                var inventoryParameters = new InventoryParameters(); 
                if (Inventory is null)
                {
                    _logger.LogError("Inventory object sent from client is null.");
                    return BadRequest("Inventory object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Inventory object sent from client.");
                    return BadRequest("Invalid model object");
                }

                bool isExist = _mapper.Map<IEnumerable<InventoryCreateUpdateDTO>>(_repository.Inventory.GetInventories(inventoryParameters)).Any(a => a.Equals(Inventory));
                if (isExist)
                {
                    _logger.LogError("This Inventory object has exist.");
                    return BadRequest("This Inventory object has exist");
                }
                var inventoryEntity = _mapper.Map<Inventory>(Inventory);
                _repository.Inventory.CreateInventory(inventoryEntity);
                _repository.Save();
                return Ok(Inventory);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateInventory action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateInventory(int id, [FromBody] InventoryCreateUpdateDTO InventoryDto)
        {
            try
            {
                if (InventoryDto is null)
                {
                    _logger.LogError("Inventory object sent from client is null.");
                    return BadRequest("Inventory object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Inventory object sent from client.");
                    return BadRequest("Invalid model object");
                }
                var InventoryEntity = _repository.Inventory.GetInventoryDetails(id);
                if (InventoryEntity is null)
                {
                    _logger.LogError($"Inventory with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                _mapper.Map(InventoryDto, InventoryEntity);
                _repository.Inventory.UpdateInventory(InventoryEntity);
                _repository.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside UpdateInventory action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteInventory(int id)
        {
            try
            {
                var Inventory = _repository.Inventory.GetInventoryDetails(id);
                if (Inventory == null)
                {
                    _logger.LogError($"Inventory with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                _repository.Inventory.DeleteInventory(Inventory);
                _repository.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteInventory action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet]
        [ActionName("ExportExcel")]
        public IActionResult GetExportExcel()
        {
            try
            {
                var Inventories = _repository.Inventory.ExportExel();
                _logger.LogInfo($"Returned all Inventorys from database.");
                var InventorysResult = _mapper.Map<IEnumerable<InventoryDTO>>(Inventories);
                return Ok(InventorysResult);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllOwners action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
