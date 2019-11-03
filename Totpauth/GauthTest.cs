// Set project to "Windows Application" and make this the startup object.
using System;

namespace pGina.Plugin.Totpauth{

    class GauthTest {

        static void Main(string[] args){

            var gauth = new Gauth();

            Console.WriteLine(gauth.OneTimePassword);

            Console.WriteLine(gauth.QrCodeUrl);
        }
    }
}
