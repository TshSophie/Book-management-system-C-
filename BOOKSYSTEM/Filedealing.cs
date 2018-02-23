using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BOOKSYSTEM
{
   public class Filedealing
    {
       
       static string defaultpath = "..\\..\\..\\images\\";
      
       //删除冗余文件
       public void deletefile(string filename)
        {
            string deletefilename = defaultpath;
            deletefilename += filename;
            File.Delete(defaultpath);             
        }


       //将用户选择的图片上传到系统指定目录
       public string uploadfile(string filename)
       {         
           System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1)); // 当地时区
           long timeStamp = (long)(DateTime.Now - startTime).TotalMilliseconds; // 相差毫秒数

           string newfilepath = defaultpath; //将指定上传路径复制给newfilepath
          
           newfilepath += timeStamp + ".jpg";//使用时间戳作为文件名称

           if (filename != null) //防止用户取消后出现的异常提示
           {
               File.Copy(filename, newfilepath);
               return newfilepath;
           }else
               {
                 return "";
               }


       }

    }
}
