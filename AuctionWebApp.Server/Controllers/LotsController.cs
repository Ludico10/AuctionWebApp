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
                var lot = await lotService.Place(lotInfo);
                if (lot != null)
                {
                    lotInfo.Id = lot.LId;
                    return Ok(lotInfo);
                }
            }

            return BadRequest(ModelState);
        }

        [HttpPut]
        public async Task<IActionResult> ChangeLotAsync(LotInfo lotInfo)
        {
            if (ModelState.IsValid)
            {
                var result = await lotService.Change(lotInfo);
                if (result) return Ok(lotInfo);
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{lotId}")]
        public async Task<IActionResult> DeleteLotAsync(ulong lotId)
        {
            if (ModelState.IsValid)
            {
                var result = await lotService.Remove(lotId);
                //штраф
                return Ok(result);
            }

            return BadRequest(ModelState);
        }

        [HttpGet("freeDays")]
        public async Task<IEnumerable<int>> GetFreeDaysAsync(ushort catId, int year, int month)
        {
            return await lotService.FreePremiumDates(catId, month, year);
        }

        [HttpGet("comments/{lotId}")]
        public async Task<IEnumerable<CommentInfo>> GetLotCommentsAsync(ulong lotId)
        {
            return await communicationService.GetLotComments(lotId);
        }

        [HttpPost("comments")]
        public async Task<IActionResult> PostCommentAsync(CommentInfo comment)
        {
            if (ModelState.IsValid)
            {
                await communicationService.PlaceComment(comment, comment.LotId);
                return Ok(comment);
            }

            return BadRequest(ModelState);
        }
    }
}
