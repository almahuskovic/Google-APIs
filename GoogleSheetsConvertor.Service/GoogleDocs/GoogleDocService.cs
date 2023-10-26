using Google.Apis.Docs.v1;
using Google.Apis.Docs.v1.Data;
using Google.Apis.Sheets.v4.Data;
using GoogleSheetsConvertor.Service.Helpers;

namespace GoogleSheetsConvertor.Service.GoogleDocs
{
    public class GoogleDocService : IGoogleDocService
    {
        public DocumentsResource _googleDocumentValues;
        public GoogleDocService(GoogleHelper googleDocsHelper)
        {
            _googleDocumentValues = googleDocsHelper.DocsService.Documents;
        }
        public async Task<Document> Create(Spreadsheet spreadsheet)
        {
            try
            {
                var spreadsheetData = spreadsheet.Sheets.Select(x => x.Data);
                var sheetNames = spreadsheet.Sheets.Select(x => x.Properties.Title).Skip(1).ToList();

                Document document = new Document();
                document.Title = "Converted Document";
                var createdDocument = await _googleDocumentValues.Create(document).ExecuteAsync();

                await Update(createdDocument.DocumentId, spreadsheet);

                return createdDocument;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<Document> Update(string documentId, Spreadsheet spreadsheet)
        {
            try
            {
                var document = await _googleDocumentValues.Get(documentId).ExecuteAsync();
                var spreadsheetData = spreadsheet.Sheets.Select(x => x.Data);
                var sheetNames = spreadsheet.Sheets.Select(x => x.Properties.Title).Skip(1).ToList();

                List<Google.Apis.Docs.v1.Data.Request> requests = new List<Google.Apis.Docs.v1.Data.Request>();

                int temp = 0;
                foreach (var sheet in spreadsheetData)
                {
                    foreach (var gridData in sheet.ToList())
                    {
                        if (temp == 0)
                        {
                            requests.AddRange(CreateCoverPage(gridData));
                            requests.AddRange(CreateTableOfContents(sheetNames));
                        }
                        else
                        {
                            foreach (var row in gridData.RowData)
                            {
                                if (row.Values != null && temp==1)
                                {
                                    foreach (var cell in row.Values)
                                    {
                                        if (cell.FormattedValue != null)
                                        {
                                            //requests.Add(new Google.Apis.Docs.v1.Data.Request()
                                            //{
                                            //    InsertText = new InsertTextRequest()
                                            //    {
                                            //        Text = cell.FormattedValue,
                                            //        Location = new Location() { Index = 1 }
                                            //    }
                                            //});
                                            //requests.Add(new Google.Apis.Docs.v1.Data.Request()
                                            //{
                                            //    InsertTableRow = new InsertTableRowRequest()
                                            //    {
                                            //        TableCellLocation = new TableCellLocation() { TableStartLocation = new Location() { Index = 1 }, RowIndex = 1, ColumnIndex = 0 },
                                            //        InsertBelow = true
                                            //    }
                                            //});
                                        }
                                    }
                                }
                            }
                            //requests.Add(new Google.Apis.Docs.v1.Data.Request()
                            //{
                            //    InsertTable = new InsertTableRequest()
                            //    {
                            //        Rows = 2,
                            //        Columns = 1,
                            //        Location = new Location() { Index = 1 },
                            //    }
                            //});
                        }
                    }
                    temp++;
                }
                requests.Reverse();
                BatchUpdateDocumentRequest body = new BatchUpdateDocumentRequest() { Requests = requests };
                BatchUpdateDocumentResponse response = _googleDocumentValues.BatchUpdate(body, documentId).Execute();

                return document;
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region Helper methods
        public IList<Google.Apis.Docs.v1.Data.Request> CreateCoverPage(GridData gridData)
        {
            List<Google.Apis.Docs.v1.Data.Request> requests = new List<Google.Apis.Docs.v1.Data.Request>
                {
                    new Google.Apis.Docs.v1.Data.Request()
                    {
                        InsertInlineImage = new InsertInlineImageRequest()
                        {
                            Uri = "https://www.strunkmedia.com/wp-content/uploads/2018/05/bigstock-222496366.jpg",
                            Location = new Location { Index = 1 },
                            ObjectSize = new Size
                            {
                                Height = new Dimension
                                {
                                    Magnitude = 450.0,
                                    Unit = "PT"
                                },
                                Width = new Dimension
                                {
                                    Magnitude = 450.0,
                                    Unit = "PT"
                                }
                            }
                        }
                    }
                };

            foreach (var rowData in gridData.RowData)
            {
                int temp = 1;
                foreach (var row in rowData.Values)
                {
                    if (rowData.Values[0].FormattedValue != "Title")
                    {
                        requests.Add(new Google.Apis.Docs.v1.Data.Request()
                        {
                            InsertText = new InsertTextRequest()
                            {
                                Text = row.FormattedValue + (temp == rowData.Values.Count() ? "\n" : ": "),
                                Location = new Location() { Index = 1 }
                            }
                        });
                        requests.Add(new Google.Apis.Docs.v1.Data.Request()
                        {
                            UpdateTextStyle = new UpdateTextStyleRequest
                            {
                                Range = new Google.Apis.Docs.v1.Data.Range() { StartIndex = 1, EndIndex = row.FormattedValue.Count() },
                                TextStyle = new TextStyle() { FontSize = new Dimension() { Magnitude = 14, Unit = "PT" }, WeightedFontFamily = new WeightedFontFamily() { FontFamily = "Ariel" } },
                                Fields = (temp == rowData.Values.Count() ? "bold" : "*")
                            }
                        });
                    }
                    else if (temp != rowData.Values.Count())
                    {
                        requests.Add(new Google.Apis.Docs.v1.Data.Request()
                        {
                            InsertText = new InsertTextRequest()
                            {
                                Text = rowData.Values[1].FormattedValue + "\n",
                                Location = new Location() { Index = 1 }
                            }
                        });
                        requests.Add(new Google.Apis.Docs.v1.Data.Request()
                        {
                            UpdateTextStyle = new UpdateTextStyleRequest
                            {
                                Range = new Google.Apis.Docs.v1.Data.Range() { StartIndex = 1, EndIndex = rowData.Values[1].FormattedValue.Count() },
                                TextStyle = new TextStyle() { FontSize = new Dimension() { Magnitude = 26, Unit = "PT" }, WeightedFontFamily = new WeightedFontFamily() { FontFamily = "Ariel" } },
                                Fields = (temp == rowData.Values.Count() ? "bold" : "*")
                            }
                        });
                        requests.Add(new Google.Apis.Docs.v1.Data.Request()
                        {
                            InsertText = new InsertTextRequest()
                            {
                                Text = "Prepared by: Agent Name\n",
                                Location = new Location() { Index = 1 }
                            }
                        });
                    }
                    temp++;
                }
            }
            return requests;
        }

        public IList<Google.Apis.Docs.v1.Data.Request> CreateTableOfContents(List<string> sheetNames)
        {
            List<Google.Apis.Docs.v1.Data.Request> requests = new List<Google.Apis.Docs.v1.Data.Request>();
            var brojac = 3;
            foreach (var sheetName in sheetNames)
            {
                requests.Add(new Google.Apis.Docs.v1.Data.Request()
                {
                    InsertText = new InsertTextRequest()
                    {
                        Text = sheetName,
                        Location = new Location() { Index = 1 }
                    },
                });
                requests.Add(new Google.Apis.Docs.v1.Data.Request()
                {
                    InsertText = new InsertTextRequest()
                    {
                        Text = brojac++ + "\n",
                        Location = new Location() { Index = 1 }
                    },
                });
            }
            return requests;
        }
    }
    #endregion
}
