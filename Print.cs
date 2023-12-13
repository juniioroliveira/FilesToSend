using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FilesToSend
{
    public class Print
    {
        public void Log(string message, string nameService)
        {
            try
            {
                // Definindo os caracteres a serem removidos
                string caracteresParaRemover = "/ ";

                // Convertendo a string de caracteres para um array de caracteres
                char[] charsParaRemover = caracteresParaRemover.ToCharArray();

                // Criando a expressão regular para substituir os caracteres
                string padrao = "[" + Regex.Escape(new string(charsParaRemover)) + "]";
                string resultado = Regex.Replace(nameService, padrao, "");


                string _logFilePath = Path.Combine(Directories.LogPath, resultado);

                string currentDate = DateTime.Now.ToString("MM-yyyy");
                string currentHour = DateTime.Now.ToString("HH:mm:ss");

                string logDirectory = Path.Combine(_logFilePath, currentDate);
                string logFilePath = Path.Combine(logDirectory, $"{(string)DateTime.Now.ToString("dd")}.log");

                if (!Directory.Exists(logDirectory)) Directory.CreateDirectory(logDirectory);

                File.AppendAllText(logFilePath, $"{currentHour} {message}\n");
            }
            catch (Exception ex)
            {

            }
        }
    }
}
