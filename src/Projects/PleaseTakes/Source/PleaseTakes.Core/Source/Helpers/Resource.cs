using System;
using System.Reflection;
using System.IO;

namespace PleaseTakes.Core.Helpers {

	internal sealed class Resource {
		private string _resourcePath;
		private Stream _stream;
		private bool _isBinary;

		public Resource(Helpers.Path.Parser path) {
			this._resourcePath = Resource.FormatPath(path);
			this.CompleteInitialisation();
		}

		public Resource(string path) {
			this._resourcePath = Resource.FormatPath(new Core.Helpers.Path.Parser(path));
			this.CompleteInitialisation();
		}

		private void CompleteInitialisation() {
			Assembly resourcesAssembly = Assembly.Load("PleaseTakes.Resources");
			this._stream = resourcesAssembly.GetManifestResourceStream(this._resourcePath);

			if (this._stream == null)
				throw new ResourceNotFoundException(this._resourcePath);

			if ((this.FileExtension.Equals("png")) || (this.FileExtension.Equals("jpg")) || (this.FileExtension.Equals("gif")) || (this.FileExtension.Equals("pdf")))
				this._isBinary = true;
			else
				this._isBinary = false;
		}

		private static string FormatPath(Helpers.Path.Parser path) {
			string returnStr = "PleaseTakes.Resources.";

			if (path.IsEmpty)
				throw new ArgumentNullException();

			for (int i = 0; (i <= (path.Count - 2)); i++) {
				string temp = path.Next();
				returnStr += temp.Substring(0, 1).ToUpper() + temp.Substring(1) + ".";
			}

			return returnStr + path.Next();
		}

		public bool IsBinary {
			get {
				return this._isBinary;
			}
		}

		public string FileExtension {
			get {
				return System.IO.Path.GetExtension(this._resourcePath).TrimStart('.').ToLower();
			}
		}

		public string Path {
			get {
				return this._resourcePath;
			}
		}

		public string ContentType {
			get {
				switch (this.FileExtension) {
					case "html":
					case "htm":
						return "text/html";
					case "htc":
						return "text/x-component";
					case "js":
						return "text/javascript";
					case "css":
						return "text/css";
					case "gif":
						return "image/gif";
					case "jpg":
						return "image/jpeg";
					case "png":
						return "image/png";
					case "pdf":
						return "application/pdf";
					default:
						return "text/plain";
				}
			}
		}

		public string StringRepresentation {
			get {
				if (this._isBinary)
					throw new InvalidResourceRequestException(this._resourcePath + ": file is binary.");

				return new StreamReader(this._stream).ReadToEnd();
			}
		}

		public byte[] ByteRepresentation {
			get {
				if (!this._isBinary)
					throw new InvalidResourceRequestException(this._resourcePath + ": file is plaintext.");

				int streamLength = Convert.ToInt32(this._stream.Length);
				byte[] returnArray = new byte[streamLength + 1];

				this._stream.Read(returnArray, 0, streamLength);
				this._stream.Close();

				return returnArray;
			}
		}

		public override string ToString() {
			return this._resourcePath;
		}
	}

	public class ResourceNotFoundException : Exception {
		public ResourceNotFoundException(string message) : base(message) { }
	}

	public class InvalidResourceRequestException : Exception {
		public InvalidResourceRequestException(string message) : base(message) { }
	}

}