using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using GoogleSheetsConvertor.Service.Helpers;

namespace GoogleSheetsConvertor.Service.GoogleSheets
{
    public class GoogleSheetService : IGoogleSheetService
    {
        public SpreadsheetsResource _googleSheetValues;
        public GoogleSheetService(GoogleHelper googleSheetsHelper)
        {
            _googleSheetValues = googleSheetsHelper.SheetsService.Spreadsheets;
        }
        public Spreadsheet GetSpreadsheet(string id, bool includeGridData = false)
        {
            var result = _googleSheetValues.GetByDataFilter(new Google.Apis.Sheets.v4.Data.GetSpreadsheetByDataFilterRequest() { IncludeGridData = includeGridData }, id).Execute();
            return result;
        }
    }
}
