﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ElFinder.DTO;
using ElFinder.Response;
using System;
using System.Collections;
using stellar;
using stellar.Services;

namespace ElFinder {
    /// <summary>
    /// Represents a driver for local file system
    /// </summary>
    public class FileSystemDriver : IDriver {
        #region private
        private const string _volumePrefix = "v";
        private List<Root> _roots;

        /// <summary> </summary>
        private JsonResult Json(object data) {
            return new JsonDataContractResult(data) { JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        /// <summary> </summary>
        public FullPath ParsePath(string target) {
            StringBuilder volumeIdBuilder = new StringBuilder();
            StringBuilder pathBuilder = null;
            foreach (var c in target) {
                if (pathBuilder != null) {
                    pathBuilder.Append(c);
                } else {
                    volumeIdBuilder.Append(c);
                    if (c == '_') {
                        pathBuilder = new StringBuilder();
                    }
                }
            }
            Root root = _roots.First(r => r.VolumeId == volumeIdBuilder.ToString());
            string path = file_helper.decode_path(pathBuilder.ToString());
            string dirUrl = path != root.Directory.Name ? path : string.Empty;
            var dir = new DirectoryInfo(root.Directory.FullName + dirUrl);
            if (dir.Exists) {
                string parentPath = dir.FullName.Substring(root.Directory.FullName.Length).Replace('\\', '/');
                return new FullPath() { Directory = dir, Root = root, RelativePath = root.Alias + parentPath };
            } else {
                var file = new FileInfo(root.Directory.FullName + dirUrl);
                string parentPath = file.FullName.Substring(root.Directory.FullName.Length).Replace('\\', '/');
                return new FullPath() { File = file, Root = root, RelativePath = root.Alias + parentPath };
            }
        }

        /// <summary> </summary>
        private void DirectoryCopy(DirectoryInfo sourceDir, string destDirName, bool copySubDirs) {
            DirectoryInfo[] dirs = sourceDir.GetDirectories();

            // If the source directory does not exist, throw an exception.
            if (!sourceDir.Exists) {
                throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDir.FullName);
            }

            // If the destination directory does not exist, create it.
            if (!Directory.Exists(destDirName)) {
                Directory.CreateDirectory(destDirName);
            }

            // Get the file contents of the directory to copy.
            FileInfo[] files = sourceDir.GetFiles();

            foreach (FileInfo file in files) {
                // Create the path to the new copy of the file.
                string temppath = Path.Combine(destDirName, file.Name);

                // Copy the file.
                file.CopyTo(temppath, false);
            }

            // If copySubDirs is true, copy the subdirectories.
            if (copySubDirs) {

                foreach (DirectoryInfo subdir in dirs) {
                    // Create the subdirectory.
                    string temppath = Path.Combine(destDirName, subdir.Name);

                    // Copy the subdirectories.
                    DirectoryCopy(subdir, temppath, copySubDirs);
                }
            }
        }

        #endregion

        #region public

        /// <summary>
        /// Initialize new instance of class ElFinder.FileSystemDriver 
        /// </summary>
        public FileSystemDriver() {
            _roots = new List<Root>();
        }

        /// <summary>
        /// Adds an object to the end of the roots.
        /// </summary>
        /// <param name="item"></param>
        public void AddRoot(Root item) {
            _roots.Add(item);
            item.VolumeId = _volumePrefix + _roots.Count + "_";
        }

        /// <summary>
        /// Gets collection of roots
        /// </summary>
        public IEnumerable<Root> Roots { get { return _roots; } }
        #endregion public

        #region   IDriver
        /// <summary> </summary>
        JsonResult IDriver.retrive_object(string target) {
            FullPath fullPath = ParsePath(target);
            OpenResponse answer = new OpenResponse(DTOBase.Create(fullPath.File.Directory, fullPath.Root), fullPath);
            answer.AddResponse(DTOBase.Create(fullPath.File, fullPath.Root));
            return Json(answer);
        }


        /// <summary> </summary>
        JsonResult IDriver.Open(string target, bool tree, Hashtable posting_json_obj) {
            FullPath fullPath = ParsePath(target);
            OpenResponse answer = new OpenResponse(DTOBase.Create(fullPath.Directory, fullPath.Root), fullPath);
            foreach (var item in fullPath.Directory.GetFiles()) {
                if (HttpContext.Current.Request.Params.AllKeys.Contains("mimes[]")) {

                    if (file_mime.mime_type(item.Extension.Trim('.')).IndexOf(HttpContext.Current.Request.Params["mimes[]"]) > -1) {
                        answer.AddResponse(DTOBase.Create(item, fullPath.Root, posting_json_obj));
                    }
                } else {
                    answer.AddResponse(DTOBase.Create(item, fullPath.Root, posting_json_obj));
                }
            }
            foreach (var item in fullPath.Directory.GetDirectories()) {
                answer.AddResponse(DTOBase.Create(item, fullPath.Root));
            }
            return Json(answer);
        }
        /// <summary> </summary>
        JsonResult IDriver.Init(string target, Hashtable posting_json_obj) {
            Root root;
            DirectoryInfo dir;
            if (string.IsNullOrEmpty(target)) {
                root = _roots.First();
                dir = root.Directory;
            } else {
                FullPath fullPath = ParsePath(target);
                root = fullPath.Root;
                dir = fullPath.Directory;
            }
            InitResponse answer = new InitResponse(DTOBase.Create(dir, root));


            foreach (var item in dir.GetFiles()) {
                if (HttpContext.Current.Request.Params.AllKeys.Contains("mimes[]")) {
                    if (file_mime.mime_type(item.Extension.Trim('.')).IndexOf(HttpContext.Current.Request.Params["mimes[]"]) > -1) {
                        answer.AddResponse(DTOBase.Create(item, root, posting_json_obj));
                    }
                } else {
                    answer.AddResponse(DTOBase.Create(item, root, posting_json_obj));
                }
            }
            foreach (var item in dir.GetDirectories()) {
                answer.AddResponse(DTOBase.Create(item, root));
            }
            foreach (var item in _roots) {
                answer.AddResponse(DTOBase.Create(item.Directory, item));
            }
            foreach (var item in root.Directory.GetDirectories()) {
                answer.AddResponse(DTOBase.Create(item, root));
            }
            //if (dir.FullName != root.Directory.FullName)
            //{
            //    foreach (var item in root.Directory.GetDirectories())
            //    {
            //        if (item.FullName == root.Directory.FullName)
            //        {
            //            foreach (var rootSubDir in item.GetDirectories())
            //            {
            //                if (rootSubDir.FullName != dir.FullName)
            //                {
            //                    answer.AddResponse(DTOBase.Create(rootSubDir, item));
            //                }
            //            }
            //        }
            //    }
            //}
            string parentPath = string.IsNullOrEmpty(target) ? root.Alias : root.Alias + dir.FullName.Substring(root.Directory.FullName.Length).Replace('\\', '/');
            answer.options.path = parentPath;
            answer.options.url = root.Url;
            answer.options.tmbUrl = root.TmbUrl;
            return Json(answer);
        }
        /// <summary> </summary>
        JsonResult IDriver.Parents(string target) {
            FullPath fullPath = ParsePath(target);
            TreeResponse answer = new TreeResponse();
            if (fullPath.Directory.FullName == fullPath.Root.Directory.FullName) {
                answer.tree.Add(DTOBase.Create(fullPath.Directory, fullPath.Root));
            } else {
                DirectoryInfo parent = fullPath.Directory;
                foreach (var item in parent.Parent.GetDirectories()) {
                    answer.tree.Add(DTOBase.Create(item, fullPath.Root));
                }
                while (parent.FullName != fullPath.Root.Directory.FullName) {
                    parent = parent.Parent;
                    answer.tree.Add(DTOBase.Create(parent, fullPath.Root));
                }
            }
            return Json(answer);
        }
        /// <summary> </summary>
        JsonResult IDriver.Tree(string target) {
            FullPath fullPath = ParsePath(target);
            TreeResponse answer = new TreeResponse();
            foreach (var item in fullPath.Directory.GetDirectories()) {
                answer.tree.Add(DTOBase.Create(item, fullPath.Root));
            }
            return Json(answer);
        }
        /// <summary> </summary>
        JsonResult IDriver.List(string target) {
            FullPath fullPath = ParsePath(target);
            ListResponse answer = new ListResponse();
            foreach (var item in fullPath.Directory.GetFileSystemInfos()) {
                answer.list.Add(item.Name);
            }
            return Json(answer);
        }
        /// <summary> </summary>
        JsonResult IDriver.MakeDir(string target, string name) {
            FullPath fullPath = ParsePath(target);
            DirectoryInfo newDir = Directory.CreateDirectory(Path.Combine(fullPath.Directory.FullName, name));
            return Json(new AddResponse(newDir, fullPath.Root));
        }
        /// <summary> </summary>
        JsonResult IDriver.MakeFile(string target, string name) {
            FullPath fullPath = ParsePath(target);
            FileInfo newFile = new FileInfo(Path.Combine(fullPath.Directory.FullName, name));
            newFile.Create().Close();
            return Json(new AddResponse(newFile, fullPath.Root));
        }
        /// <summary> </summary>
        JsonResult IDriver.Rename(string target, string name) {
            FullPath fullPath = ParsePath(target);
            var answer = new ReplaceResponse();
            answer.removed.Add(target);
            if (fullPath.Directory != null) {
                fullPath.Directory.MoveTo(Path.Combine(fullPath.Directory.Parent.FullName, name));
                var newDir = new DirectoryInfo(Path.Combine(fullPath.Directory.Parent.FullName, name));
                answer.added.Add(DTOBase.Create(newDir, fullPath.Root));

            } else {
                fullPath.File.MoveTo(Path.Combine(fullPath.File.Directory.FullName, name));
                var newFile = new FileInfo(Path.Combine(fullPath.File.Directory.FullName, name));
                answer.added.Add(DTOBase.Create(newFile, fullPath.Root));
            }
            return Json(answer);
        }
        /// <summary> </summary>
        JsonResult IDriver.Remove(IEnumerable<string> targets) {
            RemoveResponse answer = new RemoveResponse();
            foreach (var item in targets) {
                FullPath fullPath = ParsePath(item);
                if (fullPath.Directory != null) {
                    fullPath.Directory.Delete(true);
                } else {
                    fullPath.File.Delete();
                }
                answer.removed.Add(item);
            }
            return Json(answer);
        }
        /// <summary> </summary>
        JsonResult IDriver.Get(string target) {
            FullPath fullPath = ParsePath(target);
            GetResponse answer = new GetResponse();
            using (StreamReader reader = new StreamReader(fullPath.File.OpenRead())) {
                answer.content = reader.ReadToEnd();
            }
            return Json(answer);
        }
        /// <summary> </summary>
        JsonResult IDriver.Put(string target, string content) {
            FullPath fullPath = ParsePath(target);
            PutResponse answer = new PutResponse();
            using (StreamWriter writer = new StreamWriter(fullPath.File.FullName, false)) {
                writer.Write(content);
            }
            answer.changed.Add((FileDTO)DTOBase.Create(fullPath.File, fullPath.Root));
            return Json(answer);
        }
        /// <summary> </summary>
        JsonResult IDriver.Paste(string source, string dest, IEnumerable<string> targets, bool isCut) {
            FullPath destPath = ParsePath(dest);
            ReplaceResponse response = new ReplaceResponse();
            foreach (var item in targets) {
                FullPath src = ParsePath(item);
                if (src.Directory != null) {
                    DirectoryInfo newDir = new DirectoryInfo(Path.Combine(destPath.Directory.FullName, src.Directory.Name));
                    if (newDir.Exists)
                        Directory.Delete(newDir.FullName, true);
                    if (isCut) {
                        src.Directory.MoveTo(newDir.FullName);
                        response.removed.Add(item);
                    } else {
                        DirectoryCopy(src.Directory, newDir.FullName, true);
                    }
                    response.added.Add(DTOBase.Create(newDir, destPath.Root));
                } else {
                    string newFilePath = Path.Combine(destPath.Directory.FullName, src.File.Name);
                    if (File.Exists(newFilePath))
                        File.Delete(newFilePath);
                    if (isCut) {
                        src.File.MoveTo(newFilePath);
                        response.removed.Add(item);
                    } else {
                        File.Copy(src.File.FullName, newFilePath);
                    }
                    response.added.Add(DTOBase.Create(new FileInfo(newFilePath), destPath.Root));
                }
            }
            return Json(response);
        }
        /// <summary> </summary>
        JsonResult IDriver.Upload(string target, System.Web.HttpFileCollectionBase targets) {
            FullPath dest = ParsePath(target);
            var response = new AddResponse();
            for (int i = 0; i < targets.AllKeys.Length; i++) {
                HttpPostedFileBase file = targets[i];
                string path = Path.Combine(dest.Directory.FullName, file.FileName);
                if (File.Exists(path)) {
                    //TODO: throw error
                } else {
                    file.SaveAs(path);
                    response.added.Add((FileDTO)DTOBase.Create(new FileInfo(path), dest.Root));
                }
            }
            return Json(response);
        }
        /// <summary> </summary>
        JsonResult IDriver.Extract(string target, IEnumerable<string> targets) {
            FullPath dest = ParsePath(target);
            var response = new AddResponse();

            //this should move all files to a temp folder
            //then extract then there
            //then count them and get a list of the names
            //move them all in mass to the target folder
            //then respond with the file info


            foreach (var item in targets) {
                FullPath fullPath = ParsePath(item);

                FileInfo fileinfo = new FileInfo(fullPath.File.Name);


                DirectoryInfo newDir = Directory.CreateDirectory(file_info.site_cache_path() + "/temp/");
                file_handler.Decompress(fileinfo);


                response.added.Add((FileDTO)DTOBase.Create(fileinfo, dest.Root));
            }

            return Json(response);
        }



        /// <summary> </summary>
        JsonResult IDriver.Duplicate(IEnumerable<string> targets) {
            AddResponse response = new AddResponse();
            foreach (var target in targets) {
                FullPath fullPath = ParsePath(target);
                if (fullPath.Directory != null) {
                    var parentPath = fullPath.Directory.Parent.FullName;
                    var name = fullPath.Directory.Name;
                    var newName = string.Format(@"{0}\{1} copy", parentPath, name);
                    if (!Directory.Exists(newName)) {
                        DirectoryCopy(fullPath.Directory, newName, true);
                    } else {
                        for (int i = 1; i < 100; i++) {
                            newName = string.Format(@"{0}\{1} copy {2}", parentPath, name, i);
                            if (!Directory.Exists(newName)) {
                                DirectoryCopy(fullPath.Directory, newName, true);
                                break;
                            }
                        }
                    }
                    response.added.Add(DTOBase.Create(new DirectoryInfo(newName), fullPath.Root));
                } else {
                    var parentPath = fullPath.File.Directory.FullName;
                    var name = fullPath.File.Name.Substring(0, fullPath.File.Name.Length - fullPath.File.Extension.Length);
                    var ext = fullPath.File.Extension;

                    var newName = string.Format(@"{0}\{1} copy{2}", parentPath, name, ext);

                    if (!File.Exists(newName)) {
                        fullPath.File.CopyTo(newName);
                    } else {
                        for (int i = 1; i < 100; i++) {
                            newName = string.Format(@"{0}\{1} copy {2}{3}", parentPath, name, i, ext);
                            if (!File.Exists(newName)) {
                                fullPath.File.CopyTo(newName);
                                break;
                            }
                        }
                    }
                    response.added.Add(DTOBase.Create(new FileInfo(newName), fullPath.Root));
                }
            }
            return Json(response);
        }

        #endregion IDriver






    }
}