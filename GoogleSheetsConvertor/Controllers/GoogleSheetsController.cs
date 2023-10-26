using GoogleSheetsConvertor.Model;
using GoogleSheetsConvertor.Service.GoogleDocs;
using GoogleSheetsConvertor.Service.GoogleDrive;
using GoogleSheetsConvertor.Service.GoogleSheets;
using Microsoft.AspNetCore.Mvc;

namespace GoogleSheetsConvertor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GoogleSheetsController : ControllerBase
    {
        protected IGoogleSheetService _googleSheetService;
        protected IGoogleDocService _googleDocService;
        protected IGoogleDriveService _googleDriveService;
        public GoogleSheetsController(IGoogleSheetService googleSheetService, IGoogleDocService googleDocService, IGoogleDriveService googleDriveService)
        {
            _googleSheetService = googleSheetService;
            _googleDocService = googleDocService;
            _googleDriveService = googleDriveService;
        }

        [HttpPost("Convert")]
        public async Task<IActionResult> Convert([FromBody] ConvertData data)
        {
            var spreadsheet = _googleSheetService.GetSpreadsheet(data.FileId, true);

            var file = _googleDriveService.CreateDocumentOnDrive(data.ParentId);

            await _googleDocService.Update(file.Id, spreadsheet);

            return Ok();
        }
    }
}
