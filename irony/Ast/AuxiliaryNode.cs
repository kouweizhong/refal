﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Ast;
using Irony.Parsing;
using Irony.Interpreter;

namespace Refal
{
	/// <summary>
	/// Temporary AST nodes used internally while building AST
	/// </summary>
	public class AuxiliaryNode : AstNode
	{
		public IList<ParseTreeNode> ChildParseNodes { get; private set; }

		public AuxiliaryNode()
		{
			ChildParseNodes = new List<ParseTreeNode>();
		}

		public override void Init(ParsingContext context, ParseTreeNode treeNode)
		{
			base.Init(context, treeNode);
			
			foreach (var node in treeNode.ChildNodes)
			{
				// linearize AuxiliaryNode structure
				if (node.AstNode is AuxiliaryNode)
				{
					var auxNode = (AuxiliaryNode)node.AstNode;
					foreach (var n in auxNode.ChildNodes)
						ChildNodes.Add(n);
					foreach (var n in auxNode.ChildParseNodes)
						ChildParseNodes.Add(n);
					continue;
				}

				// copy AST nodes
				if (node.AstNode is AstNode)
				{
					ChildNodes.Add(node.AstNode as AstNode);
					continue;
				}

				// otherwise, save parse nodes
				ChildParseNodes.Add(node);
			}
		}

		public override System.Collections.IEnumerable GetChildNodes()
		{
			throw new NotImplementedException("Auxiliary nodes should not appear in the final AST");
		}

		public override void EvaluateNode(EvaluationContext context, AstMode mode)
		{
			throw new NotImplementedException("Auxiliary node cannot be interpreted");
		}
	}
}
