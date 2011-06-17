using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;
using System.Xml;

namespace HypeMachine
{
    public class StorageHelper<T>
    {
        private String fileName;
        private String lastModifiedFileName;

        public StorageHelper(String fileName)
        {
            this.fileName = fileName;
        }

        public StorageHelper(String fileName, String lastModifiedFileName)
        {
            this.fileName = fileName;
            this.lastModifiedFileName = lastModifiedFileName;
        }

        public Boolean Exists()
        {
            IsolatedStorageFile isoStorage = IsolatedStorageFile.GetUserStoreForApplication();
            return isoStorage.FileExists(this.fileName);
        }

        public void Delete()
        {
            IsolatedStorageFile isoStorage = IsolatedStorageFile.GetUserStoreForApplication();
            isoStorage.DeleteFile(this.fileName);
        }

        public List<T> LoadAll()
        {
            List<T> genericList = new List<T>();
            TextReader textReader = null;
            try
            {
                IsolatedStorageFile isoStorage = IsolatedStorageFile.GetUserStoreForApplication();
                IsolatedStorageFileStream file = isoStorage.OpenFile(this.fileName, FileMode.OpenOrCreate);
                DataContractSerializer serializer = new DataContractSerializer(typeof(List<T>));
                XmlDictionaryReader xmlReader = XmlDictionaryReader.CreateDictionaryReader(XmlReader.Create(file));
                genericList.AddRange((List<T>)serializer.ReadObject(xmlReader, false));
                textReader.Close();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            finally
            {
                if (textReader != null)
                    textReader.Dispose();
            }
            return genericList;
        }

        public void SaveAll(List<T> genericList)
        {
            TextWriter textWriter = null;
            try
            {
                IsolatedStorageFile isoStorage = IsolatedStorageFile.GetUserStoreForApplication();
                IsolatedStorageFileStream file = isoStorage.OpenFile(this.fileName, FileMode.Create);
                textWriter = new StreamWriter(file);
                DataContractSerializer serializer = new DataContractSerializer(typeof(List<T>));
                serializer.WriteObject(file, genericList);
                textWriter.Close();

                if (this.lastModifiedFileName != null)
                {
                    if (!this.lastModifiedFileName.Equals(String.Empty))
                    {
                        IsolatedStorageFileStream lastModifiedFile = isoStorage.OpenFile(this.lastModifiedFileName, FileMode.Create);
                        textWriter = new StreamWriter(lastModifiedFile);
                        textWriter.WriteLine(DateTime.Now.ToString());
                        textWriter.Close();
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            finally
            {
                if (textWriter != null)
                    textWriter.Dispose();
            }
        }

        public Boolean IsStale(TimeSpan shelfLife)
        {
            Boolean result = true;
            if (!this.lastModifiedFileName.Equals(String.Empty))
            {
                TextReader textReader = null;
                try
                {
                    IsolatedStorageFile isoStorage = IsolatedStorageFile.GetUserStoreForApplication();
                    IsolatedStorageFileStream file = isoStorage.OpenFile(this.lastModifiedFileName, FileMode.OpenOrCreate);
                    textReader = new StreamReader(file);
                    DateTime lastModifiedDate;
                    if (DateTime.TryParse(textReader.ReadLine(), out lastModifiedDate))
                    {
                        if ((DateTime.Now - lastModifiedDate) < shelfLife)
                        {
                            result = false;
                        }
                    }
                    textReader.Close();
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                }
                finally
                {
                    if (textReader != null)
                        textReader.Dispose();
                }
            }
            return result;
        }
    }
}