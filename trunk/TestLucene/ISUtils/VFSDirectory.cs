using System;
using System.Collections.Generic;
using System.Security.Cryptography;
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
        ~VFSDirectory()
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
            FileObject[] files = directory.getChildren();
            List<string> list=new List<string>();
            foreach (FileObject file in files)
            {
                list.Add(file.URL.ToString());
            }
            return list.ToArray();
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
                return new VFSLock(lockFile, buf);
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        private static MessageDigestSupport DIGESTER;
        internal static char[] HEX_DIGITS = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
        
        private StringBuilder GetLockPrefix()
        {
            string dirName;//name to be hashed
            try
            {
                dirName=directory.URL.ToString();
            }
            catch (System.IO.IOException ioe)
            {
                throw ioe;
            }
            catch (Exception e)
            {
                throw e;
            }
            sbyte[] digest;

            lock (DIGESTER)
            {
                digest = DIGESTER.DigestData(ToSByteArray(ToByteArray(dirName)));
            }

            StringBuilder buf = new StringBuilder();
            buf.Append("lucene-");
            for (int i = 0; i < digest.Length; i++)
            {
                int b = digest[i];
                buf.Append(HEX_DIGITS[(b >> 4) & 0xf]);
                buf.Append(HEX_DIGITS[b & 0xf]);
            }
            return buf;
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
        internal class VFSIndexInput : BufferedIndexInput
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
        internal class VFSIndexOutput:BufferedIndexOutput
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
            ~VFSIndexOutput()
            {
                finalize();
            }
        }
        public class VFSLock:Lock
        {
            private FileObject lockFile = null;
            private StringBuilder buf = null;
            public VFSLock(FileObject file,StringBuilder buffer)
            {
                lockFile = file;
                buf = buffer;
            }
            public override bool Obtain()
            {
                lockFile.createFile();
                return lockFile.exists();
            }

            public override void Release()
            {
                try
                {
                    lockFile.delete();
                }
                catch (FileSystemException fse)
                {
                    throw new Exception("Cannot release lock:" + buf.ToString(), fse);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }

            public override bool IsLocked()
            {
                try
                {
                    return lockFile.exists();
                }
                catch (FileSystemException fse)
                {
                    throw new Exception("Cannot check locking status:" + buf.ToString(), fse);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            public override string ToString()
            {
                return "Lock@"+lockFile.ToString();
            }
        }
        internal class MessageDigestSupport
        {
            private HashAlgorithm algorithm;
            private byte[] data = new byte[0];
            private int position;
            private string algorithmName;

            /// <summary>
            /// The HashAlgorithm instance that provide the cryptographic hash algorithm
            /// </summary>
            public HashAlgorithm Algorithm
            {
                get
                {
                    return algorithm;
                }
                set
                {
                    algorithm = value;
                }
            }

            /// <summary>
            /// The digest data
            /// </summary>
            public byte[] Data
            {
                get
                {
                    return this.data;
                }
                set
                {
                    this.data = value;
                }
            }

            /// <summary>
            /// The name of the cryptographic hash algorithm used in the instance
            /// </summary>
            public string AlgorithmName
            {
                get
                {
                    return algorithmName;
                }
            }

            /// <summary>
            /// Creates a message digest using the specified name to set Algorithm property.
            /// </summary>
            /// <param name="algorithm">The name of the algorithm to use</param>
            public MessageDigestSupport(string algorithm)
            {
                if (algorithm.Equals("SHA-1"))
                {
                    algorithmName = "SHA";
                }
                else
                {
                    algorithmName = algorithm;
                }
                Algorithm = (HashAlgorithm)CryptoConfig.CreateFromName(algorithmName);
                data = new byte[0];
                position = 0;
            }

            /// <summary>
            /// Computes the hash value for the internal data digest.
            /// </summary>
            /// <returns>The array of signed bytes with the resulting hash value</returns>
            public sbyte[] DigestData()
            {
                sbyte[] result = ToSByteArray(this.Algorithm.ComputeHash(this.data));
                Reset();
                return result;
            }

            /// <summary>
            /// Performs and update on the digest with the specified array and then completes the digest
            /// computation.
            /// </summary>
            /// <param name="newData">The array of bytes for final update to the digest</param>
            /// <returns>An array of signed bytes with the resulting hash value</returns>
            public sbyte[] DigestData(sbyte[] newData)
            {
                Update(ToByteArray(newData));
                return DigestData();
            }


            /// <summary>
            /// Computes the hash value for the internal digest and places the digest returned into the specified buffer
            /// </summary>
            /// <param name="buff">The buffer for the output digest</param>
            /// <param name="offset">Offset into the buffer for the beginning index</param>
            /// <param name="length">Total number of bytes for the digest</param>
            /// <returns>The number of bytes placed into the output buffer</returns>
            public int DigestData(sbyte[] buffer, int offset, int length)
            {
                byte[] result = this.Algorithm.ComputeHash(this.data);
                int count = 0;
                if (length >= this.GetDigestLength())
                {
                    if (buffer.Length >= (length + offset))
                    {
                        for (; count < result.Length; count++)
                        {
                            buffer[offset + count] = (sbyte)result[count];
                        }
                    }
                    else
                    {
                        throw new ArgumentException("output buffer too small for the specified offset and length");
                    }
                }
                else
                {
                    throw new Exception("Partial digests not returned");
                }
                return count;
            }

            /// <summary>
            /// Updates the digest data with the specified array of bytes by making an append
            /// operation in the internal array of data.
            /// </summary>
            /// <param name="newData">The array of bytes for the update operation</param>
            public void Update(byte[] newData)
            {
                if (position == 0)
                {
                    Data = newData;
                    position = this.Data.Length - 1;
                }
                else
                {
                    byte[] oldData = this.Data;
                    Data = new byte[newData.Length + position + 1];
                    oldData.CopyTo(Data, 0);
                    newData.CopyTo(Data, oldData.Length);

                    position = Data.Length - 1;
                }
            }

            /// <summary>
            /// Updates the digest data with the input byte by calling the method Update with an array.
            /// </summary>
            /// <param name="newData">The input byte for the update</param>
            public void Update(byte newData)
            {
                byte[] newDataArray = new byte[1];
                newDataArray[0] = newData;
                Update(newDataArray);
            }

            /// <summary>
            /// Updates the specified count of bytes with the input array of bytes starting at the
            /// input offset.
            /// </summary>
            /// <param name="newData">The array of bytes for the update operation</param>
            /// <param name="offset">The initial position to start from in the array of bytes</param>
            /// <param name="count">The number of bytes fot the update</param>
            public void Update(byte[] newData, int offset, int count)
            {
                byte[] newDataArray = new byte[count];
                Array.Copy(newData, offset, newDataArray, 0, count);
                Update(newDataArray);
            }

            /// <summary>
            /// Resets the digest data to the initial state.
            /// </summary>
            public void Reset()
            {
                data = null;
                position = 0;
            }

            /// <summary>
            /// Returns a string representation of the Message Digest
            /// </summary>
            /// <returns>A string representation of the object</returns>
            public override string ToString()
            {
                return this.Algorithm.ToString();
            }

            /// <summary>
            /// Generates a new instance of the MessageDigestSupport class using the specified algorithm
            /// </summary>
            /// <param name="algorithm">The name of the algorithm to use</param>
            /// <returns>A new instance of the MessageDigestSupport class</returns>
            public static MessageDigestSupport GetInstance(string algorithm)
            {
                return new MessageDigestSupport(algorithm);
            }

            /// <summary>
            /// Compares two arrays of signed bytes evaluating equivalence in digest data
            /// </summary>
            /// <param name="firstDigest">An array of signed bytes for comparison</param>
            /// <param name="secondDigest">An array of signed bytes for comparison</param>
            /// <returns>True if the input digest arrays are equal</returns>
            public static bool EquivalentDigest(sbyte[] firstDigest, sbyte[] secondDigest)
            {
                bool result = false;
                if (firstDigest.Length == secondDigest.Length)
                {
                    int index = 0;
                    result = true;
                    while (result && index < firstDigest.Length)
                    {
                        result = firstDigest[index] == secondDigest[index];
                        index++;
                    }
                }

                return result;
            }


            /// <summary>
            /// Gets a number of bytes representing the length of the digest
            /// </summary>
            /// <returns>The length of the digest in bytes</returns>
            public int GetDigestLength()
            {
                return algorithm.HashSize / 8;
            }
        }
        public static byte[] ToByteArray(sbyte[] sbyteArray)
        {
            byte[] byteArray = null;

            if (sbyteArray != null)
            {
                byteArray = new byte[sbyteArray.Length];
                for (int index = 0; index < sbyteArray.Length; index++)
                    byteArray[index] = (byte)sbyteArray[index];
            }
            return byteArray;
        }

        /// <summary>
        /// Converts a string to an array of bytes
        /// </summary>
        /// <param name="sourceString">The string to be converted</param>
        /// <returns>The new array of bytes</returns>
        public static byte[] ToByteArray(string sourceString)
        {
            return System.Text.UTF8Encoding.UTF8.GetBytes(sourceString);
        }

        /// <summary>
        /// Converts a array of object-type instances to a byte-type array.
        /// </summary>
        /// <param name="tempObjectArray">Array to convert.</param>
        /// <returns>An array of byte type elements.</returns>
        public static byte[] ToByteArray(object[] tempObjectArray)
        {
            byte[] byteArray = null;
            if (tempObjectArray != null)
            {
                byteArray = new byte[tempObjectArray.Length];
                for (int index = 0; index < tempObjectArray.Length; index++)
                    byteArray[index] = (byte)tempObjectArray[index];
            }
            return byteArray;
        }

        /*******************************/
        /// <summary>
        /// Receives a byte array and returns it transformed in an sbyte array
        /// </summary>
        /// <param name="byteArray">Byte array to process</param>
        /// <returns>The transformed array</returns>
        public static sbyte[] ToSByteArray(byte[] byteArray)
        {
            sbyte[] sbyteArray = null;
            if (byteArray != null)
            {
                sbyteArray = new sbyte[byteArray.Length];
                for (int index = 0; index < byteArray.Length; index++)
                    sbyteArray[index] = (sbyte)byteArray[index];
            }
            return sbyteArray;
        }
    }

}
