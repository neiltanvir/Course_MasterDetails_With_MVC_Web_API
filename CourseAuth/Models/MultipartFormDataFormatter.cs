using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace CourseAuth.Models
{
    public class MultipartFormDataFormatter : MediaTypeFormatter
    {
        public MultipartFormDataFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("multipart/form-data"));
        }

        public override bool CanReadType(Type type)
        {
            return type == typeof(CourseRequest);
        }

        public override bool CanWriteType(Type type)
        {
            return false;
        }

        public override async Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            var multipartData = await content.ReadAsMultipartAsync();
            var courseData = new CourseRequest();

            foreach (var contentPart in multipartData.Contents)
            {
                var fieldName = contentPart.Headers.ContentDisposition.Name.Trim('\"');

                if (fieldName == "Course")
                {
                    var courseContent = await contentPart.ReadAsStringAsync();
                    courseData.Course = JsonConvert.DeserializeObject<Course>(courseContent);
                }
                else if (fieldName == "ImageFile")
                {
                    courseData.ImageFile = await contentPart.ReadAsByteArrayAsync();
                    courseData.ImageFileName = contentPart.Headers.ContentDisposition.FileName;
                }
            }

            return courseData;
        }
    }
}