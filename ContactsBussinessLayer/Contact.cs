
using ContactsDataAccessLayer;
using Logging;

namespace ContactsBussinessLayer {
    public class Contact {

        public int ContactID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public int CountryID { get; set; }
        

        public Contact(){
            this.ContactID = -1;
            this.Address = "";
            this.Phone = "";
            this.Email = "";
            this.FirstName = "";
            this.LastName = "";
            this.CountryID = -1;
        }
        private Contact(int ContactID,string FirstName,string LastName,string Email,string Phone,string Address,int CountryID) {
            this.Address = Address;
            this.ContactID = ContactID;
            this.Phone = Phone;
            this.Email = Email;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.CountryID = CountryID;
        }

        public static Contact FindByID(int ContactID){
            string FirstName ="";
            string LastName = "";
            string Email = "";
            string Phone = "";
            string Address = "";
            int CountryID=1;

            bool isFound = ContactData.getContactInfoByID(ContactID, ref FirstName, ref LastName, ref Email , ref Phone, ref Address, ref CountryID);
            if (isFound) {
                return new Contact(ContactID,FirstName,LastName,Email,Phone,Address,CountryID);
            }
            else{
                return null!;
            }
        }
        
        public bool Saving(){
            this.ContactID = ContactData.SavingContact(this.FirstName!, this.LastName!, this.Email!, this.Phone!, this.Address!, this.CountryID);
            return this.ContactID != -1;
        }

        public bool Updating() {
            return ContactData.UpdateContact(this.ContactID,this.Phone!) != false;
        }

        public bool IsExist(int ContactID){
            return ContactData.FindByID(ContactID) != false;
        }

        public bool Deleting(int ContactID){
            return ContactData.DeleteById(ContactID) != false;
        }

    }
}
