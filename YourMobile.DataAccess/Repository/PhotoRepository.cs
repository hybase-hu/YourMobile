using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourMobile.DataAccess.Data;
using YourMobile.DataAccess.IRepository;
using YourMobile.Models;

namespace YourMobile.DataAccess.Repository
{
	public class PhotoRepository : IPhotoRepository
	{
		private readonly AppDbContext _context;

		public PhotoRepository(AppDbContext context)
		{
			_context = context;
		}

		public void AddPhoto(int productId, string path)
		{
			_context.Photos.Add(new Models.Photo
			{
				ProductId = productId,
				imageUrl = path
			});
			_context.SaveChanges();
		}

		public void DeleteAllPhoto(int productId)
		{
			if (productId != null)
			{
				_context.Photos.Where(u => u.ProductId == productId).ExecuteDelete();
			}
		}

		public List<Photo> GetProductPhoto(int productId)
		{
			return _context.Photos.Where(p=>p.ProductId == productId).ToList();
		}
	}
}
