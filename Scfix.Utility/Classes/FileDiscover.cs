using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScFix.Utility.Classes
{
    public class FileDiscover
    {
        #region Members

        protected Queue<string> _NonExploredDirectories = new Queue<string>();
        public Queue<string> NonExploredDirectories 
        {
            get 
            { 
                return _NonExploredDirectories;
            }
            set 
            {
                if (_NonExploredDirectories != value)
                    _NonExploredDirectories = value;
            }
        }

        protected List<string> _AcceptedFiles = new List<string>();
        public List<string> AcceptedFiles
        {
            get 
            { 
                return _AcceptedFiles; 
            }

            set 
            {
                if(_AcceptedFiles != value)
                    _AcceptedFiles = value; 
            }
        }

        protected List<String> _Extension;
        public List<String> Extensions
        {
            get
            {
                return _Extension;
            }
            set
            {
                if (_Extension != value)
                {
                    _Extension = value;
                }
            }
        }

        #endregion //Members
        #region Constructors

        public FileDiscover(String path, List<String> Extension)
        {
           
           this.Extensions = Extension;
           NonExploredDirectories.Enqueue(path);
        }

        #endregion //Constructors
        #region methods 
        public void Discover()
        {
            int count = 1;
            //these are needed to work, only would catch if you manually overrided those properties
            if (NonExploredDirectories != null && AcceptedFiles != null)
            {
                while (NonExploredDirectories.Count > 0)
                {
                    //Console.WriteLine("Number of Directories at before pop: {0}", NonExploredDirectories .Count);
                    // very much breath first search
                    string currentDirectory = NonExploredDirectories.Dequeue();
                    try
                    {

                        string[] directoriesInDirectory = Directory.GetDirectories(currentDirectory);

                        foreach (string s in directoriesInDirectory)
                        {
                            NonExploredDirectories.Enqueue(s);
                            count++;
                        }
                        Console.WriteLine(NonExploredDirectories.Count);
                        string[] filesInDirectory = Directory.GetFiles(currentDirectory);
                        foreach (string s in filesInDirectory)
                        {
                            if (IsVaildExtesion(s))
                            {
                                AcceptedFiles.Add(s);
                            }
                            // if they have a master image I would most likely put catch it here
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception("File Directory Explorer Error", e);
                    }
                }
                Console.WriteLine("Total number of Directories in that root is: {0}", count);
                Console.WriteLine("Total number of the files in all directories is: {0}", AcceptedFiles.Count);
            }
        }
        private bool IsVaildExtesion(String s)
        {
            if (Extensions != null)
            {

                foreach (String ext in Extensions)
                {
                    if (s.Contains(ext))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion //methods
    }
}
