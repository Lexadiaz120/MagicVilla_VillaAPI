using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers

{
    [Route("api/VillaApi")]
   //  [ApiController]

    public class VillaApiController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok(VillaStore.villaList);
        }
        [HttpGet("id", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult  GetVilla(int id )
        {         
            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id ==  id);
            if(villa == null)
            {
                return NotFound(); 
            }

            return Ok(villa);
        }

        [HttpPost ]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

       public ActionResult<VillaDTO> createVilla(VillaDTO villaDTO)
        {  
            if(VillaStore.villaList.FirstOrDefault(u => u.Name.ToLower() == villaDTO.Name.ToLower()) !=  null)
            {
                ModelState.AddModelError("CustomError", "Villa already exists");
                return BadRequest(ModelState);
            }
            
            if(villaDTO == null)
            {
                return BadRequest(); 
            } 
            if(villaDTO.Id >  0 )       
            {
                return StatusCode(StatusCodes.Status500InternalServerError); 
            }
            villaDTO.Id = VillaStore.villaList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
            VillaStore.villaList.Add(villaDTO);
            return CreatedAtRoute("GetVilla", new { id = villaDTO.Id }, villaDTO);
        }
        [HttpDelete("{id:int}", Name =  "Delete Villa")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteVilla(int id)
        {    
            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            VillaStore.villaList.Remove(villa);
            return NoContent(); 
        }

    }
}
