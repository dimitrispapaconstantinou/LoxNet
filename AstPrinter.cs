using System;
using System.Collections.Generic;
using System.Text;
using LoxNet.AST;

namespace LoxNet
{
  class AstPrinter : Expr.IVisitor<string>
  {

    public string Print(Expr expr)
    {
      return expr.Accept(this);

    }
    public string VisitBinaryExpr(Expr.Binary expr)
    {
      return Parenthesize(expr.op.lexeme,expr.left, expr.right);
    }

    private string Parenthesize(string name, params Expr[] exprs)
    {
      StringBuilder builder = new StringBuilder();
      builder.Append("(").Append(name);

      foreach (var e in exprs)
      {
          builder.Append(" ");
          builder.Append(e.Accept(this));
      } 
      
      builder.Append(")"); 


      return builder.ToString();
    }

    public string VisitGroupingExpr(Expr.Grouping expr)
    {
        return Parenthesize("group", expr.expression);
    }

    public string VisitLiteralExpr(Expr.Literal expr)
    {
        return expr.value.ToString();
    }

    public string VisitUnaryExpr(Expr.Unary expr)
    {
        return Parenthesize(expr.op.lexeme, expr.right);
        }
  }
}
