using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Demetrios.Models;
using Demetrios.Services.Interfaces;
using Demetrios.Validation;

namespace Demetrios.Controllers
{
    [Route("Minutos")]
    public class MinutosController : Controller
    {
        private readonly IMinutoPostService _MinutoPostService;

        public MinutosController(IMinutoPostService MinutoPostService)
        {
            this._MinutoPostService = MinutoPostService;
        }

        [HttpGet("GetMinutoPostsAndCreate")]
        public void GetMinutoPostsAndCreate()
        {
            _MinutoPostService.GetMinutoPostsAndCreate();
        }

        [HttpPost("MinutoCreate")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody]MinutoPost MinutoPost)
        {
            if (MinutoPost.IsValid(out IEnumerable<string> errors))
            {
                var result = await _MinutoPostService.Create(MinutoPost);

                return CreatedAtAction(
                    nameof(GetAllByUserAccountId), 
                    new { id = result.link }, result);
            }
            else
            {
                return BadRequest(errors);
            }
        }

        [HttpPost("MinutoCreateLote")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public void CreateLote()
        {
            for (int a = 0; a < 50; a++)
            {
                MinutoPost MinutoPost = new MinutoPost { id = a.ToString(),
                                                            link = "link" + a.ToString(),
                                                            description = "description" + a.ToString()
                };

                _MinutoPostService.Create(MinutoPost);
            }
        }

        [HttpPut("MinutoUpdate")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody]MinutoPost MinutoPost)
        {
            if (MinutoPost.IsValid(out IEnumerable<string> errors))
            {
                var result = await _MinutoPostService.Update(MinutoPost);

                return Ok(result);
            }
            else
            {
                return BadRequest(errors);
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAll(int? pageNumber, int? pageSize)
        {
            var result = _MinutoPostService.GetAll(pageNumber, pageSize);

            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetAllByUserAccountId(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                var result = _MinutoPostService.GetAllByUserAccountId(id);

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                var result = await _MinutoPostService.Delete(id);

                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        
    }
}

