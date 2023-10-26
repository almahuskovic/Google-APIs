using Google.Apis.Docs.v1.Data;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GoogleSheetsConvertor.Service.GoogleDocs
{
    public interface IGoogleDocService
    {
        Task<Document> Create(Spreadsheet spreadsheet);
        Task<Document> Update(string documentId,Spreadsheet spreadsheet);
    }
}
