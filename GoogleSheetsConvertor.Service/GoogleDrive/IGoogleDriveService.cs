using Google.Apis.Docs.v1.Data;

namespace GoogleSheetsConvertor.Service.GoogleDrive
{
    public interface IGoogleDriveService
    {
        Google.Apis.Drive.v3.Data.File CreateDocumentOnDrive(string parentId);
    }
}
