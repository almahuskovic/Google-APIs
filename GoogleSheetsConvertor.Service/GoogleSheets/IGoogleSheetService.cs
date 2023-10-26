using Google.Apis.Sheets.v4.Data;

namespace GoogleSheetsConvertor.Service.GoogleSheets
{
    public interface IGoogleSheetService
    {
        Spreadsheet GetSpreadsheet(string id, bool includeGridData = false);
    }
}
