using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using POV.Comun.BO;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace POV.AmazonAWS.Services
{
    /// <summary>
    /// Clase cliente que se encarga de las operaciones sobre los servios S3 Amazon
    /// </summary>
    public class AmazonS3ClientCtrl
    {
        private string AWSAccessKey = "";
        private string AWSSecretKey = "";

        public AmazonS3ClientCtrl(string AWSAccessKey, string AWSSecretKey)
        {
            this.AWSAccessKey = AWSAccessKey;
            this.AWSSecretKey = AWSSecretKey;
        }
        /// <summary>
        /// Se encarga de subir un archivo al servidor S3
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="file"></param>
        public void UploadFile(string bucketName, FileWrapper file)
        {
            try
            {

                IAmazonS3 clientS3 = AWSClientFactory.CreateAmazonS3Client(AWSAccessKey, AWSSecretKey, RegionEndpoint.USEast1);

                PutObjectRequest request = new PutObjectRequest();
                request.BucketName = bucketName;
                request.ContentType = file.Type;
                request.Key = (file.Name);

                byte[] data = file.Data;
                Stream stream = new MemoryStream(data);
                request.InputStream = stream;

                request.CannedACL = S3CannedACL.PublicRead;

                PutObjectResponse response = clientS3.PutObject(request);

            }
            catch (AmazonS3Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Se encarga de eliminar un archivo en el servidor s3
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="fileName"></param>
        public void DeleteFile(string bucketName, string fileName)
        {
            try
            {
                IAmazonS3 clientS3 = AWSClientFactory.CreateAmazonS3Client(AWSAccessKey, AWSSecretKey, RegionEndpoint.USEast1);

                DeleteObjectRequest deleteObjectRequest =
                  new DeleteObjectRequest();
                deleteObjectRequest.BucketName = bucketName;
                deleteObjectRequest.Key = fileName;

                clientS3.DeleteObject(deleteObjectRequest);

            }
            catch (AmazonS3Exception ex)
            {
            }
        }

        public void GetFile(string bucketName, FileWrapper fileName)
        {

        }
    }
}
