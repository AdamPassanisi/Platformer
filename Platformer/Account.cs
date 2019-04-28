using System;
using System.Security.Cryptography;
using System.Text;

namespace Platformer
{
    public class Account



    {


        private string userID { set; get; }
        private string password{ set; get; }



        // when a new account is created 
        // we also need to add username onto the database
        public Account(string userID,string pass)
        {

            this.userID = userID;
           
            //Console.WriteLine(Convert.ToBase64String(GenerateSaltedHash(Encoding.UTF8.GetBytes(password), Encoding.UTF8.GetBytes("cs496"))));
            this.password = Convert.ToBase64String((GenerateSaltedHash(Encoding.UTF8.GetBytes(pass), Encoding.UTF8.GetBytes("cs496"))));
            Console.WriteLine(password);
        }


        // incase user has already signed up
        public Account(string userID)
        {

            this.userID = userID;
            
        }

        // Takes in a string and a 'salt'
        // Concatenates them to one string and returns its SHA256 hash
        public static String GenerateHash(String p, String s)
        {
            HashAlgorithm algorithm = new SHA256Managed();

            byte[] b = algorithm.ComputeHash(Encoding.UTF8.GetBytes(p+s));

            StringBuilder hashed = new System.Text.StringBuilder();


            foreach (byte Byte in b)
            {
                hashed.Append(Byte.ToString("x2"));
            }

            return hashed.ToString();

        }


        // genereate hash
        // salts to be randomised
        public static byte[] GenerateSaltedHash(byte[] plainText, byte[] salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();
          

            byte[] plainTextWithSaltBytes =
              new byte[plainText.Length + salt.Length];

            for (int i = 0; i < plainText.Length; i++)
            {
                plainTextWithSaltBytes[i] = plainText[i];
            }
            for (int i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBytes[plainText.Length + i] = salt[i];
            }



          
            return algorithm.ComputeHash(plainTextWithSaltBytes);
        }

        // compare hashes
        public static bool CompareByteArrays(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length)
            {
                return false;
            }

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}