﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using InterfaceOfAlgorithmAdaptersWithVisualizer;
using VizualAlgoGeom.ThreadSafeComponentHandling;

namespace VizualAlgoGeom
{
  public partial class AlgorithmControl : UserControl
  {
    public AlgorithmControl()
    {
      InitializeComponent();
    }

    void ArrangeInTreeForm(int depth, string item)
    {
      if (depth == 0)
      {
        treeViewPseudocode.Nodes.Add(item);
        return;
      }
      TreeNode tn = treeViewPseudocode.Nodes[treeViewPseudocode.Nodes.Count - 1];
      for (var j = 1; j < depth; j++)
      {
        if (tn.Nodes.Count > 0)
          tn = tn.Nodes[tn.Nodes.Count - 1];
      }
      tn.Nodes.Add(item);
    }

    internal void ArrangeInTreeView(List<IPseudocodeLine> list)
    {
      this.UiThreadExecute(
        () =>
        {
          foreach (IPseudocodeLine element in list)
            ArrangeInTreeForm(element.Depth, element.Text);
          treeViewPseudocode.ExpandAll();
        });
    }

    internal void EmptyTreeView()
    {
      this.UiThreadExecute(treeViewPseudocode.Nodes.Clear);
    }

    internal void SelectLine(int lineNumber)
    {
      this.UiThreadExecute(() =>
        {
          treeViewPseudocode.SelectedNode = FindTreeNodeAt(lineNumber);
          treeViewPseudocode.Focus();
        });
    }

    TreeNode FindTreeNodeAt(int lineNumber)
    {
      var currentLineNumber = 1;
      return FindTreeNodeAt(lineNumber, treeViewPseudocode.Nodes, ref currentLineNumber);
    }

    TreeNode FindTreeNodeAt(int lineNumber, TreeNodeCollection nodes, ref int currentLineNumber)
    {
      foreach (TreeNode node in nodes)
      {
        if (currentLineNumber == lineNumber)
        {
          return node;
        }
        ++ currentLineNumber;
        TreeNode nodeInSubItems = FindTreeNodeAt(lineNumber, node.Nodes, ref currentLineNumber);
        if (currentLineNumber > lineNumber || nodeInSubItems != null)
        {
          return nodeInSubItems;
        }
      }
      return null;
    }
  }
}