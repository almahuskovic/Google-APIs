using Google.Apis.Docs.v1.Data;
using Google.Apis.Drive.v3;
using Google.Apis.Upload;
using GoogleSheetsConvertor.Service.Helpers;
using System.Web;
using Microsoft.Owin.Host.SystemWeb;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Google.Apis.Drive.v3.Data;

namespace GoogleSheetsConvertor.Service.GoogleDrive
{
    public class GoogleDriveService : IGoogleDriveService
    {
        public DriveService _googleDriveValues;
        
        public GoogleDriveService(GoogleHelper googleDriveHelper)
        {
            _googleDriveValues = googleDriveHelper.DriveService;
        }
       
        public Google.Apis.Drive.v3.Data.File CreateDocumentOnDrive(string parentId)
        {
            try
            {
                var fileMetaData = new Google.Apis.Drive.v3.Data.File();

                fileMetaData.Name ="Converted Document";
                fileMetaData.MimeType = "application/vnd.google-apps.document";
                fileMetaData.Parents = new string[] { parentId };

                var request = _googleDriveValues.Files.Create(fileMetaData);
                request.Fields = "id";
                var x = request.Execute();
                return x;
            }
            catch (Exception)
            {
                
                throw;
            }
        }
    }
}
