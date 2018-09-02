using DeerTier.Web.Utils;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Helpers;

namespace DeerTier.Web.Services
{
    public class WebContentService
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(WebContentService));
        private static readonly object _lock = new object();
        private static readonly IList<string> _cachedUrls = new List<string>();

        public string GetContent(string url)
        {
            // Check cache first
            var content = WebCache.Get(url) as string;
            if (content != null)
            {
                return content;
            }
            
            lock (_lock)
            {
                // Check cache again
                content = WebCache.Get(url) as string;
                if (content != null)
                {
                    return content;
                }

                // Reload content from source
                content = GetContentFromSource(url);
                if (!string.IsNullOrWhiteSpace(content))
                {
                    // Cache the content
                    WebCache.Set(url, content, minutesToCache: 60, slidingExpiration: false);
                    if (!_cachedUrls.Contains(url))
                    {
                        _cachedUrls.Add(url);
                    }

                    // Save backup copy in case the source becomes unavailable
                    SaveContentBackup(url, content);

                    return content;
                }

                // Try loading backup
                content = LoadContentBackup(url);
                if (content != null)
                {
                    return content;
                }
            }

            _logger.Error($"Failed to get content from cache, source, and backup for URL: {url}");
            return null;
        }
        
        private string GetContentFromSource(string url)
        {
            try
            {
                var request = WebRequest.CreateHttp(url);

                using (var response = request.GetResponse())
                using (var responseStream = response.GetResponseStream())
                using (var responseReader = new StreamReader(responseStream))
                {
                    var content = responseReader.ReadToEnd();
                    if (string.IsNullOrWhiteSpace(content))
                    {
                        throw new Exception("No content");
                    }
                    return content;
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to get web content for URL: {url}", ex);
                return null;
            }
        }

        private void SaveContentBackup(string url, string content)
        {
            try
            {
                var backupFile = GetBackupFilePath(url);
                File.WriteAllText(backupFile, content, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to save content backup for URL: {url}", ex);
            }
        }

        private string LoadContentBackup(string url)
        {
            try
            {
                var backupFile = GetBackupFilePath(url);
                if (File.Exists(backupFile))
                {
                    return File.ReadAllText(backupFile, Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Filed to load conteent backup for URL: {url}", ex);
            }

            return null;
        }

        private string GetBackupFilePath(string url)
        {
            var fileName = PathUtil.SanitizeFileName(url);
            return Path.Combine(PathUtil.AppPath, ConfigHelper.WebContentBackupPath, fileName);
        }

        public void FlushCache()
        {
            lock (_lock)
            {
                try
                {
                    foreach (var url in _cachedUrls)
                    {
                        WebCache.Remove(url);
                    }

                    _cachedUrls.Clear();
                }
                catch (Exception ex)
                {
                    _logger.Error("Failed to flush the cache", ex);
                }
            }   
        }
    }
}