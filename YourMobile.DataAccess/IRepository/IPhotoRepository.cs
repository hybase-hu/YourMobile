using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourMobile.Models;

namespace YourMobile.DataAccess.IRepository
{
	public interface IPhotoRepository
	{
		public void AddPhoto(int productId, string path);
		public void DeleteAllPhoto(int productId);
		public List<Photo> GetProductPhoto(int productId);
		
	}
}
