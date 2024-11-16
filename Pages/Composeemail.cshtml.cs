using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using FinalProject.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;




namespace FinalProject.Pages
{
    

    public class ComposeemailModel : PageModel
    {
        public String errorMessage = "";
        public String successMessage = "";


        [BindProperty]
        public InputModel Input { get; set; }


        public class InputModel
        {

            public string Email { get; set; }

            public string Massage { get; set; }

            public string Subject { get; set; }
        }



        public void OnPost()
        {

            if (Input.Email.Length == 0 || Input.Subject.Length == 0 ||
                Input.Massage.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }

            try
            {
                DateTime now = DateTime.Now;
                string formattedDate = now.ToString("dd-MM-yyyy HH:mm:ss");
                string username = "";
                if (User.Identity.Name == null)
                {
                    username = "";
                }
                else
                {
                    username = User.Identity.Name;
                }
                String connectionString = "Server=tcp:wer4.database.windows.net,1433;Initial Catalog=wer4;Persist Security Info=False;User ID=wer4;Password=Weare44_;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO emails " +
                                 "(subject, message, date, sender, receiver) VALUES " +
                                 "(@subject, @message, @date, @sender, @receiver);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@subject", Input.Subject);
                        command.Parameters.AddWithValue("@message", Input.Massage);
                        command.Parameters.AddWithValue("@date", formattedDate);
                        command.Parameters.AddWithValue("@sender", username);
                        command.Parameters.AddWithValue("@receiver", Input.Email);

                        command.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }

            Response.Redirect("Index");
        }
    }
}

