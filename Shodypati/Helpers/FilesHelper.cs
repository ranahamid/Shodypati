using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Hosting;

namespace Shodypati.Helpers
{
    public class FilesHelper
    {
        private readonly string DeleteType;

        private readonly string DeleteURL;

        private readonly string serverMapPath;
        private readonly string StorageRoot;
        private readonly string tempPath;
        private readonly string UrlBase;

        public FilesHelper(string DeleteURL, string DeleteType, string StorageRoot, string UrlBase, string tempPath,
            string serverMapPath)
        {
            this.DeleteURL = DeleteURL;
            this.DeleteType = DeleteType;
            this.StorageRoot = StorageRoot;
            this.UrlBase = UrlBase;
            this.tempPath = tempPath;
            this.serverMapPath = serverMapPath;
        }

        public void DeleteFiles(string pathToDelete)
        {
            var path = HostingEnvironment.MapPath(pathToDelete);

            Debug.WriteLine(path);
            if (Directory.Exists(path))
            {
                var di = new DirectoryInfo(path);
                foreach (var fi in di.GetFiles())
                {
                    File.Delete(fi.FullName);
                    Debug.WriteLine(fi.Name);
                }

                di.Delete(true);
            }
        }

        public string DeleteFile(string file)
        {
            Debug.WriteLine("DeleteFile");
            //    var req = HttpContext.Current;
            Debug.WriteLine(file);

            var fullPath = Path.Combine(StorageRoot, file);
            Debug.WriteLine(fullPath);
            Debug.WriteLine(File.Exists(fullPath));
            var thumbPath = "/" + file + "80x80.jpg";
            var partThumb1 = Path.Combine(StorageRoot, "thumbs");
            var partThumb2 = Path.Combine(partThumb1, file + "80x80.jpg");

            Debug.WriteLine(partThumb2);
            Debug.WriteLine(File.Exists(partThumb2));
            if (File.Exists(fullPath))
            {
                //delete thumb 
                if (File.Exists(partThumb2)) File.Delete(partThumb2);
                File.Delete(fullPath);
                var succesMessage = "Ok";
                return succesMessage;
            }

            var failMessage = "Error Delete";
            return failMessage;
        }

        //Edit
        public JsonFiles GetFileList()
        {
            var r = new List<ViewDataUploadFilesResult>();

            var fullPath = Path.Combine(StorageRoot);
            if (Directory.Exists(fullPath))
            {
                var dir = new DirectoryInfo(fullPath);
                foreach (var file in dir.GetFiles())
                {
                    var SizeInt = unchecked((int) file.Length);
                    r.Add(UploadResult(file.Name, SizeInt, file.FullName));
                }
            }

            var files = new JsonFiles(r);

            return files;
        }

        public void UploadAndShowResults(HttpContextBase ContentBase, List<ViewDataUploadFilesResult> resultList)
        {
            var httpRequest = ContentBase.Request;
            Debug.WriteLine(Directory.Exists(tempPath));

            var fullPath = Path.Combine(StorageRoot);
            Directory.CreateDirectory(fullPath);
            // Create new folder for thumbs
            Directory.CreateDirectory(fullPath + "/thumbs/");

            foreach (string inputTagName in httpRequest.Files)
            {
                var headers = httpRequest.Headers;

                var file = httpRequest.Files[inputTagName];
                Debug.WriteLine(file.FileName);

                if (string.IsNullOrEmpty(headers["X-File-Name"]))
                    UploadWholeFile(ContentBase, resultList);
                else
                    UploadPartialFile(headers["X-File-Name"], ContentBase, resultList);
            }
        }


        private void UploadWholeFile(HttpContextBase requestContext, List<ViewDataUploadFilesResult> statuses)
        {
            var request = requestContext.Request;
            for (var i = 0; i < request.Files.Count; i++)
            {
                var file = request.Files[i];
                var pathOnServer = Path.Combine(StorageRoot);
                var fullPath = Path.Combine(pathOnServer, Path.GetFileName(file.FileName));
                file.SaveAs(fullPath);

                //Create thumb
                var imageArray = file.FileName.Split('.');
                if (imageArray.Length != 0)
                {
                    var extansion = imageArray[imageArray.Length - 1].ToLower();
                    if (extansion != "jpg" && extansion != "png" && extansion != "jpeg"
                    ) //Do not create thumb if file is not an image
                    {
                    }
                    else
                    {
                        var ThumbfullPath = Path.Combine(pathOnServer, "thumbs");
                        var fileThumb = Path.GetFileNameWithoutExtension(file.FileName) + "80x80.jpg";
                        var ThumbfullPath2 = Path.Combine(ThumbfullPath, fileThumb);
                        using (var stream = new MemoryStream(File.ReadAllBytes(fullPath)))
                        {
                            var thumbnail = new WebImage(stream).Resize(80, 80);
                            thumbnail.Save(ThumbfullPath2, "jpg");
                        }
                    }
                }

                statuses.Add(UploadResult(file.FileName, file.ContentLength, file.FileName));
            }
        }


