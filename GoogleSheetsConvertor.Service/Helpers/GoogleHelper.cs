using Google.Apis.Auth.OAuth2;
using Google.Apis.Docs.v1;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSheetsConvertor.Service.Helpers
{
    public class GoogleHelper
    {
        public SheetsService SheetsService { get; set; }
        public DocsService DocsService { get; set; }
        public DriveService DriveService { get; set; }

        const string APPLICATION_NAME = "SheetsConverter";
        string[] Scopes = { SheetsService.Scope.Spreadsheets, DocsService.ScopeConstants.Documents, DriveService.Scope.Drive, DriveService.Scope.DriveFile };

        [Obsolete]
        public GoogleHelper()
        {
            InitializeService();
        }

        [Obsolete]
        private void InitializeService()
        {
            var credential = GetCredentialsFromFile();
            SheetsService = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = APPLICATION_NAME
            });
            DocsService = new DocsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = APPLICATION_NAME
            });
            DriveService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = APPLICATION_NAME
            });
        }

        [Obsolete]
        private UserCredential GetCredentialsFromFile()
        {
            UserCredential credential;
            using (var stream = new FileStream("wwwroot/client_secret.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.Load(stream).Secrets,
                Scopes,
                "user",
                CancellationToken.None).Result;
            }
            return credential;
        }
    }
}
