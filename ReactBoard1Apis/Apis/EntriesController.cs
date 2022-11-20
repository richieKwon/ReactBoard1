using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ReactBoard1.Models;
using Dul;

namespace ReactBoard1Apis.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("/api/[controller]")]
    public class EntriesController : ControllerBase
    {
        private readonly IEntryRepository _entryRepository;
        private readonly ILogger _logger;

        public EntriesController(IEntryRepository entryRepository, ILoggerFactory loggerFactory)
        {
            _entryRepository = entryRepository ?? throw new ArgumentException(nameof(EntriesController));
            _logger = loggerFactory.CreateLogger(nameof(EntriesController));
        }

        #region Post

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] Entry dto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var temp = new Entry();
            temp.Name = dto.Name;
            temp.Title = dto.Title;
            temp.Content = dto.Content;
            temp.Created = DateTime.Now;

            try
            {
                var model = await _entryRepository.AddAsync(temp);
                if (model==null)
                {
                    return BadRequest();
                }
                // status 201 Created
                // return CreatedAtRoute("GetEntryById", new { id = model.Id }, model);
                return Ok(model);
                // return CreatedAtAction(nameof(GetEntryById), new {id =model.Id }, model);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
            }
        }
        #endregion

        #region Get
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var models = await _entryRepository.GetAllAsync();
                if (!models.Any())
                {
                    return new NoContentResult();
                }
                return Ok(models);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
            }
        }
        #endregion

        #region GetById

        [HttpGet("{id:int}", Name = "GetEntryById")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var model = await _entryRepository.GetByIdAsync(id);
                if (model==null)
                {
                    return NotFound();
                }
                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
            }
        }
        #endregion

        #region Put

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] Entry dto)
        {
            if (dto==null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var origin = await _entryRepository.GetByIdAsync(id);
            if (origin !=null)
            {
                origin.Name = dto.Name;
                origin.Title = dto.Title;
                origin.Content = dto.Content;
            }
            // origin.Created = dto.Created;
            
            try
            {
                origin.Id = id;
                var status = await _entryRepository.UpdateAsync(origin);
                if (!status)
                {
                    return BadRequest();
                }
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
            }
        }
        #endregion

        #region Delete

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            try
            {
                var status = await _entryRepository.DeleteAsync(id);
                if (!status)
                {
                    BadRequest();
                }
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest("Impossible to delete");
            }
        }
        #endregion

        #region Paging

        [HttpGet("Page/{pageNumber:int}/{pageSize:int}")]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize =10)
        {
            try
            {
                int pageIndex = (pageNumber > 0) ? pageNumber - 1 : 0;
                var resultSet = await _entryRepository.GetAllPageAsync(pageIndex, pageSize);
                if (resultSet.Records == null)
                {
                    return NotFound("No data");
                }
                Response.Headers.Add("X-TotalRecordCount", resultSet.TotalRecords.ToString());
                Response.Headers.Add("Access-Control-Expose-Headers", "X-TotalRecordCount");
                var responseValue = resultSet.Records;
                return Ok(responseValue);
            }
            catch (Exception e)
            {
                _logger?.LogError($"Error({nameof(GetAll)}) :{e.Message}");
                return BadRequest();
            }
        }
        #endregion
    }
}