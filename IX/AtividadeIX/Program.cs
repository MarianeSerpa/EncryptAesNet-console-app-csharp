using System.Security.Cryptography;
using System.Text;

namespace AtividadeIX
{
    class Program
    {
        static void Main()
        {
            // Gere uma chave de 128 bits (16 bytes)
            byte[] chave = GerarChaveAes() ?? throw new InvalidOperationException("Erro ao gerar a chave AES.");

            // Gere um IV (Vetor de Inicialização) de 16 bytes
            byte[] iv = GerarIVAes() ?? throw new InvalidOperationException("Erro ao gerar o IV AES.");

            string encryptedData;
            string decryptedData;

            do
            {
                Console.WriteLine("Insira o dado a ser Encryptado...");
                string input = Console.ReadLine();
                encryptedData = EncryptDataWithAes(input, chave, iv);

                Console.WriteLine("\nTexto Criptografado:\n" + encryptedData);

                decryptedData = DecryptDataWithAes(encryptedData, chave, iv);
                Console.WriteLine("\nTexto Descriptografado:\n" + decryptedData);

                Console.WriteLine("\nAperte a tecla 'q' + 'Enter' para sair do programa.\nSe deseja continuar, aperte 'Enter'.");
                string letra = Console.ReadLine().ToLower();
                if (letra == "q") { break; }
            }
            while (true);

            Console.WriteLine("\nAté breve!");
        }

        static string EncryptDataWithAes(string data, byte[] chave, byte[] iv)
        {
            using Aes aesAlg = Aes.Create();
            aesAlg.Key = chave;
            aesAlg.IV = iv;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            byte[] textoBytes = Encoding.UTF8.GetBytes(data);
            byte[] textoCriptografadoBytes = encryptor.TransformFinalBlock(textoBytes, 0, textoBytes.Length);

            return Convert.ToBase64String(textoCriptografadoBytes);
        }

        static string DecryptDataWithAes(string data, byte[] chave, byte[] iv)
        {
            using Aes aesAlg = Aes.Create();
            aesAlg.Key = chave;
            aesAlg.IV = iv;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            byte[] textoCriptografadoBytes = Convert.FromBase64String(data);
            byte[] textoBytes = decryptor.TransformFinalBlock(textoCriptografadoBytes, 0, textoCriptografadoBytes.Length);

            return Encoding.UTF8.GetString(textoBytes);
        }

        static byte[]? GerarChaveAes()
        {
            using Aes aesAlg = Aes.Create();
            aesAlg.KeySize = 128; // Tamanho da chave em bits (128 bits)
            aesAlg.GenerateKey();
            return aesAlg.Key;
        }

        static byte[]? GerarIVAes()
        {
            using Aes aesAlg = Aes.Create();
            aesAlg.GenerateIV();
            return aesAlg.IV;
        }
    }
}


