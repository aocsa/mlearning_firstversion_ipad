using MLearning.Core.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MLearning.UnifiedTouch.File
{
	public class AsyncStorageTouchService : IAsyncStorageService
	{
		public async Task<byte[]> TryReadBinaryFile(string filename)
		{

			byte[] bytes = null;
			string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			string localFilename = filename;
			var localpath = Path.Combine(documentsPath, localFilename);

			using (FileStream fs = new FileStream(localpath, FileMode.Open, FileAccess.Read))
			{ 
				MemoryStream ms;
				using ( ms = new MemoryStream())
				{
					await fs.CopyToAsync(ms);
				}

				bytes = ms.ToArray ();
				//await fs.ReadAsync (bytes, 0, (int)fs.Length);
			}

			return bytes;

			/*byte[] bytes;
			using (StreamReader reader = new StreamReader (filename)) {
				string data = reader.ReadToEnd ();	
				bytes = new byte[data.Length * sizeof(char)];
				System.Buffer.BlockCopy(data.ToCharArray(), 0, bytes, 0, bytes.Length); 
			} 
			return bytes;
			*/
		}



		public async Task<string> TryReadTextFile(string filename)
		{
			string data = null ;
			byte[] bytes = null;
			string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			string localFilename = filename;
			var localpath = Path.Combine(documentsPath, localFilename);


			using (FileStream fs = new FileStream(localpath,FileMode.Open, FileAccess.Read)) // FileMode.Create, FileAccess.Write))
			{ 
				await fs.ReadAsync(bytes, 0, (int)fs.Length);
			}
 
			System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
			data = enc.GetString (bytes);
 

			//string data;
			//using (StreamReader reader = new StreamReader (localpath)) {
			//	data = reader.ReadToEnd ();	 
			//}
			//StorageFile storageFile = await ApplicationData.Current.LocalFolder.GetFileAsync(filename); 
			//string text = await FileIO.ReadTextAsync(storageFile);
			return data;
		}
	}
}

