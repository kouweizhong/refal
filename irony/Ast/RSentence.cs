using System;
using Irony.Ast;
using Irony.Parsing;
using System.Collections.Generic;
using Irony.Interpreter;
using Refal.Runtime;

namespace Refal
{
	/// <summary>
	/// RSentence is a helper class used internally
	/// It is not used in the AST
	/// </summary>
	public class RSentence : AuxiliaryNode
	{
		public Conditions Conditions { get; private set; }

		public Expression Expression { get; private set; }

		public override void Init(ParsingContext context, ParseTreeNode parseNode)
		{
			base.Init(context, parseNode);
			
			foreach (ParseTreeNode node in parseNode.ChildNodes)
			{
				if (node.AstNode is Expression)
				{
					Expression = node.AstNode as Expression;
				}
				else if (node.AstNode is Conditions)
				{
					Conditions = node.AstNode as Conditions;
				}
			}
		}
	}
}