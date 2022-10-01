using ImageProcessor;
using ImageProcessor.Plugins.WebP.Imaging.Formats;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WCore.Core.Infrastructure;
using WCore.Web.Areas.Admin.Models;
using SixLabors.ImageSharp.Processing.Processors.Transforms;

namespace WCore.Web.Areas.Admin.Helpers
{
    public class ImageHelper
    {

        public async System.Threading.Tasks.Task<UploadImageModel> ConvertImage(IFormFile image, bool convertToWebP, bool useResize, string directoryName)
        {
            var _webHostEnvironment = EngineContext.Current.Resolve<IWebHostEnvironment>();

            var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads\\" + directoryName);

            if (image == null)
            {
                return null;
            }

            if (image.Length < 1)
            {
                return null;
            }

            string[] allowedImageTypes = new string[] { "image/jpeg", "image/png" };
            if (!allowedImageTypes.Contains(image.ContentType.ToLower()))
            {
                return null;
            }


            var originalDirectoryPath = "uploads/" + directoryName + "/images";
            var bigDirectoryPath = "uploads/" + directoryName + "/images/big";
            var mediumDirectoryPath = "uploads/" + directoryName + "/images/medium";
            var smallDirectoryPath = "uploads/" + directoryName + "/images/small";
            if (!Directory.Exists(Path.Combine(_webHostEnvironment.WebRootPath, originalDirectoryPath)))
            {
                Directory.CreateDirectory(Path.Combine(_webHostEnvironment.WebRootPath, originalDirectoryPath));
            }
            if (!Directory.Exists(Path.Combine(_webHostEnvironment.WebRootPath, bigDirectoryPath)))
            {
                Directory.CreateDirectory(Path.Combine(_webHostEnvironment.WebRootPath, bigDirectoryPath));
            }
            if (!Directory.Exists(Path.Combine(_webHostEnvironment.WebRootPath, mediumDirectoryPath)))
            {
                Directory.CreateDirectory(Path.Combine(_webHostEnvironment.WebRootPath, mediumDirectoryPath));
            }
            if (!Directory.Exists(Path.Combine(_webHostEnvironment.WebRootPath, smallDirectoryPath)))
            {
                Directory.CreateDirectory(Path.Combine(_webHostEnvironment.WebRootPath, smallDirectoryPath));
            }

            string originalPath = Path.Combine(uploads, "images");
            string bigPath = Path.Combine(uploads, "images/big");
            string mediumPath = Path.Combine(uploads, "images/medium");
            string smallPath = Path.Combine(uploads, "images/small");
            if (!Directory.Exists(originalPath))
            {
                Directory.CreateDirectory(originalPath);
            }
            if (!Directory.Exists(bigPath))
            {
                Directory.CreateDirectory(bigPath);
            }
            if (!Directory.Exists(mediumPath))
            {
                Directory.CreateDirectory(mediumPath);
            }
            if (!Directory.Exists(smallPath))
            {
                Directory.CreateDirectory(smallPath);
            }

            if (convertToWebP)
            {
                var webPFormatImage = new UploadImageModel();

                // Prepare paths for saving images
                string webPFileName = Guid.NewGuid() + ".webp";

                string webPOriginalPath = Path.Combine(originalPath, webPFileName);
                string webPBigPath = Path.Combine(bigPath, webPFileName);
                string webPMediumPathh = Path.Combine(mediumPath, webPFileName);
                string webPSmallPath = Path.Combine(smallPath, webPFileName);
                if (!Directory.Exists(webPOriginalPath))
                {
                    Directory.CreateDirectory(webPOriginalPath);
                }
                if (!Directory.Exists(webPBigPath))
                {
                    Directory.CreateDirectory(webPBigPath);
                }
                if (!Directory.Exists(webPMediumPathh))
                {
                    Directory.CreateDirectory(webPMediumPathh);
                }
                if (!Directory.Exists(webPSmallPath))
                {
                    Directory.CreateDirectory(webPSmallPath);
                }


                // Then save in WebP format
                using (FileStream webPFileStream = new FileStream(webPOriginalPath, FileMode.Create))
                {
                    using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                    {
                        imageFactory.Load(image.OpenReadStream())
                                    .Format(new WebPFormat())
                                    .Quality(100)
                                    .Save(webPFileStream);
                    }
                }
                if (useResize)
                {
                    using (FileStream webPFileStream = new FileStream(webPBigPath, FileMode.Create))
                    {
                        using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                        {
                            imageFactory.Load(image.OpenReadStream())
                                        .Format(new WebPFormat())
                                        .Quality(100)
                                        .Save(webPFileStream);
                        }
                    }

                    using (FileStream webPFileStream = new FileStream(webPMediumPathh, FileMode.Create))
                    {
                        using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                        {
                            imageFactory.Load(image.OpenReadStream())
                                        .Format(new WebPFormat())
                                        .Quality(100)
                                        .Save(webPFileStream);
                        }
                    }

                    using (FileStream webPFileStream = new FileStream(webPSmallPath, FileMode.Create))
                    {
                        using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                        {
                            imageFactory.Load(image.OpenReadStream())
                                        .Format(new WebPFormat())
                                        .Quality(100)
                                        .Save(webPFileStream);
                        }
                    }
                }



                webPFormatImage.Original = "/" + originalDirectoryPath + "/" + webPFileName;
                webPFormatImage.Big = "/" + bigDirectoryPath + "/" + webPFileName;
                webPFormatImage.Medium = "/" + mediumDirectoryPath + "/" + webPFileName;
                webPFormatImage.Small = "/" + smallDirectoryPath + "/" + webPFileName;
                return webPFormatImage;

            }
            else
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);

                var defaultFormatImage = new UploadImageModel();

                using (var memoryStream = new MemoryStream())
                {
                    image.CopyTo(memoryStream);
                    using (var _image = Image.Load(memoryStream.ToArray()))
                    {
                        _image.Mutate(x =>
                        {
                            _image.Save(originalPath + "/" + fileName);

                            x.Resize(new ResizeOptions()
                            {
                                Mode = ResizeMode.Pad,
                                Size = new Size(300, 300)
                            });
                            _image.Save(smallPath + "/" + fileName);

                            x.Resize(new ResizeOptions()
                            {
                                Mode = ResizeMode.BoxPad,
                                Size = new Size(720, 720)
                            });
                            _image.Save(mediumPath + "/" + fileName);

                            x.Resize(new ResizeOptions()
                            {
                                Mode = ResizeMode.Min,
                                Size = new Size(1200, 1200)
                            });
                            _image.Save(bigPath + "/" + fileName);

                        });
                    }
                }

                defaultFormatImage.Original = "/" + originalDirectoryPath + "/" + fileName;
                defaultFormatImage.Big = "/" + bigDirectoryPath + "/" + fileName;
                defaultFormatImage.Medium = "/" + mediumDirectoryPath + "/" + fileName;
                defaultFormatImage.Small = "/" + smallDirectoryPath + "/" + fileName;
                return defaultFormatImage;
            }
        }
    }
}
