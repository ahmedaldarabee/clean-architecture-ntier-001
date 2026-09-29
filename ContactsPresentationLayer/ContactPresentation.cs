
using ContactsBussinessLayer;

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

        public static void updateByID(int contactID){
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

        public static void deleteContact(int contactID){
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


        public static void Main(string[] args) {
            deleteContact(12);
        }
    }
}
