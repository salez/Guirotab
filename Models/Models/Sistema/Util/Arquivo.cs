using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.IO;

namespace Util
{
    /// <summary>
    /// Summary description for Arquivo
    /// </summary>
    public static class Arquivo
    {
        public static bool Exists(string filePath)
        {
            return Exists(filePath, false);
        }

        public static bool Exists(string filePath, bool delete)
        {
            if (File.Exists(filePath))
            {
                if (delete)
                {
                    File.Delete(filePath);
                }

                return true;
            }

            return false;
        }

        public static void CreateDirectoryIfNotExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// Deleta o diretorio se existir
        /// </summary>
        /// <param name="path"></param>
        /// <param name="recursive">true para deletar o diretorio e todos os arquivos do mesmo.</param>
        public static void DeleteDirectoryIfExists(string path, bool recursive)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, recursive);
            }
        }

        public static void CopyDirectoryIfExists(string source, string destination, bool overwrite)
        {
            if (Directory.Exists(source))
                CopyDirectory(source, destination, overwrite);
        }

        public static void CopyDirectory(string source, string destination, bool overwrite)
        {
            // Create the destination folder if missing.
            if (!Directory.Exists(destination))
                Directory.CreateDirectory(destination);

            DirectoryInfo dirInfo = new DirectoryInfo(source);

            // Copy all files.
            foreach (FileInfo fileInfo in dirInfo.GetFiles())
                fileInfo.CopyTo(Path.Combine(destination, fileInfo.Name), overwrite);

            // Recursively copy all sub-directories.
            foreach (DirectoryInfo subDirectoryInfo in dirInfo.GetDirectories())
                CopyDirectory(subDirectoryInfo.FullName, Path.Combine(destination, subDirectoryInfo.Name), overwrite);
        }

        //retorna conteudo do arquivo em string
        public static string LerArquivo(string path)
        {
            StreamReader reader = new StreamReader(path, true);

            string resposta = reader.ReadToEnd();
            
            reader.Close();
            reader.Dispose();

            return resposta;
        }

        //retorna conteudo do arquivo em string
        public static void EscreverArquivo(string path, string conteudo)
        {
            System.IO.File.WriteAllText(path,conteudo,Encoding.Default);
        }

    }

}