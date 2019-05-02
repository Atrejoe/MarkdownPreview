using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Windows.Forms;
using static MarkdownPreview.Core.Engine;

namespace MarkdownPreview.Demo
{
	public partial class Form : System.Windows.Forms.Form
	{

		public Form()
		{
			InitializeComponent();

			treeView.NodeMouseClick += treeView_NodeMouseClick;
			treeView.BeforeExpand += treeView_NodeExpand;
			treeView.AfterSelect += treeView_NodeSelected;

			PopulateTreeView();
		}

		private void treeView_NodeSelected(object sender, TreeViewEventArgs e)
		{
			TreeNode newSelected = e.Node;
			ShowContent(newSelected);
		}

		private void treeView_NodeExpand(object sender, TreeViewCancelEventArgs e)
		{
			foreach (TreeNode node in e.Node.Nodes)
				if ((node.Tag is DirectoryInfo dir))
					GetDirectories(dir, node);
		}

		private void PopulateTreeView()
		{

			foreach (var drive in DriveInfo.GetDrives())
			{
				var info = drive.RootDirectory;
				if (info.Exists)
				{
					var rootNode = new TreeNode(info.Name)
					{
						Tag = info
					};
					GetDirectories(info, rootNode);
					treeView.Nodes.Add(rootNode);

					rootNode.Expand();
				}
			}
		}

		//private static bool HasReadAccess(string path)
		//{

		//	FileIOPermission writePermission = new FileIOPermission(FileIOPermissionAccess.PathDiscovery, path);
		//	return SecurityManager.IsGranted(writePermission);
		//}

		public static bool HasReadAccess(string path)
		{
			var permissionSet = new PermissionSet(PermissionState.None);
			var writePermission = new FileIOPermission(FileIOPermissionAccess.PathDiscovery, path);
			permissionSet.AddPermission(writePermission);

			return permissionSet.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet);
		}

		private static void GetDirectories(DirectoryInfo dir, TreeNode nodeToAddTo)
		{

			if (!HasReadAccess(dir.FullName))
			{
				nodeToAddTo.Nodes.Add(
					new TreeNode("No access", 0, 0)
					{
						BackColor = Color.Gray
					});
				return;
			}

			try
			{
				foreach (DirectoryInfo subDir in dir.GetDirectories())
				{
					var aNode = new TreeNode(subDir.Name, 0, 0)
					{
						Tag = subDir,
						ImageKey = "folder"
					};

					nodeToAddTo.Nodes.Add(aNode);
				}


				var files = dir.GetFiles();
				if (files.Any())
					GetFiles(files, nodeToAddTo);
			}
			catch (UnauthorizedAccessException)
			{
				nodeToAddTo.Nodes.Add(
					new TreeNode("Access denied", 0, 0)
					{
						BackColor = Color.Gray
					});
			}
#pragma warning disable CA1031 // Catching all exceptions
			catch (Exception ex)

#pragma warning restore CA1031 // Do not catch general exception types
			{
				nodeToAddTo.Nodes.Add(
					new TreeNode(ex.Message, 0, 0)
					{
						BackColor = Color.Red
					});

				Trace.WriteLine($"{ex}");
			}
		}
		private static void GetFiles(FileInfo[] files, TreeNode nodeToAddTo)
		{
			foreach (var file in files)
				nodeToAddTo.Nodes.Add(
					new TreeNode(file.Name, 0, 0)
					{
						Tag = file,
						ImageKey = "file",
						ForeColor = IsMarkDown(file)
							? Color.Blue
							: Color.Black
					});

		}

		void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			TreeNode newSelected = e.Node;
			ShowContent(newSelected);

		}

		private void ShowContent(TreeNode newSelected)
		{
			string content;
			try
			{
				content = (newSelected.Tag is FileInfo file)
												? IsMarkDown(file)
													? file.Length > (10 * 1024 * 1024) // Let's not load 10MB MarkDown files
														? $"File exceeds maximum file length ({file.Length:n0} bytes)"
														: File.ReadAllText(file.FullName)
													: $"`{file.Name}` is no MarkDown file"
												: "No Markdown file selected";
			}
#pragma warning disable CA1031 // Meaning to catch all
			catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
			{
				content = $@"An error ocurred while reading file:
```
{ex}
```";
			}

			var html = MarkDownToHtml(content);

			webBrowser.DocumentText = html;
		}

		private static bool IsMarkDown(FileInfo nodeFileInfo)
		{
			return (new[] { ".md", ".markdown" }).Contains(nodeFileInfo.Extension, StringComparer.InvariantCultureIgnoreCase);
		}
	}

}
