using AuctionWebApp.Server.Data.Dto;
using AuctionWebApp.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuctionWebApp.Server.Controllers
{
    [ApiController]
    [Route("lots")]
    public class LotsController(ILotService lotService, ICommunicationService communicationService) : Controller
    {
        [HttpGet("{lotId}")]
        public async Task<LotInfo?> GetLotAsync(ulong lotId)
        {
            if (ModelState.IsValid)
            {
                return await lotService.GetInfo(lotId);
            }

            return null;
        }

        [HttpPost]
        public async Task<IActionResult> PlaceLotAsync(LotInfo lotInfo)
        {
            if (ModelState.IsValid)
            {
                await lotService.Place(lotInfo);
                return Ok(lotInfo);
            }

            return BadRequest(ModelState);
        }

        [HttpPut]
        public async Task<IActionResult> ChangeLotAsync(LotInfo lotInfo)
        {
            if (ModelState.IsValid)
            {
                await lotService.Change(lotInfo);
                return Ok(lotInfo);
            }

            return BadRequest(ModelState);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteLotAsync(LotInfo lotInfo)
        {
            if (ModelState.IsValid && lotInfo.Id != null)
            {
                await lotService.Remove(lotInfo.Id.Value, lotInfo.SellerId, lotInfo.InitialCost);
                //штраф
                return Ok(lotInfo);
            }

            return BadRequest(ModelState);
        }

        [HttpGet("comments/{lotId}")]
        public async Task<IEnumerable<CommentInfo>> GetLotCommentsAsync(ulong lotId)
        {
            return await communicationService.GetLotComments(lotId);
        }

        [HttpPost("comments/{lotId}")]
        public async Task<IActionResult> PostCommentAsync(ulong lotId, [FromBody] CommentInfo comment)
        {
            if (ModelState.IsValid)
            {
                await communicationService.PlaceComment(comment, lotId);
                return Ok(comment);
            }

            return BadRequest(ModelState);
        }
    }
}
