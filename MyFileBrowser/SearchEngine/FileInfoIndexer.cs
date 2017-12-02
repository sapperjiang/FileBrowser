
using System.IO;
using System.Text.RegularExpressions;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;

namespace MyFileBrowser
{
    
    /*
 * Copyright 2005 dotlucene.net
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
	/// <summary>
	/// Summary description for Indexer.
	/// </summary>
	public abstract class FileInfoIndexer
	{
		protected IndexWriter writer;
		private string docRootDirectory;
		private string pattern;

		/// <summary>
		/// Creates a new index in <c>directory</c>. Overwrites the existing index in that directory.
		/// </summary>
		/// <param name="directory">Path to index (will be created if not existing).</param>
		public FileInfoIndexer(string directory)
		{
			writer = new IndexWriter(directory, new StandardAnalyzer(), true);
			writer.SetUseCompoundFile(true);
		}

		/// <summary>
		/// Add HTML files from <c>directory</c> and its subdirectories that match <c>pattern</c>.
		/// </summary>
		/// <param name="directory">Directory with the HTML files.</param>
		/// <param name="pattern">Search pattern, e.g. <c>"*.html"</c></param>
		public void AddDirectory(DirectoryInfo directory, string pattern)
		{
			this.docRootDirectory = directory.FullName;
			this.pattern = pattern;
			AddSubDirectory(directory);
         }

		private void AddSubDirectory(DirectoryInfo directory)
		{
			foreach (FileInfo fi in directory.GetFiles())//(pattern))
			{
				AddDocument(fi.FullName);
			}
			foreach (DirectoryInfo di in directory.GetDirectories())
			{
				AddSubDirectory(di);
			}
		}

        ///// <summary>
        ///// Loads, parses and indexes an HTML file.
        ///// </summary>
        ///// <param name="path"></param>
        //public void AddHtmlDocument(string path)
        //{
        //    Document doc = new Document();
        //    string html;
        //    using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
        //    {
        //        html = sr.ReadToEnd();
        //    }

        //    int relativePathStartsAt = this.docRootDirectory.EndsWith("\\") ? this.docRootDirectory.Length : this.docRootDirectory.Length + 1;
        //    string relativePath = path.Substring(relativePathStartsAt);

        //    doc.Add(Field.UnStored("text", parseHtml(html)));
        //    doc.Add(Field.Keyword("path", relativePath));
        //    doc.Add(Field.Text("title", getTitle(html)));
        //    writer.AddDocument(doc);
        //} 
        public virtual void AddDocument(string path)
        {
            Document doc = new Document();

            string fileName = Path.GetFileName(path);
            //doc.Add(new Field())
            //doc.add(new Field("title", "lucene introduction", Field.Store.YES, Field.Index.TOKENIZED));
            doc.Add(new Field("FileName", fileName, true, true, true));
            doc.Add(new Field("Path",path, true, true, true));
            //doc.Add(Field.UnStored("text", parseHtml(html)));
            //doc.Add(Field.Keyword("path", relativePath));
            //doc.Add(Field.Text("title", getTitle(html)));
            writer.AddDocument(doc);
        } 

		/// <summary>
		/// Very simple, inefficient, and memory consuming HTML parser. Take a look at Demo/HtmlParser in DotLucene package for a better HTML parser.
		/// </summary>
		/// <param name="html">HTML document</param>
		/// <returns>Plain text.</returns>
        //private string parseHtml(string html)
        //{
        //    string temp = Regex.Replace(html, "<[^>]*>", "");
        //    return temp.Replace("&nbsp;", " ");
        //}

        ///// <summary>
        ///// Finds a title of HTML file. Doesn't work if the title spans two or more lines.
        ///// </summary>
        ///// <param name="html">HTML document.</param>
        ///// <returns>Title string.</returns>
        //private string getTitle(string html)
        //{
        //    Match m = Regex.Match(html, "<title>(.*)</title>");
        //    if (m.Groups.Count == 2)
        //        return m.Groups[1].Value;
        //    return "(unknown)";
        //}

		/// <summary>
		/// Optimizes and save the index.
		/// </summary>
		public void Close()
		{
			writer.Optimize();
			writer.Close();
		}


	}
    public class CommonFileIndexer:FileInfoIndexer 
    {
        public CommonFileIndexer(string dir): base(dir)
		{
            writer = new IndexWriter(dir, new StandardAnalyzer(), true);
			writer.SetUseCompoundFile(true);
		}

    }
}
