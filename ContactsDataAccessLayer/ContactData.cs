using Microsoft.Data.SqlClient;
using System.Data;

namespace ContactsDataAccessLayer {
    public class ContactData {

        public static bool getContactInfoByID(int ContactID, 
            ref string FirstName,
            ref string LastName,
            ref string Email,
            ref string Phone,
            ref string Address,
            ref int CountryID
            ) {
            
            bool isContactFound = false;
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionSQLString);
            string query = "select * from Contacts where ContactID = @ContactID";

            try {

                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.Add("@ContactID", SqlDbType.Int).Value = ContactID;

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read()) {
                    isContactFound = true;

                    FirstName = (string)reader["FirstName"];
                    LastName = (string)reader["LastName"];
                    Email = (string)reader["Email"];
                    Phone = (string)reader["Phone"];
                    Address = (string)reader["Address"];
                    CountryID = (int)reader["CountryID"];
                }
                else {
                    isContactFound = false;
                }

                reader.Close();

                return isContactFound;
            }
            catch(Exception) {
                isContactFound = false;
            }
            finally{
                connection.Close();
            }

            return isContactFound;
        }
        
        public static int SavingContact(
            string FirstName,
            string LastName,
            string Email,
            string Phone,
            string Address,
            int CountryID)
        {
            int ContactID = -1;
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionSQLString);
            try {
                connection.Open();

                string query =
                    @"insert into Contacts values(@FirstName,@LastName,@Email,@Phone,@Address,@CountryID);
                    select SCOPE_IDENTITY();";

                SqlCommand command = new SqlCommand(query,connection);
                command.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = FirstName;
                command.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = LastName;
                command.Parameters.Add("@Email", SqlDbType.NVarChar).Value = Email;
                command.Parameters.Add("@Phone", SqlDbType.NVarChar).Value = Phone;
                command.Parameters.Add("@Address", SqlDbType.NVarChar).Value = Address;
                command.Parameters.Add("@CountryID", SqlDbType.Int).Value = CountryID;

                var effectedRows = command.ExecuteScalar();


                if (effectedRows != null && int.TryParse(effectedRows.ToString(),out int resultOFInsertion)) {
                    ContactID = resultOFInsertion;
                }
                connection.Close();
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
            
            return ContactID;
        }
    }
}
