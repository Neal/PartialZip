using System;
using System.IO;
using System.Net;
using System.Collections;
using ICSharpCode.SharpZipLib.Zip;
using RemoteZip;

namespace PartialZip
{
    class Program
    {
        public static void CopyStream(Stream input, Stream output)
        {
            int num = 0;
            byte[] buffer = new byte[0x2000];
            while (InlineAssignHelper<int>(ref num, input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, num);
            }
        }

        private static T InlineAssignHelper<T>(ref T target, T value)
        {
            target = value;
            return value;
        }

		public static bool DownloadFileFromZip(string ZipURL, string FilePathInZip, string LocalPath)
		{
			bool ret = false;
			RemoteZipFile file = new RemoteZipFile();
			if (file.Load(ZipURL))
			{
				try
				{
					IEnumerator enumerator = file.GetEnumerator();
					while (enumerator.MoveNext())
					{
						ZipEntry current = (ZipEntry)enumerator.Current;
						if (current.Name == FilePathInZip)
						{
							FileStream output = new FileStream(LocalPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
							CopyStream(file.GetInputStream(current), output);
							output.Close();
							ret = true;
						}
					}
					if (enumerator is IDisposable) (enumerator as IDisposable).Dispose();
				}
				catch (Exception) { }
			}
			return ret;
		}

		public static bool isURLValid(string url)
		{
			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
				request.Timeout = 5000;
				request.Method = "HEAD";
				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
				int statusCode = (int)response.StatusCode;
				return (statusCode >= 100 && statusCode < 400);
			}
			catch (WebException) { }
			catch (Exception) { }
			return false;
		}

        static void Main(string[] args)
        {
            string CurrentProcessName = Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);

            if (args.Length != 3)
            {
				Console.WriteLine("Usage: {0} <ZipURL> <ZipFilePath> <LocalFilePath>", CurrentProcessName);
                Environment.Exit(1);
            }

			if (!isURLValid(args[0]))
			{
				Console.WriteLine("Invalid URL!");
				Environment.Exit(2);
			}

            Console.Error.Write("Downloading {0}...", Path.GetFileName(args[1]));
			Console.Error.WriteLine(DownloadFileFromZip(args[0], args[1], args[2]) ? "   Done!" : "   Remote file not found!");
            Environment.Exit(0);
        }
    }
}
