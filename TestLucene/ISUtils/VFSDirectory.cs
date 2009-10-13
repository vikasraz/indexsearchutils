using System;
using System.Collections.Generic;
using System.Text;
using Lucene.Net.Store;
using org.apache.commons.vfs;

namespace ISUtils
{
    /**
    * 扩展Lucene索引的存储,使用commons-vfs做为lucene的存储引擎
    */
    public class VFSDirectory : Directory
    {
        #region VARS
        private FileSystemManager fsManager;
        private FileObject directory;
        private FileObject locker;
        #endregion
        public VFSDirectory(string repositoryUri, string lockUri)
        {
            try
            {
                fsManager = VFS.getManager();
                directory = fsManager.resolveFile(repositoryUri);
                if (!directory.exists())
                {
                    directory.createFolder();
                }
                locker = fsManager.resolveFile(lockUri);
                if (!locker.exists())
                {
                    locker.createFolder();
                }
            }
            catch (System.IO.IOException ioe)
            {
                throw ioe;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public override void Close()
        {
            directory.close();
        }

        protected void finallize()
        {
            Close();
        }
        public ~VFSDirectory()
        {
            finallize();
        }

        public override IndexOutput CreateOutput(string name)
        {
            FileObject file=fsManager.resolveFile(directory,name);
            return new VFSIndexOutput(file);
        }

        public override IndexInput OpenInput(string name)
        {
            FileObject file = fsManager.resolveFile(directory, name);
            return new VFSIndexInput(file);
        }

        public override void DeleteFile(string name)
        {
            FileObject file = fsManager.resolveFile(directory, name);
            if (!file.delete())
            {
                throw new System.IO.IOException("Cannot delete " + file.ToString());
            }
        }

        public override bool FileExists(string name)
        {
            FileObject file = fsManager.resolveFile(directory, name);
            return file.exists();
        }

        public override long FileLength(string name)
        {
            FileObject file = fsManager.resolveFile(directory, name);
            return file.getContent().getSize();
        }

        public override long FileModified(string name)
        {
            FileObject file = fsManager.resolveFile(directory, name);
            return file.getContent().getLastModifiedTime();
        }

        public override string[] List()
        {
            throw new NotImplementedException();
        }

        public override Lock MakeLock(string name)
        {
            //return base.MakeLock(name);
            StringBuilder buf=GetLockPrefix();
            buf.Append("-");
            buf.Append(name);
            try 
            {
                FileObject lockFile = fsManager.resolveFile(locker, buf.ToString());
                //return new Lock(){
                //    public bool Obtain()
                //    {
                //        lockFile.
                //    }
                //}
            }
            catch(Exception)
            {
            }
        }
        private StringBuilder GetLockPrefix()
        {
            string dirName;//name to be hashed
            try
            {
                //dirName=directory.getURL().ToString();
            }
            catch (System.IO.IOException ioe)
            {
                throw ioe;
            }
            catch (Exception e)
            {
                throw e;
            }
            byte[] digest;
            lock(this)
            {
                //digest=
            }
        }
        public override void RenameFile(string from, string to)
        {
            FileObject fileFrom = fsManager.resolveFile(directory, from);
            FileObject fileTo = fsManager.resolveFile(directory, to);
            try
            {
                //               
            }
            finally
            {
                Close(fileFrom);
                Close(fileTo);
            }
        }

        public override void TouchFile(string name)
        {
            FileObject file = fsManager.resolveFile(directory, name);
            file.getContent().setLastModifiedTime(DateTime.Now.ToFileTime());
        }

        private void Close(FileObject file)
        {
            if (file != null)
            {
                try
                {
                    file.close();
                }
                catch (System.IO.IOException ioe)
                {
                    throw ioe;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
    }
    static class VFSIndexInput : BufferedIndexInput
    {
        private long length;
        private RandomAccessContent content;
        public VFSIndexInput(FileObject file)
        {
            try
            {
                content = file.getContent().getRandomAccessContent();
                length = content.length;
            }
            catch (System.IO.IOException ioe)
            {
                throw ioe;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public override void Close()
        {
            content.close();
        }
        public override long Length()
        {
            return length;
        }
        protected override void ReadInternal(byte[] b, int offset, int len)
        {
            content.Read(b, offset, len);
        }
        protected override void SeekInternal(long pos)
        {
            content.seek(pos);
        }
    }
    static class VFSIndexOutput:BufferedIndexOutput
    {
        private RandomAccessContent content;
        public VFSIndexOutput(FileObject file)
        {
            try
            {
                if (!file.exists())
                    file.createFile();
                content = file.getContent().getRandomAccessContent();
            }
            catch (System.IO.IOException ioe)
            {
                throw ioe;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public override void FlushBuffer(byte[] b, int offset, int len)
        {
            content.Write(b,offset,len);
        }
        public override void Seek(long pos)
        {
            base.Seek(pos);
            content.seek(pos);
        }
        public override long Length()
        {
            return content.length();
        }
        public override void Close()
        {
            base.Close();
            content.close();
        }
        public void finalize()
        {
            Close();
        }
        public ~VFSIndexOutput()
        {
            finalize();
        }
    }
}
