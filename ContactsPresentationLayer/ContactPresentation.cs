
using ContactsBussinessLayer;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ContactsPresentationLayer {
    public class ContactPresentation {
        public static void FindContactByID(int id){
            Contact contact = Contact.FindByID(id);

            if (contact != null){
                Console.WriteLine($"user full name: {contact.FirstName} {contact.LastName}");
                Console.WriteLine($"user email: {contact.Email}");
                Console.WriteLine($"user phone: {contact.Phone}");
                Console.WriteLine($"user address: {contact.Address}");
                Console.WriteLine($"user country: {contact.CountryID}");
            }
            else{
                Console.WriteLine($"This user id info: {id} not found!");
            }
        }
        
        public static void AddNewContact(){
            Contact contact = new Contact();
            contact.FirstName = "Mohammmed";
            contact.LastName = "Al Darabee";
            contact.Email = "mohammed.info@gmail.com";
            contact.Phone = "+962-785266266";
            contact.Address = "Jordan-Amman";
            contact.CountryID = 1;
            Console.WriteLine(contact.Saving() ?
                    $"Inserted row correctlly , and new contact id: {contact.ContactID}" : 
                    "Opps, somthing went error!");
        }

        public static void UpdateByID(int contactID){
            Contact contact = new Contact();

            if (contact.IsExist(contactID)) {

                contact.ContactID = contactID;
                contact.Phone = "+962-788856669";
                Console.WriteLine(contact.Updating() ?
                        $"Needed row updated successdully for id: {contact.ContactID}" :
                        "Opps, somthing went error!");
            }
            else {
                Console.WriteLine("Opps, this user not exist!");
            }
        }

        public static void DeleteContact(int contactID){
            Contact contact = new Contact();

            Console.WriteLine("sender id: {0}", contactID);
            if (contact.IsExist(contactID)){
                if (contact.Deleting(contactID)){
                    Console.WriteLine("user deleted successfully");
                }
                else {
                    Console.WriteLine("Sorry, somethink went error !");
                }
            }
            else{
                Console.WriteLine("Opps, this user not exist!");
            }
        }

        public static void EmployeeDataTable(){
            DataTable employeeInfo = new DataTable("employeeInfo");
            try {
                //Table Defining
                employeeInfo.Columns.Add("ID", typeof(int));
                employeeInfo.Columns.Add("Name", typeof(string));
                employeeInfo.Columns.Add("HiringDate", typeof(DateTime));
                employeeInfo.Columns.Add("Salary", typeof(double));
                employeeInfo.Columns.Add("Country", typeof(string));

                //Set PR Key
                DataColumn[] primaryColumns = new DataColumn[1];
                primaryColumns[0] = employeeInfo.Columns["ID"]!;
                employeeInfo.PrimaryKey = primaryColumns;

                //Insertion of Data
                employeeInfo.Rows.Add(1, "ahmed al darabee", DateTime.Now, 1200.33, "Jordan");
                employeeInfo.Rows.Add(2, "ali al darabee", DateTime.Now, 1200.33, "Jordan");
                employeeInfo.Rows.Add(3, "osama al darabee", DateTime.Now, 1200.33, "Jordan");
                employeeInfo.Rows.Add(4, "oday al darabee", DateTime.Now, 7723.444, "Turkey");
                employeeInfo.Rows.Add(5, "mohamad darabee", DateTime.Now, 15200.88, "Egypt");
                employeeInfo.Rows.Add(6, "jawad al darabee", DateTime.Now, 2223.444, "KSA");
                employeeInfo.Rows.Add(7, "own al darabee", DateTime.Now, 15200.88, "Egypt");
                employeeInfo.Rows.Add(8, "yehya al darabee", DateTime.Now, 7723.444, "Turkey");
                employeeInfo.Rows.Add(9, "gogo al darabee", DateTime.Now, 7723.444, "Turkey");
                employeeInfo.Rows.Add(10, "kinda al darabee", DateTime.Now, 7723.444, "Turkey");

                //Data Sorting
                DoSorting(employeeInfo,"DESC");

                //Select [ GET ]
                EmployeeDataView(employeeInfo,"Getting all employee data");

                //Aggregate Functions
                HandleAggregateFunctions(employeeInfo);

                //filtering section
                FilterEmployeeByCountry(employeeInfo);

                //Deleting Rows, When you can to delete all records: employeeInfo.Clear();
                DataRow[] infoRows = employeeInfo.Select("ID = 10");
                foreach (DataRow info in infoRows){
                    info.Delete();
                }

                employeeInfo.AcceptChanges();
                EmployeeDataView(employeeInfo, "Data after deletion");

                //Updating Rows
                DataRow[] tableRows = employeeInfo.Select("ID = 9");
                foreach (DataRow info in tableRows){
                    info["Name"] = "Obai al darabee";
                    info["Salary"] = 8000.999;
                }

                employeeInfo.AcceptChanges();

                DoSorting(employeeInfo, "ASC");
                EmployeeDataView(employeeInfo, "Data after updation");

                ShowDataSet(employeeInfo);
            }
            catch (Exception ex){
                Console.WriteLine("Error be as: {0}", ex.Message);
            }
        }
        
        //Best way to show/view data
        public static void EmployeeDataView(DataTable employees,string title){
            Console.WriteLine($"\n\n{title}");
            DataView employeeViews = employees.DefaultView; //DefaultView that getting data of current data tabel version
            for (int i = 0; i < employeeViews.Count; i++){
                Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}", 
                    employeeViews[i][0], employeeViews[i][1],
                    employeeViews[i][2], employeeViews[i][3],
                    employeeViews[i][4]
                );
            }
        }

        public static void FilterEmployeeByCountry(DataTable employees){
            DataView employeeViews = employees.DefaultView;
            employeeViews.RowFilter = "(Country='Jordan' or Country='KSA')";
            for (int i = 0; i < employeeViews.Count; i++)
            {
                Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}",
                    employeeViews[i][0], employeeViews[i][1],
                    employeeViews[i][2], employeeViews[i][3],
                    employeeViews[i][4]
                );
            }
        }

        public static void HandleAggregateFunctions(DataTable employeeInfo){
            int totalEmployees = employeeInfo.Rows.Count;
            double maxSalary = Convert.ToDouble(employeeInfo.Compute("max(Salary)", string.Empty));
            double minSalary = Convert.ToDouble(employeeInfo.Compute("min(Salary)", string.Empty));
            double avgSalary = Convert.ToDouble(employeeInfo.Compute("avg(Salary)", string.Empty));
            double totalSalary = Convert.ToDouble(employeeInfo.Compute("sum(Salary)", string.Empty));

            Console.WriteLine("\nSo Total Employee: {0}\n\nmaximum salary: {1}\nminimum salary {2}\navg of salaries: {3}\ntotal salary {4}\n",
                totalEmployees, maxSalary, minSalary, avgSalary, totalSalary);
        }

        public static void DoSorting(DataTable employeeInfo,string operation){
            employeeInfo.DefaultView.Sort = $"ID {operation}";
            employeeInfo = employeeInfo.DefaultView.ToTable();
        }

        public static void DepartmentDataTable(){
            DataTable departmentTable = new DataTable("departmentTable");
            departmentTable.Columns.Add("DepartmentID",typeof(int));
            departmentTable.Columns.Add("DepartmentName", typeof(string));


            departmentTable.Rows.Add(1,"Engineering");
            departmentTable.Rows.Add(2, "Marketing");
            departmentTable.Rows.Add(3, "Humman Resources");

            DepartmentDataView(departmentTable);
            //ShowDataSet(departmentTable);

        }

        public static void DepartmentDataView(DataTable info){
            DataView rows = info.DefaultView;
            for (int i = 0; i < rows.Count; i++ ){
                Console.WriteLine("Id: {0}\tDepartment Name: {1}",rows[i][0], rows[i][1]);
            }
        }

        public static void ShowDataSet(DataTable? employees){
            //DataTable? department
            Console.WriteLine("\n\nData Set Tables Info\n");
            DataSet sets = new DataSet();
            sets.Tables.Add(employees!);
            //sets.Tables.Add(department!);

            DataView employeeViews = sets.Tables["employeeInfo"]?.DefaultView!;

            for (int i =0 ; i < employees?.Rows.Count; i++){
                Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}",
                        employeeViews[i][0], employeeViews[i][1],
                        employeeViews[i][2], employeeViews[i][3],employeeViews[i][4]
                );
            }

            //DataView departmentsViews = department?.DefaultView!;
            //for (int i = 0; i < departmentsViews.Count; i++) {
            //    Console.WriteLine("Id: {0}\tDepartment Name: {1}", departmentsViews[i][0], departmentsViews[i][1]);
            //}
        }

        public static void DataAdapterHandler(){
            string connectionSQLString = "Server=.;Database=ContactsDB;Integrated Security=True;TrustServerCertificate=True;"??"";
            SqlConnection connection = new SqlConnection(connectionSQLString);

            string query = "select * from Contacts"??"";

            try{
                connection.Open();

                DataSet dataSets = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter(query,connection);
                adapter.SelectCommand.Connection = connection;

                adapter.Fill(dataSets, "Contacts");
                connection.Close();

                foreach(DataRow info in dataSets.Tables["Contacts"]!.Rows){
                    Console.WriteLine("ID: {0}\tFirstName: {1}\tLastName: {2}\tEmail: {3}", 
                        info["ContactID"], info["FirstName"], info["LastName"], info["Email"]);
                }

            }
            catch (Exception ex){
                Console.WriteLine(ex.Message);
            }
        }

        public static void Main(string[] args) {
            DataAdapterHandler();
        }
    }
}
