using System;
using System.Collections.Generic;
using System.Text;
using LoxNet.Tokens;

/*
 * expression     → literal
               | unary
               | binary
               | grouping ;

literal        → NUMBER | STRING | "true" | "false" | "nil" ;
grouping       → "(" expression ")" ;
unary          → ( "-" | "!" ) expression ;
binary         → expression operator expression ;
operator       → "==" | "!=" | "<" | "<=" | ">" | ">="
               | "+"  | "-"  | "*" | "/" ;

------------------------------------------------------------

expression     → equality ;
equality       → comparison ( ( "!=" | "==" ) comparison )* ;
comparison     → term ( ( ">" | ">=" | "<" | "<=" ) term )* ;
term           → factor ( ( "-" | "+" ) factor )* ;
factor         → unary ( ( "/" | "*" ) unary )* ;
unary          → ( "!" | "-" ) unary | primary ;
primary        → NUMBER | STRING | "true" | "false" | "nil" | "(" expression ")" ;
 */
namespace LoxNet.AST
{
  abstract class Expr
  {
    public interface IVisitor<T>
    {
      public T VisitBinaryExpr(Binary expr);

      public T VisitGroupingExpr(Grouping expr);

      public T VisitLiteralExpr(Literal expr);

      public T VisitUnaryExpr(Unary expr);
    }

    public abstract T Accept<T>(IVisitor<T> visitor);


    public class Binary : Expr
    {
      public Binary(Expr left, Token op, Expr right)
      {
        this.left = left;
        this.op = op;
        this.right = right;
      }


      public Expr left { get; }
      public Token op { get; }
      public Expr right { get; }

      public override T Accept<T>(IVisitor<T> visitor)
      {
        return visitor.VisitBinaryExpr(this);
      }



    }

    public class Grouping : Expr
    {
      public Grouping(Expr expression)
      {
        this.expression = expression;
      }

      public Expr expression { get; }


      public override T Accept<T>(IVisitor<T> visitor)
      {
        return visitor.VisitGroupingExpr(this);
      }
    }


    public class Literal : Expr
    {
      public Literal(object value)
      {
        this.value = value;
      }

      public object value { get; }


      public override T Accept<T>(IVisitor<T> visitor)
      {
        return visitor.VisitLiteralExpr(this);
      }
    }


    public class Unary : Expr
    {
      public Unary(Token op, Expr right)
      {
        this.op = op;
        this.right = right;
      }

      public Token op { get; }
      public Expr right { get; }


      public override T Accept<T>(IVisitor<T> visitor)
      {
        return visitor.VisitUnaryExpr(this);
      }
    }
  }
}