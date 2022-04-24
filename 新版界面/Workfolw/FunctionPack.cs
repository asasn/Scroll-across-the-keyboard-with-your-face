using RootNS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RootNS.Workfolw
{
    /// <summary>
    /// 函数集合
    /// </summary>
    internal class FunctionPack
    {
        public static void AddToTreeEnd(Node rootNode, Node selectedNode)
        {
            if (rootNode.ChildNodes.Last<Node>().IsDir == true)
            {
                rootNode.ChildNodes.Last<Node>().ChildNodes.Add(selectedNode);
            }
            else
            {
                rootNode.ChildNodes.Add(selectedNode);
            }
        }


    }
}
