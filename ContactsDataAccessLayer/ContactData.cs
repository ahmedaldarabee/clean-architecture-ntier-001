using Logging;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ContactsDataAccessLayer {
    public class ContactData {
        private static readonly NLogLogger _logger = new NLogLogger();

        public static bool getContactInfoByID(int ContactID,ref string FirstName,ref string LastName,ref string Email,ref string Phone,ref string Address,ref int CountryID) {
            
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
        
        public static int SavingContact(string FirstName,string LastName,string Email,string Phone,string Address,int CountryID){
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

                _logger.Info("new row addedd successfully");
                connection.Close();
            }
            catch (Exception ex) {
                _logger.Error(ex, "Error about adding new row:");
            }
            
            return ContactID;
        }

        public static bool UpdateContact(int ContactID, string Phone){
            bool updatingResult = false;
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionSQLString);
            try {
                connection.Open();
                string query = @"update Contacts set Phone = @Phone where ContactID = @ContactID";
                SqlCommand command = new SqlCommand(query,connection);

                command.Parameters.Add("@ContactID", SqlDbType.Int).Value = ContactID;
                command.Parameters.Add("@Phone", SqlDbType.NVarChar).Value = Phone;

                var resultOfEffectedRow = command.ExecuteNonQuery();

                if (resultOfEffectedRow != 0){
                    updatingResult = true;
                }

                _logger.Info($"updated row of this user {ContactID} successfully");

            }
            catch (Exception ex){
                _logger.Error(ex, $"Error about updating this user {ContactID}");
            }
            connection.Close();
            return updatingResult;
        }
    
        public static bool FindByID(int ContactID){
            bool result = false;
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionSQLString);
            try {
                connection.Open();

                string query = @"select * from Contacts where ContactID = @ContactID";
                SqlCommand command = new SqlCommand(query,connection);
                command.Parameters.Add("@ContactID", SqlDbType.Int).Value = ContactID;

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read()){
                    result = true;
                }
                _logger.Info("get contact info successfullt");
                reader.Close();
            }
            catch (Exception ex){
                _logger.Error(ex,"Error about getting contact by id! where almost this user not found!");
            }

            connection.Close();

            return result;
        }
        
        public static bool DeleteById(int ContactID){
            bool result = false;
            SqlConnection connection = new SqlConnection(DataAccessSettings.ConnectionSQLString);
            try {
                connection.Open();

                string query = @"delete from Contacts where ContactID = @ContactID";

                SqlCommand command = new SqlCommand(query,connection);
                command.Parameters.Add("@ContactID", SqlDbType.Int).Value = ContactID;

                var effectedRow = command.ExecuteNonQuery();

                if(effectedRow != 0) {
                    result = true;
                    _logger.Info($"Deleted this user {ContactID} successfully");
                }
                connection.Close();
            }
            catch (Exception ex){
                _logger.Error(ex,"Error about deletion new ");
            }
            connection.Close();
            return result;
        }
    }
}
