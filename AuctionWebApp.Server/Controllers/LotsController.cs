using AuctionWebApp.Server.Data.Dto;
using AuctionWebApp.Server.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuctionWebApp.Server.Controllers
{
    [ApiController]
    [Route("lots")]
    public class LotsController(IAuctionService auctionService) : Controller
    {
        [HttpPost("{lotId}"), Authorize]
        public async Task<IActionResult> PostAsync([FromBody] BidRequest newBid)
        {
            if (ModelState.IsValid)
            {
                await auctionService.PlaceBid(newBid.LotId, newBid.UserId, newBid.Size, DateTime.Now, newBid.MaxSize);
                return Ok(newBid);
            }

            return BadRequest(ModelState);
        }

        [HttpGet("bids/{lotId}")]
        public async Task<IActionResult> GetCostAsync(ulong lotId)
        {
            if (ModelState.IsValid)
            {
                var cost = await auctionService.GetActualCost(lotId);
                if (cost != null) return Ok(cost);
                else return BadRequest(ModelState);
            }

            return BadRequest(ModelState);
        }

        [HttpGet("track/{lotId}")]
        public async Task<bool> GetTrackableAsync(ulong lotId, ulong userId)
        {
            return await auctionService.GetTrackable(lotId, userId);
        }

        [HttpPost("track/{lotId}")]
        public async Task<IActionResult> PostTrackable(ulong lotId, ulong userId, [FromBody] bool trackable)
        {
            if (ModelState.IsValid)
            {
                await auctionService.ChangeTrackable(lotId, userId, trackable);
                return Ok(trackable);
            }

            return BadRequest(ModelState);
        }

        [HttpGet("comments/{lotId}")]
        public async Task<IEnumerable<CommentInfo>> GetLotComments(ulong lotId)
        {
            return await auctionService.GetLotComments(lotId);
        }

        [HttpPost("comments/{lotId}")]
        public async Task<IActionResult> PostCommentAsync(ulong lotId, [FromBody] CommentInfo comment)
        {
            if (ModelState.IsValid)
            {
                await auctionService.PlaceComment(comment, lotId);
                return Ok(comment);
            }

            return BadRequest(ModelState);
        }

        [HttpPost("catalog")]
        public async Task<IEnumerable<LotShortInfo>> CatalogAsync([FromBody] CatalogRequest catalogInfo)
        {
            var result = new List<LotShortInfo>();
            if (ModelState.IsValid)
            {
                var lotsList = await auctionService.GetLotsPage(catalogInfo);
                foreach (var lot in lotsList)
                {
                    result.Add(new LotShortInfo(lot.Item1, lot.Item2));
                }
            }

            return result;
        }

        [HttpGet("{lotId}")]
        public async Task<LotInfo?> GetAsync(ulong lotId)
        {
            if (ModelState.IsValid)
            {
                return await auctionService.GetLotInfo(lotId);
            }

            return null;
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(LotInfo lotInfo)
        {
            if (ModelState.IsValid)
            {
                await auctionService.PlaceLot(lotInfo);
                return Ok(lotInfo);
            }

            return BadRequest(ModelState);
        }

        [HttpPut("{lotId}")]
        public async Task<IActionResult> PutAsync(LotInfo lotInfo)
        {
            if (ModelState.IsValid)
            {
                await auctionService.ChangeLot(lotInfo);
                return Ok(lotInfo);
            }

            return BadRequest(ModelState);
        }

        [HttpDelete("{lotId}")]
        public async Task<IActionResult> DeleteAsync(LotInfo lotInfo)
        {
            if (ModelState.IsValid && lotInfo.Id != null)
            {
                await auctionService.RemoveLot(lotInfo.Id.Value, lotInfo.SellerId, lotInfo.InitialCost);
                //штраф
                return Ok(lotInfo);
            }

            return BadRequest(ModelState);
        }

        [HttpGet("image")]
        public async Task GetImageAsync(string name)
        {
            string path = Directory.GetCurrentDirectory() + "\\Data\\Images\\" + name + ".png";
            //if (System.IO.File.Exists(path))
            {
                await HttpContext.Response.SendFileAsync(path);
            }
        }
    }
}
