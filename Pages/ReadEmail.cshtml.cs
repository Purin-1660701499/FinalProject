using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace FinalProject.Pages
{
    public class ReadEmailModel : PageModel
    {
        public List<EmailInfo> listEmails2 = new List<EmailInfo>();

        private readonly ILogger<ReadEmailModel> _logger;

        public ReadEmailModel(ILogger<ReadEmailModel> logger)
        {
            _logger = logger;
        }
        public void OnGet()
        {
            String id = Request.Query["emailid"];
            try
            {
                String connectionString = "Server=tcp:wer4.database.windows.net,1433;Initial Catalog=wer4;Persist Security Info=False;User ID=wer4;Password=Weare44_;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    
                    String sql = "SELECT * FROM emails WHERE id='" + id + "'";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                EmailInfo emailInfo = new EmailInfo();
                                emailInfo.EmailID = "" + reader.GetInt32(0);
                                emailInfo.EmailSubject = reader.GetString(1);
                                emailInfo.EmailMessage = reader.GetString(2);
                                emailInfo.EmailSender = reader.GetString(5);

                                listEmails2.Add(emailInfo);
                            }
                        }
                    }
                    
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            try
            {
                String connectionString = "Server=tcp:wer4.database.windows.net,1433;Initial Catalog=wer4;Persist Security Info=False;User ID=wer4;Password=Weare44_;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE emails SET isRead=@isRead WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@isRead", 1);

                        command.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}

