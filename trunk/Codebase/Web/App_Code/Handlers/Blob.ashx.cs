using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using BUDI2_NS.Data;

namespace BUDI2_NS.Handlers
{
	public partial class BlobFactory
    {
        
        static BlobFactory()
        {
            // register blob handlers
            RegisterHandler("PhotosPhoto", "\"dbo\".\"Photos\"", "\"Photo\"", new string[] {
                        "\"ID\""}, "Photos Photo", String.Empty);
        }
    }
    
    public enum BlobMode
    {
        
        Thumbnail,
        
        Original,
        
        Upload,
    }
    
    public class BlobHandlerInfo
    {
        
        private string _key;
        
        private string _tableName;
        
        private string _fieldName;
        
        private string[] _keyFieldNames;
        
        private string _text;
        
        private string _contentType;
        
        public BlobHandlerInfo()
        {
        }
        
        public BlobHandlerInfo(string key, string tableName, string fieldName, string[] keyFieldNames, string text, string contentType)
        {
            this.Key = key;
            this.TableName = tableName;
            this.FieldName = fieldName;
            this.KeyFieldNames = keyFieldNames;
            this.Text = text;
            this._contentType = contentType;
        }
        
        public virtual string Key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = value;
            }
        }
        
        protected string this[string name]
        {
            get
            {
                return ((string)(HttpContext.Current.Items[("BlobHandlerInfo_" + name)]));
            }
            set
            {
                HttpContext.Current.Items[("BlobHandlerInfo_" + name)] = value;
            }
        }
        
        public virtual string TableName
        {
            get
            {
                return _tableName;
            }
            set
            {
                _tableName = value;
            }
        }
        
        public virtual string FieldName
        {
            get
            {
                return _fieldName;
            }
            set
            {
                _fieldName = value;
            }
        }
        
        public virtual string[] KeyFieldNames
        {
            get
            {
                return _keyFieldNames;
            }
            set
            {
                _keyFieldNames = value;
            }
        }
        
        public virtual string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
            }
        }
        
        public virtual string Error
        {
            get
            {
                return this["Error"];
            }
            set
            {
                this["Error"] = value;
            }
        }
        
        public virtual string FileName
        {
            get
            {
                return this["FileName"];
            }
            set
            {
                this["FileName"] = value;
            }
        }
        
        public virtual string ContentType
        {
            get
            {
                string s = this["ContentType"];
                if (String.IsNullOrEmpty(s))
                	s = this._contentType;
                return s;
            }
            set
            {
                this["ContentType"] = value;
            }
        }
        
        public static BlobHandlerInfo Current
        {
            get
            {
                BlobHandlerInfo d = ((BlobHandlerInfo)(HttpContext.Current.Items["BlobHandler_Current"]));
                if (d == null)
                	foreach (string key in HttpContext.Current.Request.QueryString.Keys)
                    	if (!(String.IsNullOrEmpty(key)) && BlobFactory.Handlers.ContainsKey(key))
                        {
                            d = BlobFactory.Handlers[key];
                            HttpContext.Current.Items["BlobHandler_Current"] = d;
                            break;
                        }
                return d;
            }
        }
        
        public BlobMode Mode
        {
            get
            {
                if (Value.StartsWith("u|"))
                	return BlobMode.Upload;
                if (Value.StartsWith("t|"))
                	return BlobMode.Thumbnail;
                else
                	return BlobMode.Original;
            }
        }
        
        public string Value
        {
            get
            {
                return HttpContext.Current.Request.QueryString[Key];
            }
        }
        
        public string Reference
        {
            get
            {
                string s = Value.Replace("|", "_");
                return s.Substring(1);
            }
        }
        
        public virtual bool SaveFile(HttpContext context)
        {
            if (context.Request.Files.Count != 1)
            	return false;
            HttpPostedFile file = context.Request.Files[0];
            try
            {
                using (SqlStatement updateBlob = BlobFactory.CreateBlobUpdateStatement())
                	return (updateBlob.ExecuteNonQuery() == 1);
            }
            catch (Exception err)
            {
                Error = err.Message;
                return false;
            }
        }
        
        public virtual void LoadFile(Stream stream)
        {
            using (SqlStatement getBlob = BlobFactory.CreateBlobSelectStatement())
            	if (getBlob.Read())
                {
                    if (!(DBNull.Value.Equals(getBlob[0])))
                    	if (typeof(string).Equals(getBlob.Reader.GetFieldType(0)))
                        {
                            byte[] stringData = Encoding.Default.GetBytes(((string)(getBlob[0])));
                            stream.Write(stringData, 0, stringData.Length);
                        }
                        else
                        {
                            byte[] buffer = new byte[(1024 * 64)];
                            long offset = 0;
                            long bytesRead = getBlob.Reader.GetBytes(0, offset, buffer, 0, buffer.Length);
                            while (bytesRead > 0)
                            {
                                stream.Write(buffer, 0, Convert.ToInt32(bytesRead));
                                offset = (offset + bytesRead);
                                bytesRead = getBlob.Reader.GetBytes(0, offset, buffer, 0, buffer.Length);
                            }
                        }
                }
        }
    }
    
    public partial class BlobFactory
    {
        
        public static SortedDictionary<string, BlobHandlerInfo> Handlers = new SortedDictionary<string, BlobHandlerInfo>();
        
        public static void RegisterHandler(string key, string tableName, string fieldName, string[] keyFieldNames, string text, string contentType)
        {
            Handlers.Add(key, new BlobHandlerInfo(key, tableName, fieldName, keyFieldNames, text, contentType));
        }
        
        public static SqlStatement CreateBlobSelectStatement()
        {
            BlobHandlerInfo handler = BlobHandlerInfo.Current;
            if (handler != null)
            {
                string parameterMarker = SqlStatement.GetParameterMarker(String.Empty);
                List<string> keyValues = new List<string>();
                Match m = Regex.Match(handler.Value, "\\|([\\w-]+)", RegexOptions.Compiled);
                while (m.Success)
                {
                    keyValues.Add(m.Groups[1].Value);
                    m = m.NextMatch();
                }
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("select {0} from {1} where ", handler.FieldName, handler.TableName);
                for (int i = 0; (i < handler.KeyFieldNames.Length); i++)
                {
                    if (i > 0)
                    	sb.Append(" and ");
                    sb.AppendFormat("{0}={1}p{2}", handler.KeyFieldNames[i], parameterMarker, i);
                }
                SqlText getBlob = new SqlText(sb.ToString());
                for (int j = 0; (j < handler.KeyFieldNames.Length); j++)
                	getBlob.AddParameter(String.Format("{0}p{1}", parameterMarker, j), keyValues[j]);
                return getBlob;
            }
            return null;
        }
        
        public static SqlStatement CreateBlobUpdateStatement()
        {
            BlobHandlerInfo handler = BlobHandlerInfo.Current;
            if (handler != null)
            {
                string parameterMarker = SqlStatement.GetParameterMarker(String.Empty);
                List<string> keyValues = new List<string>();
                Match m = Regex.Match(handler.Value, "\\|([\\w-]+)", RegexOptions.Compiled);
                while (m.Success)
                {
                    keyValues.Add(m.Groups[1].Value);
                    m = m.NextMatch();
                }
                HttpPostedFile file = HttpContext.Current.Request.Files[0];
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("update {0} set {1} = ", handler.TableName, handler.FieldName);
                if (file.ContentLength == 0)
                	sb.Append("null");
                else
                	sb.AppendFormat("{0}blob", parameterMarker);
                sb.Append(" where ");
                for (int i = 0; (i < handler.KeyFieldNames.Length); i++)
                {
                    if (i > 0)
                    	sb.Append(" and ");
                    sb.AppendFormat("{0}={1}p{2}", handler.KeyFieldNames[i], parameterMarker, i);
                }
                SqlText updateBlob = new SqlText(sb.ToString());
                if (file.ContentLength > 0)
                {
                    byte[] data = new byte[file.ContentLength];
                    file.InputStream.Read(data, 0, data.Length);
                    updateBlob.AddParameter((parameterMarker + "blob"), data);
                }
                for (int j = 0; (j < handler.KeyFieldNames.Length); j++)
                	updateBlob.AddParameter(String.Format("{0}p{1}", parameterMarker, j), keyValues[j]);
                return updateBlob;
            }
            return null;
        }
    }
    
    public class Blob : IHttpHandler
    {
        
        public const int ThumbnailCacheTimeout = 5;
        
        public static SortedDictionary<Guid, string> ImageFormats;
        
        static Blob()
        {
            ImageFormats = new SortedDictionary<Guid, string>();
            ImageFormats.Add(ImageFormat.Bmp.Guid, "image/bmp");
            ImageFormats.Add(ImageFormat.Emf.Guid, "image/emf");
            ImageFormats.Add(ImageFormat.Exif.Guid, "image/bmp");
            ImageFormats.Add(ImageFormat.Gif.Guid, "image/gif");
            ImageFormats.Add(ImageFormat.Jpeg.Guid, "image/jpeg");
            ImageFormats.Add(ImageFormat.Png.Guid, "image/png");
            ImageFormats.Add(ImageFormat.Tiff.Guid, "image/tiff");
            ImageFormats.Add(ImageFormat.Wmf.Guid, "image/Wmf");
        }
        
        bool IHttpHandler.IsReusable
        {
            get
            {
                return false;
            }
        }
        
        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            BlobHandlerInfo handler = BlobHandlerInfo.Current;
            if (handler == null)
            	throw new HttpException(404, String.Empty);
            if (handler.Mode == BlobMode.Upload)
            	RenderUploader(context, handler, handler.SaveFile(context));
            else
            {
                string tempFileName = Path.GetTempFileName();
                Stream stream = File.Create(tempFileName);
                handler.LoadFile(stream);
                CopyToOutput(context, stream, handler);
                stream.Close();
                File.Delete(tempFileName);
            }
            if (handler.Mode != BlobMode.Thumbnail)
            	context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        }
        
        private void CopyToOutput(HttpContext context, Stream stream, BlobHandlerInfo handler)
        {
            int offset = 0;
            stream.Position = offset;
            byte[] buffer = null;
            Image img = null;
            // attempt to auto-detect content type as an image
            if (String.IsNullOrEmpty(handler.ContentType) && (stream.Length > 0))
            	try
                {
                    img = Image.FromStream(stream);
                }
                catch (Exception )
                {
                    try
                    {
                        // Correction for Northwind database image format
                        offset = 78;
                        stream.Position = offset;
                        buffer = new byte[(stream.Length - offset)];
                        stream.Read(buffer, 0, buffer.Length);
                        img = Image.FromStream(new MemoryStream(buffer, 0, buffer.Length));
                    }
                    catch (Exception ex)
                    {
                        context.Trace.Write(ex.ToString());
                    }
                }
            // send an original or a thumbnail to the output
            if (handler.Mode == BlobMode.Thumbnail)
            {
                // draw a thumbnail
                Bitmap thumbnail = new Bitmap(92, 71);
                Graphics g = Graphics.FromImage(thumbnail);
                Rectangle r = new Rectangle(0, 0, 91, 70);
                g.FillRectangle(Brushes.White, r);
                g.DrawRectangle(Pens.Silver, r);
                double thumbnailAspect = (Convert.ToDouble(r.Height) / Convert.ToDouble(r.Width));
                if (img != null)
                {
                    r.Inflate(-2, -2);
                    if (img.Width > img.Height)
                    {
                        r.Height = Convert.ToInt32((Convert.ToDouble(r.Width) * thumbnailAspect));
                        r.Width = Convert.ToInt32((Convert.ToDouble(r.Height) 
                                        * (Convert.ToDouble(img.Width) / Convert.ToDouble(img.Height))));
                    }
                    else
                    	if (img.Height > img.Width)
                        {
                            r.Width = Convert.ToInt32((Convert.ToDouble(r.Height) * thumbnailAspect));
                            r.Height = Convert.ToInt32((Convert.ToDouble(r.Width) 
                                            * (Convert.ToDouble(img.Height) / Convert.ToDouble(img.Width))));
                        }
                        else
                        {
                            r.Width = Convert.ToInt32((Convert.ToDouble(img.Height) * thumbnailAspect));
                            r.Height = r.Width;
                        }
                    double aspect = (Convert.ToDouble((thumbnail.Width - 4)) / r.Width);
                    if (r.Width <= r.Height)
                    	aspect = (Convert.ToDouble((thumbnail.Height - 4)) / r.Height);
                    if (aspect > 1)
                    	aspect = 1;
                    r.Width = Convert.ToInt32((Convert.ToDouble(r.Width) * aspect));
                    r.Height = Convert.ToInt32((Convert.ToDouble(r.Height) * aspect));
                    r.Location = new Point(((thumbnail.Width - r.Width) 
                                    / 2), ((thumbnail.Height - r.Height) 
                                    / 2));
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.DrawImage(img, r);
                }
                else
                {
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                    Font f = new Font("Arial", ((float)(7.5)));
                    g.DrawString(handler.Text, f, Brushes.Black, r);
                }
                // produce thumbnail data
                MemoryStream ts = new MemoryStream();
                thumbnail.Save(ts, ImageFormat.Png);
                ts.Flush();
                ts.Position = 0;
                byte[] td = new byte[ts.Length];
                ts.Read(td, 0, td.Length);
                ts.Close();
                // Send thumbnail to the output
                context.Response.AddHeader("Content-Length", td.Length.ToString());
                context.Response.ContentType = "image/png";
                context.Response.OutputStream.Write(td, 0, td.Length);
                if (img == null)
                	context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                else
                {
                    context.Response.Cache.SetCacheability(HttpCacheability.Public);
                    context.Response.Cache.SetExpires(DateTime.Now.AddMinutes(Blob.ThumbnailCacheTimeout));
                }
            }
            else
            {
                string contentType = handler.ContentType;
                if (img != null)
                	contentType = ImageFormats[img.RawFormat.Guid];
                if (String.IsNullOrEmpty(contentType))
                	contentType = "application/octet-stream";
                string fileName = handler.FileName;
                if (String.IsNullOrEmpty(fileName))
                	fileName = String.Format("{0}{1}.{2}", handler.Key, handler.Reference, contentType.Substring((contentType.IndexOf("/") + 1)));
                context.Response.ContentType = contentType;
                //context.Response.AddHeader("Content-Disposition", ("filename=" + fileName));
                //context.Response.AppendHeader("content-disposition", "attachment; filename=" + fileName);
                context.Response.AppendHeader("content-disposition", "filename=" + fileName);
                //context.Response.AddHeader("Content-Length", stream.Length.ToString());
                context.Response.AddHeader("content-length", stream.Length.ToString());
                stream.Position = offset;
                buffer = new byte[(1024 * 32)];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                while (bytesRead > 0)
                {
                    context.Response.OutputStream.Write(buffer, 0, bytesRead);
                    offset = (offset + bytesRead);
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                }
            }
        }
        
        private void RenderUploader(HttpContext context, BlobHandlerInfo handler, bool uploadSuccess)
        {
            HtmlTextWriter writer = new HtmlTextWriter(context.Response.Output);
            writer.WriteLine("<!DOCTYPE html PUBLIC \\\"-//W3C//DTD XHTML 1.0 Transitional//EN\\\" \\\"http://www.w3." +
                    "org/TR/xhtml1/DTD/xhtml1-transitional.dtd\\\">");
            writer.AddAttribute("xmlns", "http://www.w3.org/1999/xhtml");
            writer.RenderBeginTag(HtmlTextWriterTag.Html);
            // head
            writer.RenderBeginTag(HtmlTextWriterTag.Head);
            writer.RenderBeginTag(HtmlTextWriterTag.Title);
            writer.Write("Uploader");
            writer.RenderEndTag();
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
            writer.RenderBeginTag(HtmlTextWriterTag.Script);
            writer.WriteLine(@" 
function ShowUploadControls() { 
    document.getElementById('UploadControlsPanel').style.display ='block'; 
    document.getElementById('StartUploadPanel').style.display = 'none';   
    document.getElementById('FileUpload').focus();      
} 
function Owner() {
    var m = window.location.href.match(/owner=(.+?)&/);
    return m ? parent.$find(m[1]) : null;
}
function StartUpload(msg) {
    if (msg && !window.confirm(msg)) return;
    if (parent && parent.window.Web) {
        var m = window.location.href.match(/&index=(\d+)$/);
        if (m) Owner()._showUploadProgress(m[1]);
    }
    document.forms[0].submit();
}
function UploadSuccess(key, message) { 
    if (parent && parent.window.Web) { 
        parent.Web.DataView.showMessage(message); 
        Owner().refresh();
    }     
    else 
        alert('Success');         
}");
            writer.RenderEndTag();
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/css");
            writer.RenderBeginTag(HtmlTextWriterTag.Style);
            writer.WriteLine("body{font-family:tahoma;font-size:8.5pt;margin:4px;background-color:white;}");
            writer.WriteLine("input{font-family:tahoma;font-size:8.5pt;}");
            writer.WriteLine("input.FileUpload{padding:3px}");
            writer.RenderEndTag();
            writer.RenderEndTag();
            // body
            string message = null;
            if (uploadSuccess)
            	if (HttpContext.Current.Request.Files[0].ContentLength > 0)
                	message = String.Format("<b>Confirmation:</b> {0} has been uploaded successfully. <b>It may take up to {1}" +
                            " minutes for the thumbnail to reflect the uploaded content.</b>", handler.Text.ToLower(), Blob.ThumbnailCacheTimeout);
                else
                	message = String.Format("<b>Confirmation:</b> {0} has been cleared.", handler.Text.ToLower());
            else
            	if (!(String.IsNullOrEmpty(handler.Error)))
                	message = String.Format("<b>Error:</b> failed to upload {0}. {1}", handler.Text.ToLower(), handler.Error.Replace("\'", "\\\'"));
            if (!(String.IsNullOrEmpty(message)))
            	writer.AddAttribute("onload", String.Format("UploadSuccess(\'{0}={1}\', \'{2}\')", handler.Key, handler.Value.Replace("u|", "t|"), message));
            writer.RenderBeginTag(HtmlTextWriterTag.Body);
            // form
            writer.AddAttribute(HtmlTextWriterAttribute.Name, "form1");
            writer.AddAttribute("method", "post");
            writer.AddAttribute("action", context.Request.RawUrl);
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "form1");
            writer.AddAttribute("enctype", "multipart/form-data");
            writer.RenderBeginTag(HtmlTextWriterTag.Form);
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            // begin "start upload" controls
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "StartUploadPanel");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            writer.Write("Click ");
            writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "ShowUploadControls();return false");
            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.Write("here");
            writer.RenderEndTag();
            writer.Write(" to upload or clear {0} file.", handler.Text.ToLower());
            // end of "start upload" controls
            writer.RenderEndTag();
            // begin "upload controls"
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "UploadControlsPanel");
            writer.AddAttribute(HtmlTextWriterAttribute.Style, "display:none");
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
            // "FileUpload" input
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "File");
            writer.AddAttribute(HtmlTextWriterAttribute.Name, "FileUpload");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "FileUpload");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "FileUpload");
            writer.AddAttribute(HtmlTextWriterAttribute.Onchange, "StartUpload()");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
            // "FileClear" input
            writer.AddAttribute(HtmlTextWriterAttribute.Type, "button");
            writer.AddAttribute(HtmlTextWriterAttribute.Id, "FileClear");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "FileClear");
            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "StartUpload(\'Clear?\')");
            writer.AddAttribute(HtmlTextWriterAttribute.Value, "Clear");
            writer.RenderBeginTag(HtmlTextWriterTag.Input);
            writer.RenderEndTag();
            // end of "upload controls"
            writer.RenderEndTag();
            // close "div"
            writer.RenderEndTag();
            // close "form"
            writer.RenderEndTag();
            // close "body"
            writer.RenderEndTag();
            // close "html"
            writer.RenderEndTag();
            writer.Close();
        }
    }
}
