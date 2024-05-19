using ECommerceMVC.Data;
using OfficeOpenXml;
namespace ECommerceMVC.Helper.Excel
{
    public class UserExcel : IExcel<DbUser>
    {
        private readonly ECommerceContext _dbContext;

        public UserExcel(ECommerceContext dbContext)
        {
            _dbContext = dbContext;
        }
        public byte[] Export(List<DbUser> items)
        {
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                // Thêm tiêu đề cột
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Name";
                worksheet.Cells[1, 3].Value = "Email";
                worksheet.Cells[1, 4].Value = "Phone";
                worksheet.Cells[1, 5].Value = "Address";
                worksheet.Cells[1, 7].Value = "Sex";
                worksheet.Cells[1, 6].Value = "Role";
                worksheet.Cells[1, 8].Value = "Status";
                worksheet.Cells[1, 9].Value = "Login Type";
                worksheet.Cells[1, 10].Value = "Created At";


                // Thêm dữ liệu
                int row = 2;
                foreach (var item in items)
                {
                    worksheet.Cells[row, 1].Value = item.Id;
                    worksheet.Cells[row, 2].Value = item.Name;
                    worksheet.Cells[row, 3].Value = item.Email;
                    worksheet.Cells[row, 4].Value = item.Phone;
                    worksheet.Cells[row, 5].Value = item.Address;
                    worksheet.Cells[row, 6].Value = item.Sex;
                    worksheet.Cells[row, 7].Value = item.IdRoleNavigation?.Name;
                    worksheet.Cells[row, 8].Value = item.Status;
                    worksheet.Cells[row, 9].Value = item.LoginType;
                    worksheet.Cells[row, 10].Value = item.CreatedAt;
                    row++;
                }

                return package.GetAsByteArray();
            }
        }

        public List<DbUser> Import(Stream fileStream)
        {
            var users = new List<DbUser>();

            using (var package = new ExcelPackage(fileStream))
            {
                var worksheet = package.Workbook.Worksheets.First();
                var rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++) // Bỏ qua dòng tiêu đề
                {
                    var user = new DbUser
                    {
                        Id = int.Parse(worksheet.Cells[row, 1]?.Value?.ToString() ?? ""),
                        Name = worksheet.Cells[row, 2]?.Value?.ToString() ?? "",
                        Email = worksheet.Cells[row, 3]?.Value?.ToString() ?? "",
                        Phone = worksheet.Cells[row, 4]?.Value?.ToString() ?? "",
                        Address = worksheet.Cells[row, 5]?.Value?.ToString() ?? "",
                        Sex = worksheet.Cells[row, 6]?.Value?.ToString() ?? "",
                        IdRole = GetRole(worksheet.Cells[row, 7]?.Value?.ToString() ?? ""),
                        Status = bool.Parse(worksheet.Cells[row, 8]?.Value?.ToString() ?? ""),
                        LoginType = worksheet.Cells[row, 9]?.Value?.ToString() ?? "",
                        CreatedAt = DateTime.Parse(worksheet.Cells[row, 10]?.Value?.ToString() ?? ""),
                    };
                    users.Add(user);
                }
            }

            return users;
        }

        public int GetRole(string name)
        {
            var role = _dbContext.DbRoles.FirstOrDefault(item => item.Name == name);
            return role != null ? role.Id : 2;
        }
    }
}
