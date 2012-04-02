using System;
using System.IO;
using System.Text;
using System.Xml;
using HtmlAgilityPack;

public static class HtmlAgilityPackHelper
{
    #region Public static methods: Save(HtmlDocument), Save(HtmlNode) + overloads.

    public static void Save(string path, HtmlDocument document)
    {
        Save(path, document, false);
    }

    /// <summary>
    /// Saves the <see cref="HtmlDocument"/> to the specified path.
    /// </summary>
    /// <param name="path">The file to write to.</param>
    /// <param name="document">The <see cref="HtmlDocument"/> to save.</param>
    /// <param name="asXml">if set to <c>true</c> save document as XML.</param>
    /// <seealso cref="HtmlNode"/>
    /// <see cref="HtmlDocument"/>
    public static void Save(string path, HtmlDocument document, bool asXml)
    {
        if (path == null || document == null)
        {
            DebugBreakOrThrow("Figure out why " + (path == null ? "path" : "document") + "is null", new ArgumentNullException(path == null ? "path" : "document"));
        }

        // DeclaredEncoding is set if document.OptionReadEncoding is true, and a valid Content-Type is found.
        // When the OptionReadEncoding is true, HtmlDocument tries to find the encoding by reading the <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />.
        // If it's null, we use the default.
        using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read, 1024))
        {
            using (StreamWriter streamWriter = new StreamWriter(fileStream, document.DeclaredEncoding ?? document.Encoding))
            {
                if (asXml)
                {
                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.Encoding = document.DeclaredEncoding ?? document.Encoding;
                    settings.Indent = true;
                    settings.CloseOutput = true;
                    settings.OmitXmlDeclaration = true;
                    settings.NewLineOnAttributes = true;
                    settings.CheckCharacters = true;

                    settings.ConformanceLevel = ConformanceLevel.Fragment;

                    document.Save(XmlWriter.Create(streamWriter, settings));
                }
                else
                {
                    document.Save(streamWriter);
                }

                streamWriter.Flush();
            }
        }
    }

    /// <summary>
    /// Saves the <see cref="HtmlDocument"/> to the specified path.
    /// </summary>
    /// <param name="path">The file to write to.</param>
    /// <param name="node">The <see cref="HtmlNode"/> to save.</param>
    /// <remarks>This method will use the <see cref="HtmlNode.OwnerDocument">HtmlNode.OwnerDocument</see> to determine which encoding to use.</remarks>
    /// <seealso cref="Save(string, HtmlNode, Encoding)"/>
    /// <seealso cref="HtmlNode"/>
    /// <see cref="HtmlDocument"/>
    public static void Save(string path, HtmlNode node)
    {
        if (path == null || node == null)
        {
            DebugBreakOrThrow("Figure out why " + (path == null ? "path" : "node") + "is null", new ArgumentNullException(path == null ? "path" : "node"));
        }

        Save(path, node, node.OwnerDocument.DeclaredEncoding ?? node.OwnerDocument.Encoding);
    }

    /// <summary>
    /// Saves the <see cref="HtmlDocument"/> to the specified path.
    /// </summary>
    /// <param name="path">The file to write to.</param>
    /// <param name="node">The <see cref="HtmlNode"/> to save.</param>
    /// <param name="encoding">The encoding to use when writing the file.</param>
    /// <seealso cref="HtmlNode"/>
    /// <see cref="HtmlDocument"/>
    public static void Save(string path, HtmlNode node, Encoding encoding)
    {
        if (path == null || node == null)
        {
            DebugBreakOrThrow("Figure out why " + (path == null ? "path" : "node") + "is null", new ArgumentNullException(path == null ? "path" : "node"));
        }

        // Will only be triggered if the caller isn't called by another Save().
        if (encoding == null)
        {
            DebugBreakOrThrow("Figure out why encoding is null.", new ArgumentNullException("encoding"));
        }

        using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read, 1024))
        {
            using (StreamWriter streamWriter = new StreamWriter(fileStream, encoding))
            {
                // StreamWriter is a TextWriter, so we can pass it to HtmlNode.WriteContentTo(TextWriter).
                // So node.WriteTo only saves the current node, which is only useful if the node has no children.
                // node.WriteContentTo, only saves the current nodes children, therefore if the current node has a parent, we use the parent to save.
                // this will include all siblings aswell :S
                if (node.ParentNode != null)
                {
                    node.ParentNode.WriteContentTo(streamWriter);
                }
                else if (node.HasChildNodes == false)
                {
                    node.WriteTo(streamWriter);
                }
                else if (node.Name == HtmlNode.HtmlNodeTypeNameDocument)
                {
                    node.WriteContentTo(streamWriter);
                }
                else
                {
                    // TODO: Properly save parent-less node with children.
                    DebugBreakOrThrow("Properly save parent-less node with children. Inspect 'node'.", new InvalidOperationException("Don't know how to save the node, and it's children!"));
                }

                streamWriter.Flush();
            }
        }
    }
    #endregion

    private static void DebugBreakOrThrow(string What_needs_to_be_checked_at_this_point, Exception exception)
    {
        if (System.Diagnostics.Debugger.IsAttached)
        {
            // Open the Call-stack toolwindow: Debug -> Windows -> Call Stack. Double-click the next item in the list.
            System.Diagnostics.Debugger.Log(1, "HtmlAgilityPackHelper -> DebugBreakOrThrow()", "Supplied message: " + What_needs_to_be_checked_at_this_point ?? "None-supplied");
            System.Diagnostics.Debugger.Break();
        }
        else
        {
            throw exception;
        }
    }
}