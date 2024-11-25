using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using SubsidyReconciliation.Data;
using SubsidyReconciliation.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SubsidyReconciliation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        //private readonly DataPullingFromDatabase _dataPullingFromDatabase;

        public HomeController(ILogger<HomeController> logger,ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            //_dataPullingFromDatabase = dataPullingFromDatabase;
        }

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult UploadSubsidy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadSubsidy(IFormFile FormFile)
        {
            HomeController userController = this;

            DateTime dateTime = DateTime.Now;
            DateTime dateonly = dateTime.Date;
            string str1 = userController._httpContextAccessor.HttpContext.Session.GetString("ActiveDirectoryUserID");
            string InitiatedBy = str1.Substring(str1.LastIndexOf('\\') + 1);
            Random rnd = new Random();
            string filenameOLD = Path.GetFileName(FormFile.FileName);
            string extension = Path.GetExtension(filenameOLD);
            string filename = Convert.ToInt32(rnd.Next(1000, 9999)).ToString() + filenameOLD;
            string str2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "SubsidyReport");
            if (!Directory.Exists(str2)) 
                Directory.CreateDirectory(str2);
            string filePath = Path.Combine(str2, filename);
            using (Stream stream = new FileStream(filePath, FileMode.Create))
                await FormFile.CopyToAsync(stream);

            string category = "";
            string reportCode = "";
            string fieldString1 = "";
            string fieldString2 = "";
            string fieldString3 = "";
            string fieldString4 = "";
            DateTime uploadDate = dateonly;

            int masterTableId = InsertSecondTableData(category, reportCode, filename, filePath, fieldString1, fieldString2, fieldString3, fieldString4, uploadDate, InitiatedBy);

            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.First();
                    DataTable dataTable = new DataTable();

                    // Add columns to the DataTable based on the Excel file headers
                    foreach (var headerCell in worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column])
                    {
                        dataTable.Columns.Add(headerCell.Text);
                    }

                    // Populate the DataTable with data from the Excel file
                    for (int rowNum = 2; rowNum <= worksheet.Dimension.End.Row; rowNum++)
                    {
                        var row = worksheet.Cells[rowNum, 1, rowNum, worksheet.Dimension.End.Column];
                        DataRow newRow = dataTable.NewRow();

                        foreach (var cell in row)
                        {
                            newRow[cell.Start.Column - 1] = cell.Text; // Adjust for zero-based index
                        }

                        dataTable.Rows.Add(newRow);
                    }

                    // Add additional columns for Upload_date, Upload_By, and MasterTableId
                    dataTable.Columns.Add("Upload_date", typeof(DateTime));
                    dataTable.Columns.Add("Upload_By", typeof(string));
                    dataTable.Columns.Add("MasterTableId", typeof(int));

                    foreach (DataRow row in dataTable.Rows)
                    {
                        row["Upload_date"] = DateTime.Now;
                        row["Upload_By"] = InitiatedBy;
                        row["MasterTableId"] = masterTableId;
                    }

                    using (SqlConnection connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
                    {
                        connection.Open();
                        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(connection))
                        {
                            sqlBulkCopy.DestinationTableName = "dbo.SubsidyUploadRecord";

                            // Map the columns
                            sqlBulkCopy.ColumnMappings.Add("Fiscal Year", "FISCAL_YEAR");
                            sqlBulkCopy.ColumnMappings.Add("Quarter", "QUARTER");
                            sqlBulkCopy.ColumnMappings.Add("Sol ID", "SOL_ID");
                            sqlBulkCopy.ColumnMappings.Add("Loan Type", "LOAN_TYPE");
                            sqlBulkCopy.ColumnMappings.Add("Client Code", "CLIENT_CODE");
                            sqlBulkCopy.ColumnMappings.Add("Loan Account", "LOAN_ACCOUNT");
                            sqlBulkCopy.ColumnMappings.Add("Name of Borrower", "NAME_OF_BORROWER");
                            sqlBulkCopy.ColumnMappings.Add("Subdiy Interest Amount", "SUBDIY_INTEREST_AMOUNT");
                            sqlBulkCopy.ColumnMappings.Add("Subsidy Category", "SUBSIDY_CATEGORY");
                            sqlBulkCopy.ColumnMappings.Add("Remarks", "REMARKS");
                            sqlBulkCopy.ColumnMappings.Add("Upload_date", "UPLOAD_DATE");
                            sqlBulkCopy.ColumnMappings.Add("Upload_By", "UPLOAD_BY");
                            sqlBulkCopy.ColumnMappings.Add("MasterTableId", "MasterTableId");

                            // Write the data to the database
                            await sqlBulkCopy.WriteToServerAsync(dataTable);
                        }
                    }
                    return Json(new { success = true, message = "Data Uploaded Successfully!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public int InsertSecondTableData(string category, string reportCode, string filename, string revisedFilename, string fieldString1, string fieldString2, string fieldString3, string fieldString4, DateTime uploadDate, string uploadBy)
        {
            try
            {
                // Insert data into the second table using the provided parameters
                // Example code to insert data into the second table
                using (SqlConnection newconnection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
                //using (SqlConnection newconnection = new SqlConnection("Data Source=192.168.9.85; Initial Catalog=POSSettlementDB; User Id=sanima; Password=P@ssw0rd"))
                {

                    using (SqlCommand command = new SqlCommand("INSERT INTO MasterTable (Category, Report_Code, File_Name, Revised_File_Name, FieldString1,FieldString2,FieldString3,FieldString4,Upload_date,Upload_By) VALUES (@Category, @Report_Code,@File_Name,@Revised_File_Name,@FieldString1,@FieldString2,@FieldString3 ,@FieldString4,@Upload_date,@Upload_By) ;SELECT SCOPE_IDENTITY();", newconnection))
                    {
                        command.Parameters.AddWithValue("@Category", category);
                        command.Parameters.AddWithValue("@Report_Code", reportCode);
                        command.Parameters.AddWithValue("@File_Name", filename);
                        command.Parameters.AddWithValue("@Revised_File_Name", revisedFilename);
                        command.Parameters.AddWithValue("@FieldString1", fieldString1);
                        command.Parameters.AddWithValue("@FieldString2", fieldString2);
                        command.Parameters.AddWithValue("@FieldString3", fieldString3);
                        command.Parameters.AddWithValue("@FieldString4", fieldString4);
                        command.Parameters.AddWithValue("@Upload_Date", uploadDate);
                        command.Parameters.AddWithValue("@Upload_By", uploadBy);

                        newconnection.Open();
                        int insertedId = Convert.ToInt32(command.ExecuteScalar());
                        return insertedId;
                    }
                }
            }
            catch (Exception ex)
            {

                throw; // Optionally, rethrow the exception to propagate it
            }
        }

        public IActionResult SubsidyReport()
        {
            int currentPage = 1; // Set the current page
            int rowsPerPage = 10; // Set the number of rows per page

            ViewBag.CurrentPage = currentPage; // Pass currentPage to the view
            ViewBag.RowsPerPage = rowsPerPage;

            var records = _context.SubsidyRecord.FromSqlRaw("exec sp_getSubsidyData").ToList();
            return View(records);
        }

        public IActionResult SubsidySumReport()
        {
            int currentPage = 1; // Set the current page
            int rowsPerPage = 10; // Set the number of rows per page

            ViewBag.CurrentPage = currentPage; // Pass currentPage to the view
            ViewBag.RowsPerPage = rowsPerPage;

            var records = _context.SubsidyRecord.FromSqlRaw("exec sp_GetLoanAcctandFYWiseSumReport").ToList();
            return View(records);
        }

    }
}
