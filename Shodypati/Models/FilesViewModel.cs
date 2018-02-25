using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Shodypati.Helpers;


namespace Shodypati.Models
{
    public class FilesViewModel
    {
        public ViewDataUploadFilesResult[] Files { get; set; }
    }
}