        private void UploadPartialFile(string fileName, HttpContextBase requestContext,
            List<ViewDataUploadFilesResult> statuses)
        {
            var request = requestContext.Request;
            if (request.Files.Count != 1)
                throw new HttpRequestValidationException(
                    "Attempt to upload chunked file containing more than one fragment per request");
            var file = request.Files[0];
            var inputStream = file.InputStream;
            var patchOnServer = Path.Combine(StorageRoot);
            var fullName = Path.Combine(patchOnServer, Path.GetFileName(file.FileName));
            var ThumbfullPath = Path.Combine(fullName, Path.GetFileName(file.FileName + "80x80.jpg"));
            var handler = new ImageHandler();

            var ImageBit = ImageHandler.LoadImage(fullName);
            handler.Save(ImageBit, 80, 80, 10, ThumbfullPath);
            using (var fs = new FileStream(fullName, FileMode.Append, FileAccess.Write))
            {
                var buffer = new byte[1024];

                var l = inputStream.Read(buffer, 0, 1024);
                while (l > 0)
                {
                    fs.Write(buffer, 0, l);
                    l = inputStream.Read(buffer, 0, 1024);
                }

                fs.Flush();
                fs.Close();
            }

            statuses.Add(UploadResult(file.FileName, file.ContentLength, file.FileName));
        }

        public ViewDataUploadFilesResult UploadResult(string FileName, int fileSize, string FileFullPath)
        {
            var getType = MimeMapping.GetMimeMapping(FileFullPath);
            var result = new ViewDataUploadFilesResult
            {
                name = FileName,
                size = fileSize,
                type = getType,
                url = UrlBase + FileName,
                deleteUrl = DeleteURL + FileName,
                thumbnailUrl = CheckThumb(getType, FileName),
                deleteType = DeleteType
            };
            return result;
        }

        public string CheckThumb(string type, string FileName)
        {
            var splited = type.Split('/');
            if (splited.Length == 2)
            {
                var extansion = splited[1].ToLower();
                if (extansion.Equals("jpeg") || extansion.Equals("jpg") || extansion.Equals("png") ||
                    extansion.Equals("gif"))
                {
                    var thumbnailUrl = UrlBase + "thumbs/" + Path.GetFileNameWithoutExtension(FileName) + "80x80.jpg";
                    return thumbnailUrl;
                }
                else
                {
                    if (extansion.Equals("octet-stream")) //Fix for exe files
                        return "/Content/Free-file-icons/48px/exe.png";
                    if (extansion.Contains("zip")) //Fix for exe files
                        return "/Content/Free-file-icons/48px/zip.png";
                    var thumbnailUrl = "/Content/Free-file-icons/48px/" + extansion + ".png";
                    return thumbnailUrl;
                }
            }

            return UrlBase + "/thumbs/" + Path.GetFileNameWithoutExtension(FileName) + "80x80.jpg";
        }

        //Edit.... From DB
        public List<string> FilesList()
        {
            var Filess = new List<string>();
            var path = HostingEnvironment.MapPath(serverMapPath);
            Debug.WriteLine(path);
            if (Directory.Exists(path))
            {
                var di = new DirectoryInfo(path);
                foreach (var fi in di.GetFiles())
                {
                    Filess.Add(fi.Name);
                    Debug.WriteLine(fi.Name);
                }
            }

            return Filess;
        }
    }

    public class ViewDataUploadFilesResult
    {
        public Guid Id { get; set; }
        public string name { get; set; }
        public int size { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string deleteUrl { get; set; }
        public string thumbnailUrl { get; set; }
        public string deleteType { get; set; }
    }

    public class JsonFiles
    {
        public ViewDataUploadFilesResult[] files;

        public JsonFiles(List<ViewDataUploadFilesResult> filesList)
        {
            files = new ViewDataUploadFilesResult[filesList.Count];
            for (var i = 0; i < filesList.Count; i++) files[i] = filesList.ElementAt(i);
        }

        public string TempFolder { get; set; }
    }
}