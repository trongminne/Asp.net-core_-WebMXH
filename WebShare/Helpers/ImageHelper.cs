using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Security.Cryptography;

namespace WebShare.Helpers
{
    public class ImageHelper
    {
        public string SaveImage(IFormFile imageFile)
        {
            var newFileName = "";
            if (imageFile != null && imageFile.Length > 0)
            {
                var currentDateTime = DateTime.Now;
                newFileName = $"{currentDateTime:yyyyMMddHHmmss}_{imageFile.FileName}";

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image/upload", newFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }
            }

            return newFileName;
        }
		// Cắt dấu phẩy lấy ảnh
		// Trong Controller hoặc Helper
		public List<string> GetImageFileNames(string fileNames)
		{
			if (string.IsNullOrEmpty(fileNames))
			{
				return new List<string>();
			}

			return fileNames.Split(',').ToList();
		}

		// Phương thức để xoá ảnh cũ
		public void DeleteImage(string oldImagePath)
        {
            if(oldImagePath != null)
            {
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image/upload", oldImagePath);

                if (File.Exists(oldFilePath))
                {
                    File.Delete(oldFilePath);
                }
            }
        }
    }
}
