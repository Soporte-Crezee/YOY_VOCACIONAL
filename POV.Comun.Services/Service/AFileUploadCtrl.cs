using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POV.Comun.BO;
namespace POV.Comun.Service
{
    public abstract class AFileUploadCtrl
    {
        protected string ServerURL;
        protected string Username;
        protected string Password;

        public AFileUploadCtrl(string serverUrl, string username, string password)
        {
            this.ServerURL = serverUrl;
            this.Username = username;
            this.Password = password;
            
        }

        public abstract void UploadFile(FileWrapper file, string filename);

        public abstract void DeleteFile(string filename);

        public abstract void MakeDir(string dir);
    }
}
