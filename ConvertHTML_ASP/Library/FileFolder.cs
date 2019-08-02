using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public static class FileFolder
    {
        public static void Folder(String subPath)
        {
            bool exists = System.IO.Directory.Exists(subPath);

            if (!exists)
                System.IO.Directory.CreateDirectory(subPath);
        }

        public static void File(String subPath, String contents)
        {
            bool exists = System.IO.File.Exists(subPath);

            if (!exists)
            {
                using (FileStream fs = System.IO.File.Create(subPath))
                {
                    Byte[] info = new UTF8Encoding(true).GetBytes(contents);
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }
            }
             

        }

        public static void Filecontents(String subPath, String contents)
        {
            using (var tw = new StreamWriter(subPath, false))
            {
                tw.WriteLine(contents);
            }
        }


        public static void downloadfilecss(String link, String path)
        {
            var subpath = path.Split('/');
            String sub = "";
            for (int i = 0; i < subpath.Length - 1; i++)
            {
                sub += subpath[i] + "/";
                Folder(Property.RacinePathContent + "/" + sub);
            }
            var pathlocal = Property.RacinePathContent + "\\" + path;
            using (WebClient wc = new WebClient())
            {
                wc.DownloadFile(
                    // Param1 = Link of file
                    new System.Uri(link),
                   // Param2 = Path to save
                   pathlocal
                );
            }
        }
        public static void downloadfile(String link, String path)
        {
            try
            {
 
                var subpath = path.Split('/');
                String sub = "";
                for (int i = 0; i < subpath.Length - 1; i++)
                {
                    sub += subpath[i] + "/";
                    Folder(Property.RacinePathContent + "/" + sub);
                }
                var pathlocal = Property.RacinePathContent + "\\" + path;
                using (WebClient wc = new WebClient())
                {
                    wc.DownloadFile(
                        // Param1 = Link of file
                        new System.Uri(link),
                       // Param2 = Path to save
                       pathlocal
                    );
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

    }
}
